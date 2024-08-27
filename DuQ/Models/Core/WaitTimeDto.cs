namespace DuQ.Models.Core;

public class WaitTimeDto
{
    public Guid Id { get; set; }
    public string Location { get; set; }
    public int WaitTime { get; set; }
    public int NumberStudents { get; set; }
    public bool IsOpen { get; set; }
}
