using Helper.Models;
using Helper.ViewModel;
using Helper.ViewModel.Operations;
namespace Helper.Data;

public class OperationRepository(InventoryRepository inventoryRepository) : HelperDatabase<Record>
{
    public override async Task<int> AddAsync(Record operation)
    {
        if (string.IsNullOrWhiteSpace(operation.Title))
            return 0;

        var currentBalance = (await inventoryRepository.GetAsync())?.Balance;
        if (currentBalance < operation.Amount)
            return 0;

        operation.RecordType = RecordType.Operation;
        return await base.AddAsync(operation);
    }

    public async Task AddOperationTrigger()
    {
        var query = $@"
            CREATE TRIGGER IF NOT EXISTS OperationTrigger
            BEFORE INSERT ON Record
            FOR EACH ROW
            WHEN NEW.RecordType = 0  -- 0 corresponds to RecordType.Operation
            BEGIN
                UPDATE Inventory
                SET Balance = Balance - NEW.Amount
                WHERE Id = (SELECT Id FROM Inventory LIMIT 1);
            END;
        ";
        await Database.ExecuteAsync(query);
    }

    public async Task<List<string>> GetMostFrequentlyUsedOperations(int count)
    {
        var query = $@"
            SELECT 
                *
            FROM 
                [{nameof(Record)}]
            WHERE
                [{nameof(Record.RecordType)}] = {((int)RecordType.Operation)}
            GROUP BY 
                [{nameof(Record.Title)}]
            ORDER BY 
                COUNT(*) DESC, [{nameof(Record.DateAsString)}] DESC
            LIMIT {count};";


        try
        {
            var operations = await Database.QueryAsync<Record>(query);
            return operations.Select(op => op.Title).ToList();
        }
        catch { }
        return [];
    }

    public async Task<List<OperationYearViewModel>> GetAllYears()
    {
        var query = $@"
            SELECT 
	            strftime('%Y', DateAsString) AS Year,
	            SUM(Amount) AS TotalAmount, 
	            COUNT(*) AS OperationCount
            FROM 
                [{nameof(Record)}]
            WHERE
                [{nameof(Record.RecordType)}] = {((int)RecordType.Operation)}
            GROUP BY Year
            ORDER BY Year DESC;";

        var yearResults = new List<OperationYearViewModel>();

        try
        {
            var result = await Database.QueryAsync<GetAllYears>(query);

            foreach (var year in result)
            {
                yearResults.Add(new OperationYearViewModel
                {
                    Date = new DateTime(year.Year, 1, 1),
                    TotalAmount = year.TotalAmount,
                    OperationCount = year.OperationCount,
                    Operations = []
                });
            }
        }
        catch { }

        return yearResults;
    }

    public async Task<List<OperationMonthViewModel>> GetAllMonthsForSpecificYear(int year)
    {
        var query = $@"
            SELECT 
	            strftime('%m', DateAsString) AS Month,
	            SUM(Amount) AS TotalAmount, 
	            COUNT(*) AS OperationCount
            FROM 
                [{nameof(Record)}]
            WHERE
                [{nameof(Record.RecordType)}] = {((int)RecordType.Operation)} 
                AND
	            CAST(strftime('%Y', DateAsString) AS INTEGER) = ?
            GROUP BY Month
            ORDER BY Month;";

        var monthResults = new List<OperationMonthViewModel>();

        try
        {
            var result = await Database.QueryAsync<GetAllMonthsForSpecificYear>(query, year.ToString());


            foreach (var month in result)
            {
                monthResults.Add(new OperationMonthViewModel
                {
                    Date = new DateTime(year, month.Month, 1),
                    TotalAmount = month.TotalAmount,
                    OperationCount = month.OperationCount,
                    Operations = []
                });
            }
        }
        catch { }
        return monthResults;
    }

    public async Task<List<OperationDayViewModel>> GetAllDaysForSpecificMonth(int year, int month)
    {
        var query = $@"
            SELECT 
	            strftime('%d', DateAsString) AS Day,
	            SUM(Amount) AS TotalAmount, 
	            COUNT(*) AS OperationCount
            FROM 
                [{nameof(Record)}]
            WHERE
                [{nameof(Record.RecordType)}] = {((int)RecordType.Operation)}
                AND
	            CAST(strftime('%Y', DateAsString) AS INTEGER) = ?
	            AND
	            CAST(strftime('%m', DateAsString) AS INTEGER) = ?
            GROUP BY Day
            ORDER BY Day;";

        var dayResults = new List<OperationDayViewModel>();

        try
        {
            var result = await Database.QueryAsync<GetAllDaysForSpecificMonth>(query, year.ToString(), month.ToString());


            foreach (var day in result)
            {
                dayResults.Add(new OperationDayViewModel
                {
                    Date = new DateTime(year, month, day.Day),
                    TotalAmount = day.TotalAmount,
                    OperationCount = day.OperationCount,
                    Operations = []
                });
            }
        }
        catch { }
        return dayResults;
    }



    public async Task<List<Record>> GetAllOperationsForSpecificDay(int year, int month, int day, bool getIncome = false)
    {
        var query = $@"
            SELECT 
                * 
            FROM 
                [{nameof(Record)}]
            WHERE
                {(
                    getIncome ? "" : 
                    $@"[{nameof(Record.RecordType)}] = {((int)RecordType.Operation)}
                    AND"
                )}
                strftime('%Y', DateAsString) = ? 
                AND 
                strftime('%m', DateAsString) = ? 
                AND 
                strftime('%d', DateAsString) = ?;";
        try
        {
            var operations = await Database.QueryAsync<Record>(query, year.ToString(), month.ToString("D2"), day.ToString("D2"));
            return operations;
        }
        catch { }
        return [];
    }
}
