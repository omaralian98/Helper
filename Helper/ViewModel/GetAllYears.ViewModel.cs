﻿namespace Helper.ViewModel;

public record class GetAllYears
{
    public int Year { get; set; }
    public long TotalAmount { get; set; }
    public int OperationCount { get; set; }
}

