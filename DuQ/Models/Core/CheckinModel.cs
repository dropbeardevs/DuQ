using System.ComponentModel.DataAnnotations;

namespace DuQ.Models.Core;

public class CheckinModel
{
    [Required (ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name too long (50 character limit).")]
    public string? FirstName { get; set; }
    [Required (ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name too long (50 character limit).")]
    public string? LastName { get; set; }
    [Required (ErrorMessage = "Contact Details are required")]
    [StringLength(50, ErrorMessage = "Contact Details too long (50 character limit).")]
    public string? ContactDetails { get; set; }
    [Required (ErrorMessage = "Queue Location is required")]
    public string? Location { get; set; }
}
