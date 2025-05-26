using Business.Interfaces;
using Business.Models;
using Business.Models.EventRequest;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;
using System.Net.Http.Json;

namespace Business.Services;

public class BookingService(IBookingRepository bookingRepository, IHttpClientFactory httpClientFactory) : IBookingService
{
    private readonly IBookingRepository _bookingRepository = bookingRepository;
    private readonly HttpClient _eventClient = httpClientFactory.CreateClient("EventApi");

    public async Task<BookingResult> CreateBookingAsync(CreateBookingRequest request)
    {
        if (request == null)
            return new BookingResult { Succeeded = false, StatusCode = 400, Message = "Request cannot be null." };

        try
        {
            await _bookingRepository.BeginTransactionAsync();

            var eventApiResult = await _eventClient.GetFromJsonAsync<BookingResult<EventDetailRequest>>($"events/{request.EventId}/booking-details");
            //Chatgpt hjälpte lite
            if (eventApiResult == null || !eventApiResult.Succeeded || eventApiResult.Result == null)
                return new BookingResult { Succeeded = false, StatusCode = 404, Message = "Event not found or failed to retrieve." };

            var eventResponse = eventApiResult.Result;

            if (eventResponse == null)
                return new BookingResult { Succeeded = false, StatusCode = 404, Message = "Event not found." };

            var bookingResult = await _bookingRepository.GetAllAsync(where: b => b.EventId == request.EventId);
            if (!bookingResult.Succeeded || bookingResult.Result == null)
                return new BookingResult { Succeeded = false, StatusCode = 500, Message = "Failed to retrieve bookings." };

            var totalReserved = bookingResult.Result.Sum(b => b.Quantity);

            if (totalReserved + request.Quantity > eventResponse.Capacity)
                return new BookingResult { Succeeded = false, StatusCode = 400, Message = "Not enough capacity for the requested quantity." };

            var selectedPackage = eventResponse.Packages.FirstOrDefault(p =>
                p.EventPriceId == request.EventPriceId
            );

            if (selectedPackage == null)
                return new BookingResult { Succeeded = false, StatusCode = 400, Message = "Invalid package type or event price." };

            var bookingModel = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                EventId = request.EventId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                CreateAt = DateTime.UtcNow,
                PackageTypeId = request.PackageTypeId,
                EventPriceId = request.EventPriceId,
                Quantity = request.Quantity,
                BookingStatusId = 1
            };

            var entity = bookingModel.MapTo<BookingEntity>();

            var addResult = await _bookingRepository.AddAsync(entity);

            if (addResult.Succeeded)
            {
                await _bookingRepository.CommitTransactionAsync();
                return new BookingResult { Succeeded = true, StatusCode = 201, Message = "Booking created successfully." };
            }
            else
            {
                await _bookingRepository.RollbackTransactionAsync();
                return new BookingResult { Succeeded = false, StatusCode = 500, Message = "Failed to create booking." };
            }
        }
        catch (Exception ex)
        {
            await _bookingRepository.RollbackTransactionAsync();
            return new BookingResult { Succeeded = false, StatusCode = 500, Message = $"An error occurred: {ex.Message}" };
        }
    }

    public async Task<BookingResult<IEnumerable<Booking>>> GetAllBookingsAsync()
    {
        try
        {
            var result = await _bookingRepository.GetAllAsync(orderByDescending: true, sortBy: b => b.CreateAt, includes: [x => x.BookingStatus]);
            if (!result.Succeeded || result.Result == null)
                return new BookingResult<IEnumerable<Booking>> { Succeeded = false, StatusCode = 404, Message = "Event not found" };

            var bookings = result.Result.Select(b => b.MapTo<Booking>()).ToList();
            return new BookingResult<IEnumerable<Booking>> { Message = "Bookings retrieved successfully", Succeeded = true, StatusCode = 200, Result = bookings };
        }
        catch (Exception ex)
        {
            return new BookingResult<IEnumerable<Booking>> { Succeeded = false, StatusCode = 500, Message = $"An error occurred: {ex.Message}" };
        }
    }

    public async Task<BookingResult<Booking>> GetBookingByIdAsync(string bookingId)
    {
        if (string.IsNullOrEmpty(bookingId))
            return new BookingResult<Booking> { Succeeded = false, StatusCode = 400, Message = "Booking ID cannot be null or empty." };
        try
        {
            var result = await _bookingRepository.GetAsync(b => b.Id == bookingId, b => b.BookingStatus);
            if (!result.Succeeded || result.Result == null)
                return new BookingResult<Booking> { Succeeded = false, StatusCode = 404, Message = "Booking not found." };
            var booking = result.Result.MapTo<Booking>();
            return new BookingResult<Booking> { Succeeded = true, StatusCode = 200, Result = booking };
        }
        catch (Exception ex)
        {
            return new BookingResult<Booking> { Succeeded = false, StatusCode = 500, Message = $"An error occurred: {ex.Message}" };
        }
    }
}
