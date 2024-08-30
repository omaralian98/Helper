using Helper.Models;
using Helper.ViewModel.Operations;
namespace Helper.Data;

public class OperationRepository : HelperDatabase<Operation>
{
    public async Task<List<string>> GetMostFrequentlyUsedOperations(int count)
    {
        //try
        //{
        //    var res = await GetAllDaysForSpecificMonth(2024, 8);
        //    var res2 = await GetAllDaysForSpecificMonth(2024, 7);
        //    var res3 = await GetAllDaysForSpecificMonth(2024, 9);
        //}
        //catch { }
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


        var operations = await Database.QueryAsync<Operation>(query);
        return operations.Select(op => op.Title).ToList();
    }

    //public async Task<List<OperationYearViewModel>> GetAllYears()
    //{
    //    //Get all the years and but don't populate their months
    //}

    //public async Task<OperationYearViewModel> GetYear(int year)
    //{
    //    //Get this year and populate it's months using GetAllMonthsForSpecificYear
    //}

    //private async Task<List<OperationMonthViewModel>> GetAllMonthsForSpecificYear(int year)
    //{
    //    //Get all the months of the given year but don't populate it's daysviewmodel
    //}

    //private async Task<OperationMonthViewModel> GetMonthForSpecificYear(int year, int month)
    //{
    //    //Get this month and populate it's days using GetAllDaysForSpecificMonth
    //}

    //private async Task<List<OperationDayViewModel>> GetAllDaysForSpecificMonth(int year, int month)
    //{
    //    var query = @"
    //    SELECT 
    //        strftime('%d', DateAsString) AS Day,
    //        SUM(Cost) AS TotalCost, 
    //        COUNT(*) AS OperationCount
    //    FROM Operation
    //    WHERE strftime('%Y', DateAsString) = ? 
    //      AND strftime('%m', DateAsString) = ?
    //    GROUP BY Day
    //    ORDER BY Day;";

    //    var result = await Database.QueryAsync<dynamic>(query, year.ToString(), month.ToString("D2"));

    //    var dayResults = new List<OperationDayViewModel>();

    //    foreach (var day in result)
    //    {
    //        dayResults.Add(new OperationDayViewModel
    //        {
    //            Date = new DateTime(year, month, int.Parse(day.Day)),
    //            TotalCost = day.TotalCost,
    //            OperationCount = day.OperationCount,
    //            Operations = new List<Operation>()
    //        });
    //    }

    //    return dayResults;
    //}



    public async Task<OperationDayViewModel> GetDayForSpecificMonth(int year, int month, int day)
    {
        var query = @"
        SELECT 
            * 
        FROM Operation
        WHERE strftime('%Y', DateAsString) = ? 
          AND strftime('%m', DateAsString) = ? 
          AND strftime('%d', DateAsString) = ?;";

        var operations = await Database.QueryAsync<Operation>(query, year.ToString(), month.ToString("D2"), day.ToString("D2"));

        return new OperationDayViewModel
        {
            Date = new DateTime(year, month, day),
            Operations = operations,
            TotalCost = operations.Sum(o => o.Cost),
            OperationCount = operations.Count
        };
    }
}
