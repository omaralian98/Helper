namespace Helper.ViewModel.Operations;

public class OperationYearViewModel
{
    public DateTime Date { get; set; }
    public List<OperationMonthViewModel> Operations { get; set; } = [];
    public long TotalAmount { get; set; }
    public int OperationCount { get; set; }

    public override string ToString() => Date.ToString("yyyy");
}