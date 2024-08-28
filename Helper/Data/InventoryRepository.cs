using Helper.Models;

namespace Helper.Data;

public class InventoryRepository : HelperDatabase<Inventory>
{
    public async Task<List<long>> GetMostFrequentlyAddedIncomes(int count)
    {
        var query = $@"
            SELECT 
                *
            FROM 
                [{nameof(Inventory)}]
            GROUP BY 
                [{nameof(Inventory.Income)}]
            ORDER BY 
                COUNT(*) DESC
            LIMIT {count};";


        var inventories = await Database.QueryAsync<Inventory>(query);
        return inventories.Select(op => op.Income).ToList();
    }
}
