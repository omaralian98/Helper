namespace Helper;

public static class ExtensionMethods
{

    public static string ToCurrencyString(this object obj)
    {
        if (obj is null)
        {
            return string.Empty;
        }
        return $"{ApplicationSettings.Currency} {obj:N0}";
    }
}
