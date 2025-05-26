namespace Business.Models.EventRequest;

public class PackageRequest
{
    public string PackageTypeId { get; set; } = null!;
    public string EventPriceId { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = null!;
}