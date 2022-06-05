namespace Core.Extensions;

public static class EnumerableExtensions
{
    public static void Consume<T>(this IEnumerable<T> enumerable, Action<T> perItem)
    {
        foreach (var item in enumerable)
        {
            perItem(item);
        }
    }
}