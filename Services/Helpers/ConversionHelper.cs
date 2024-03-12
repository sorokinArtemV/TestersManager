namespace Services.Helpers;

public static class ConversionHelper
{
    public static string ConvertMonthsToYears(int? totalMonths)
    {
        if (totalMonths is null) return "Just started";
        var years = totalMonths / 12;
        var months = totalMonths % 12;
        return $"{(years < 1 ? "" : years.ToString())}{(months < 1 ? "" : $"{(years >= 1 ? "." : " ")}" + months)}";
    }
}