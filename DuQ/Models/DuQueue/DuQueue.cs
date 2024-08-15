namespace DuQ.Models.DuQueue;

public class DuQueue
{
    public Guid Id { get; set; }
    public required Student Student { get; set; }
    public required DuQueueStatus QueueStatus { get; set; }
    public required DuQueueLocation QueueLocation { get; set; }
    public DateTime CheckinTime { get; set; }
    public DateTime CheckoutTime { get; set; }
    public DateTime ModifiedUtc { get; set; }
}
