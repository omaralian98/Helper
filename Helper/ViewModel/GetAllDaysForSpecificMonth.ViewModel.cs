namespace Helper.ViewModel;

public record class GetAllDaysForSpecificMonth
{
    public int Day { get; set; }
    public long TotalCost { get; set; }
    public int OperationCount { get; set; }
}