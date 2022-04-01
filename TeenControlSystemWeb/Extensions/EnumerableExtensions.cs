using System.Text;

namespace TeenControlSystemWeb.Extensions;

public static class EnumerableExtensions
{
    public static string ConvertToCommasString<T>(this IEnumerable<T> enumerable)
    {
        var stringBuilder = new StringBuilder();

        foreach (var a in enumerable)
        {
            stringBuilder.Append(a.ToString() + ",");
        }

        var str = stringBuilder.ToString();
        
        return str.Substring(0, str.Length - 1);
    }
}