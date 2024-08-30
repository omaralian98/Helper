using System.ComponentModel.DataAnnotations;

namespace Helper.Models;

public class Inventory : Entity
{
    public string? Description { get; set; }
    [Required(ErrorMessage ="يجب إدخال قيمة الوارد")]
    [Range(0, long.MaxValue, ErrorMessage ="قيمة الوارد لا يمكن أن تكون سالبة")]
    public long Income { get; set; } = ApplicationSettings.InventoryDefaultValue;
}
