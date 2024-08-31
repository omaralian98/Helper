namespace Helper.ViewModel;

public record class GetAllMonthsForSpecificYear
{
    public int Month { get; set; }
    public long TotalCost { get; set; }
    public int OperationCount { get; set; }
}