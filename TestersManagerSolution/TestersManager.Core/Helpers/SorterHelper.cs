using TestersManager.Core.Enums;

namespace TestersManager.Core.Helpers;

public static class SorterHelper
{
    public static async Task<List<T>> SortByPropertyAsync<T>(List<T> list, string propertyName, SortOrderOptions order)
    {
        return await Task.Run(() =>
        {
            var prop = typeof(T).GetProperty(propertyName);

            if (prop == null || !prop.CanRead) return list;

            var comparer = Comparer<T>.Create((x, y) =>
            {
                var xValue = prop.GetValue(x);
                var yValue = prop.GetValue(y);

                // Check if the property type is a nullable int and convert to string for comparison
                if (prop.PropertyType == typeof(int?) || prop.PropertyType == typeof(int))
                {
                    var xInt = xValue as int?;
                    var yInt = yValue as int?;
                    var result = Nullable.Compare(xInt, yInt);

                    return order == SortOrderOptions.Desc ? -result : result;
                }
                else
                {
                    // Fallback to string comparison
                    var result = StringComparer.OrdinalIgnoreCase.Compare(xValue?.ToString(), yValue?.ToString());

                    return order == SortOrderOptions.Desc ? -result : result;
                }
            });

            list.Sort(comparer);

            return list;
        });
    }
}