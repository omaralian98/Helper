using Helper.Models;
using Helper.ViewModel;
using Helper.ViewModel.Operations;
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
                COUNT(*) DESC, [{nameof(Operation.DateAsString)}] DESC
            LIMIT {count};";


        try
        {
            var operations = await Database.QueryAsync<Operation>(query);
            return operations.Select(op => op.Title).ToList();
        }
        catch { }
        return [];
    }

    public async Task<List<OperationYearViewModel>> GetAllYears()
    {
        var query = @"
            SELECT 
	            strftime('%Y', DateAsString) AS Year,
	            SUM(Cost) AS TotalCost, 
	            COUNT(*) AS OperationCount
            FROM 
	            Operation 
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
                    TotalCost = year.TotalCost,
                    OperationCount = year.OperationCount,
                    Operations = []
                });
            }
        }
        catch { }

        return yearResults;
    }

    //public async Task<OperationYearViewModel> GetYear(int year)
    //{
    //    var yearViewModel = new OperationYearViewModel
    //    {
    //        Date = new DateTime(year, 1, 1),
    //        Operations = await GetAllMonthsForSpecificYear(year)
    //    };

    //    yearViewModel.OperationCount = yearViewModel.Operations.Sum(d => d.OperationCount);
    //    yearViewModel.TotalCost = yearViewModel.Operations.Sum(d => d.TotalCost);

    //    return yearViewModel;
    //}

    public async Task<List<OperationMonthViewModel>> GetAllMonthsForSpecificYear(int year)
    {
        var query = @"
            SELECT 
	            strftime('%m', DateAsString) AS Month,
	            SUM(Cost) AS TotalCost, 
	            COUNT(*) AS OperationCount
            FROM 
	            Operation 
            WHERE 
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
                    TotalCost = month.TotalCost,
                    OperationCount = month.OperationCount,
                    Operations = []
                });
            }
        }
        catch { }
        return monthResults;
    }

    //public async Task<OperationMonthViewModel> GetMonthForSpecificYear(int year, int month)
    //{
    //    var monthViewModel = new OperationMonthViewModel
    //    {
    //        Date = new DateTime(year, month, 1),
    //        Operations = await GetAllDaysForSpecificMonth(year, month)
    //    };

    //    monthViewModel.OperationCount = monthViewModel.Operations.Sum(d => d.OperationCount);
    //    monthViewModel.TotalCost = monthViewModel.Operations.Sum(d => d.TotalCost);

    //    return monthViewModel;
    //}

    public async Task<List<OperationDayViewModel>> GetAllDaysForSpecificMonth(int year, int month)
    {
        var query = @"
            SELECT 
	            strftime('%d', DateAsString) AS Day,
	            SUM(Cost) AS TotalCost, 
	            COUNT(*) AS OperationCount
            FROM 
	            Operation 
            WHERE 
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
                    TotalCost = day.TotalCost,
                    OperationCount = day.OperationCount,
                    Operations = []
                });
            }
        }
        catch { }
        return dayResults;
    }



    public async Task<List<Operation>> GetAllOperationsForSpecificDay(int year, int month, int day)
    {
        var query = @"
            SELECT 
                * 
            FROM 
                Operation
            WHERE 
                strftime('%Y', DateAsString) = ? 
                AND 
                strftime('%m', DateAsString) = ? 
                AND 
                strftime('%d', DateAsString) = ?;";
        try
        {

            var operations = await Database.QueryAsync<Operation>(query, year.ToString(), month.ToString("D2"), day.ToString("D2"));
            return operations;
        }
        catch { }
        return [];
    }
}
