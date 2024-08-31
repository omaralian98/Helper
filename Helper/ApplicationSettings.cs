namespace Helper;

public static class ApplicationSettings
{
    public static bool ResetContentAfterClosing { get; set; } = false;

    public static int SuggestionsOperationTitleCount { get; set; } = 4;

    public static bool RedirectToHomeAfterSuccessfullyAddingANewOperation { get; set; } = true;

    public static long OperationDefaultValue { get; set; } = 1_000;

    public static long InventoryDefaultValue { get; set; } = 10_000;

    public static bool ExpandTheCurrentDateOnStart { get; set; } = true;
}
