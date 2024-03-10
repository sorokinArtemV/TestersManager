using System.Reflection;
using ServiceContracts.Enums;

namespace Services.Helpers;

public static class SorterHelper
{
    public static List<T> SortByProperty<T>(List<T> list, string propertyName, SortOrderOptions order)
    {
        PropertyInfo prop = typeof(T).GetProperty(propertyName);

        if (prop == null || !prop.CanRead || !typeof(IComparable).IsAssignableFrom(prop.PropertyType))
        {
            return list;
        }

        Comparer<T> comparer = Comparer<T>.Create((x, y) =>
        {
            object xValue = prop.GetValue(x);
            object yValue = prop.GetValue(y);

            int result = StringComparer.OrdinalIgnoreCase.Compare(xValue, yValue);

            if (order == SortOrderOptions.Desc) result = -result;


            return result;
        });

        list.Sort(comparer);

        return list;
    }
}