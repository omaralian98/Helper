namespace Helper.ViewModel;

public record class GetAllDaysForSpecificMonth
{
    public int Day { get; set; }
    public long TotalAmount { get; set; }
    public int OperationCount { get; set; }
}