using ServiceContracts.DTO;

namespace Services.Helpers;

public static class FilterHelper
{
    public static List<TesterResponse> FilterBy(List<TesterResponse> testers, Func<TesterResponse, string?> selector,
        string searchString)
    {
        return testers.Where(x =>
                string.IsNullOrEmpty(selector(x)) ||
                selector(x).Contains(searchString, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}