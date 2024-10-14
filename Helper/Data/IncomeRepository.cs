using Helper.Models;

namespace Helper.Data;

public class IncomeRepository : HelperDatabase<Record>
{
    public override async Task<int> AddAsync(Record income)
    {
        income.RecordType = RecordType.Income;
        return await base.AddAsync(income);
    }

    public async Task AddIncomeTrigger()
    {
        var query = $@"
            CREATE TRIGGER IF NOT EXISTS IncomeTrigger
            AFTER INSERT ON Record
            FOR EACH ROW
            WHEN NEW.RecordType = 1
            BEGIN
                UPDATE Inventory
                SET Balance = Balance + NEW.Amount
                WHERE Id = (SELECT Id FROM Inventory LIMIT 1);

                INSERT INTO Inventory (Balance)
                SELECT NEW.Amount
                WHERE NOT EXISTS (SELECT 1 FROM Inventory);
            END;       
        ";
        await Database.ExecuteAsync(query);
    }

    public async Task<List<long>> GetMostFrequentlyAddedIncomes(int count)
    {
        var query = $@"
            SELECT 
                *
            FROM 
                [{nameof(Record)}]
            WHERE
                [{nameof(Record.RecordType)}] = {((int)RecordType.Income)}
            GROUP BY 
                [{nameof(Record.Amount)}]
            ORDER BY 
                COUNT(*) DESC, [{nameof(Record.DateAsString)}] DESC
            LIMIT {count};";


        var inventories = await Database.QueryAsync<Record>(query);
        return inventories.Select(op => op.Amount).ToList();
    }
}
