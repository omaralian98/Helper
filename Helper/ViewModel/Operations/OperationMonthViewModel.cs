namespace Helper.ViewModel.Operations;

public class OperationMonthViewModel
{
    public DateTime Date { get; set; }
    public List<OperationDayViewModel> Operations { get; set; } = [];
    public long TotalAmount { get; set; }
    public int OperationCount { get; set; }

    public override string ToString() => Date.ToString("MMM", new CultureInfo("ar-sy"));
}
