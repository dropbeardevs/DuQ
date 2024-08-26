namespace DuQ.Models.Core;

public class WaitTimeModel
{
    public Guid ArWaitTimeId { get; set; }
    public int ArWaitTime { get; set; }
    public int ArNoStudents { get; set; }
    public bool ArIsOpen { get; set; }
    public Guid ArWindowWaitTimeId { get; set; }
    public int ArWindowWaitTime { get; set; }
    public int ArWindowNoStudents { get; set; }
    public bool ArWindowIsOpen { get; set; }
    public Guid HrcWaitTimeId { get; set; }
    public int HrcWaitTime { get; set; }
    public int HrcNoStudents { get; set; }
    public bool HrcIsOpen { get; set; }
}
