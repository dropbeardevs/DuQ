namespace DuQ.Models.DuQueue;

public class DuQueueType
{
    public Guid Id { get; set; }
    public required string Type { get; set; }
    public List<DuQueue>? DuQueues { get; } = [];
    public DateTime ModifiedUtc { get; set; }
}
