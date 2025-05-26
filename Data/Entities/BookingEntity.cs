using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class BookingEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string EventId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public string PackageTypeId { get; set; } = null!;
    public string EventPriceId { get; set; } = null!;
    public int Quantity { get; set; } = 1;

    [ForeignKey(nameof(BookingStatus))]
    public string BookingStatusId { get; set; } = null!;
    public BookingStatusEntity BookingStatus { get; set; } = null!;

}
