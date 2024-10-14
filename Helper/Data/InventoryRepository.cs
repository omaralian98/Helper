using Helper.Models;

namespace Helper.Data;

public class InventoryRepository : HelperDatabase<Inventory>
{

    public override async Task<Inventory?> GetAsync(int? id = null)
    {
        var current = await base.GetAsync(id);
        if (current is null)
        {
            current = new Inventory() { Balance = 0 };
            await AddAsync(current);
        }
        return current;
    }
}
