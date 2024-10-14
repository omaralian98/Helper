using System.ComponentModel.DataAnnotations;

namespace Helper.Models;

public class Record : Entity
{
    public string? Title { get; set; } = null;
    [Required]
    [Range(0, long.MaxValue)]
    public long Amount { get; set; } = ApplicationSettings.OperationDefaultValue;
    public RecordType RecordType { get; set; }
    public string? Description { get; set; } = null;
}
