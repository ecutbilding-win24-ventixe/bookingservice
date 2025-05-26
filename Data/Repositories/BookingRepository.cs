using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class BookingRepository(DataContext context) : BaseRepository<BookingEntity, Booking>(context), IBookingRepository
{
}
