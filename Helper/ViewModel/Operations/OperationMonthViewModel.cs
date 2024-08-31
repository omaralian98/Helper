namespace Helper.ViewModel.Operations;

public class OperationMonthViewModel
{
    public DateTime Date { get; set; }
    public List<OperationDayViewModel> Operations { get; set; } = [];
    public long TotalCost { get; set; }
    public int OperationCount { get; set; }

    public override string ToString() => Date.ToString("MMMM");
}
