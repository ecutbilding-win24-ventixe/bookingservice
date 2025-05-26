namespace Domain.Models;

public class Booking
{
    public string Id { get; set; } = null!;
    public string EventId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreateAt { get; set; }
    public string PackageTypeId { get; set; } = null!;
    public string EventPriceId { get; set; } = null!;
    public int Quantity { get; set; } = 1;
    public BookingStatus BookingStatus { get; set; } = null!;
}
