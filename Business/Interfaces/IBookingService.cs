using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IBookingService
{
    Task<BookingResult> CreateBookingAsync(CreateBookingRequest request);
    Task<BookingResult<IEnumerable<Booking>>> GetAllBookingsAsync();
    Task<BookingResult<Booking>> GetBookingByIdAsync(string bookingId);
}