namespace Presentation.Model;

public class CreateBookingRequestViewModel
{
    public string EventId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PackageTypeId { get; set; } = null!;
    public string EventPriceId { get; set; } = null!;
    public int Quantity { get; set; } = 1;
}
