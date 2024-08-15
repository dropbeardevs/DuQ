namespace DuQ.Models.DuQueue;

public class DuQueueStatus
{
    public Guid Id { get; set; }
    public required string Status { get; set; }
    public List<DuQueue>? DuQueues { get; } = [];
    public DateTime ModifiedUtc { get; set; }
}
