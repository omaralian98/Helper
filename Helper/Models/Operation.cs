using System;
using System.ComponentModel.DataAnnotations;

namespace Helper.Models;

public class Operation : Entity
{
    [Required(ErrorMessage = "يجب أن يكون هناك عنوان للعملية")]
    [MinLength(1)]
    public string Title { get; set; }
    [Required]
    [Range(0, long.MaxValue)]
    public long Cost { get; set; } = ApplicationSettings.OperationDefaultValue;
    public string? Description { get; set; } = null;
}
