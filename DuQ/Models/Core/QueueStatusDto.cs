namespace DuQ.Models.Core;

public class QueueStatusDto
{
    public string QueueLocation { get; set; } = string.Empty;
    public int QueueWaitTime { get; set; }
    public int QueueNoStudents { get; set;  }
    public int TotalWaitTime { get; set; }
}
