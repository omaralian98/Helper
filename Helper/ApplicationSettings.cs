namespace Helper;

public static class ApplicationSettings
{
    public static bool ResetContentAfterClosing { get; set; } = false;

    public static int SuggestionsOperationTitleCount { get; set; } = 4;

    public static bool RedirectToHomeAfterSuccessfullyAddingANewOperation { get; set; } = true;

    public static long OperationDefaultValue { get; set; } = 1_000;

    public static bool ExpandTheCurrentDateOnStart { get; set; } = false;

    public static int NumberOfYearToShow { get; set; } = 5;

    public static string Currency { get; set; } = "ل.س";
}
