namespace Data.Entities;

public class BookingStatusEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Name { get; set; } = null!;
}
