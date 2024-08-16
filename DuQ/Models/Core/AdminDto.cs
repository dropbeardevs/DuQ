namespace DuQ.Models.Core;

public class AdminDto
{
    public Guid QueueId { get; set; }
    public required string QueueLocation { get; set; }
    public required string QueueType { get; set; }
    public required string StudentFirstName { get; set; }
    public required string StudentLastName { get; set; }
    public required string StudentContactDetails { get; set; }
    public required string QueueStatus { get; set; }
    public DateTime CheckinTime { get; set; }
    public DateTime CheckoutTime { get; set; }
    public DateTime Modified { get; set; }
}
