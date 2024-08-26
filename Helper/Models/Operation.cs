using System;
using System.ComponentModel.DataAnnotations;

namespace Helper.Models;

public class Operation
{
    public int Id { get; set; }

    [Required(ErrorMessage = "يجب أن يكون هناك عنوان للعملية")]
    [MinLength(1)]
    public string Title { get; set; }
    [Required]
    [Range(0, long.MaxValue)]
    public long Cost { get; set; } = 1_000;
    public string? Description { get; set; } = null;
    public DateTime Date { get; set; } = DateTime.Now;
    public string? Path { get; set; } = null;
}
