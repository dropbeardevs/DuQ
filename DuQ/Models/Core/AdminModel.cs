using System.ComponentModel.DataAnnotations;

namespace DuQ.Models.Core;

public class AdminModel
{
    [Required (ErrorMessage = "Queue Location is required")]
    public string? Location { get; set; }
}
