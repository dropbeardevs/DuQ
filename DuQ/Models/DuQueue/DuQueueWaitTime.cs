namespace DuQ.Models.DuQueue;

public class DuQueueWaitTime
{
    public Guid Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public int WaitTime { get; set; }
    public int QueueNoStudents { get; set; }
    public DateTime ModifiedUtc { get; set; }
}
