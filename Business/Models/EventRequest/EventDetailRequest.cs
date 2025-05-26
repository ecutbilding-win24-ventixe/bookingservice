namespace Business.Models.EventRequest;

public class EventDetailRequest
{
    public string Id { get; set; } = null!;
    public int Capacity { get; set; }
    public List<PackageRequest> Packages { get; set; } = new();
}
