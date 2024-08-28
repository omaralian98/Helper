using Helper.Models;

namespace Helper.Data;

public class OperationRepository : HelperDatabase<Operation>
{
    public async Task<List<string>> GetMostFrequentlyUsedOperations(int count)
    {
        var query = $@"
            SELECT 
                *
            FROM 
                [{nameof(Operation)}]
            GROUP BY 
                [{nameof(Operation.Title)}]
            ORDER BY 
                COUNT(*) DESC
            LIMIT {count};";


        var operations = await Database.QueryAsync<Operation>(query);
        return operations.Select(op => op.Title).ToList();
    }
}
