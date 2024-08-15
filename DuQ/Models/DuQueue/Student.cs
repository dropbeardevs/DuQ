namespace DuQ.Models.DuQueue;

public class Student
{
    public Guid Id { get; set; }
    public string? StudentNo { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string ContactDetails { get; set; }
    public List<DuQueue>? DuQueues { get; } = [];
    public DateTime ModifiedUtc { get; set; }
}
