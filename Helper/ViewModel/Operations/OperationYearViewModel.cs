using Helper.Models;

namespace Helper.ViewModel.Operations;

public class OperationYearViewModel
{
    public DateTime Date { get; set; }
    public IEnumerable<OperationDayViewModel> Operations { get; set; } = [];
    public long TotalSpent { get; set; }
    public int OperationCount { get; set; }
    public string DayName => Date.ToString("yyyy");
}


public class OperationMonthViewModel
{
    public DateTime Date { get; set; }
    public IEnumerable<OperationDayViewModel> Operations { get; set; } = [];
    public long TotalSpent { get; set; }
    public int OperationCount { get; set; }
    public string DayName => Date.ToString("MMMM");
}


public class OperationDayViewModel
{
    public DateTime Date { get; set; }
    public IEnumerable<Operation> Operations { get; set; } = [];
    public long TotalCost { get; set; }
    public int OperationCount { get; set; }
    public string DayName => Date.ToString("dd");
}
