namespace DuQ.Models.DuQueue;

public class DuQueueLocation
{
    public Guid Id { get; set; }
    public required string Location { get; set; }
    public List<DuQueue>? DuQueues { get; } = [];
    public DateTime ModifiedUtc { get; set; }
}
