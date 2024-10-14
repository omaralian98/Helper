using Helper.Models;

namespace Helper.ViewModel.Operations;

public class OperationDayViewModel
{
    public DateTime Date { get; set; }
    public List<Record> Operations { get; set; } = [];
    public long TotalAmount { get; set; }
    public int OperationCount { get; set; }

    public override string ToString() => Date.ToString("dd");
}
