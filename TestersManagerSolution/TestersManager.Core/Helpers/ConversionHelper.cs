namespace TestersManager.Core.Helpers;

public static class ConversionHelper
{
    public static string ConvertMonthsToYears(int? totalMonths)
    {
        if (totalMonths is null) return "Just started";
        var years = totalMonths / 12;
        var months = totalMonths % 12;
        return years switch
        {
            > 0 when months > 0 => $"{years}y {months}m",
            > 0 => $"{years}y",
            _ => $"{months}m"
        };
    }
}