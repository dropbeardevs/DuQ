namespace DuQ.Models.Core;

public class WaitTimeModel
{
    public Guid ArWaitTimeId { get; set; }
    public int ArWaitTime { get; set; }
    public int ArNoStudents { get; set; }
    public Guid HrcWaitTimeId { get; set; }
    public int HrcWaitTime { get; set; }
    public int HrcNoStudents { get; set; }
}
