using System;
using System.Linq;
using Nop.Core.Domain.Customers;

namespace i7MEDIA.Plugin.Widgets.Registry.Extensions;

internal static class Extensions
{
    public static bool IsNull<T>(this T source) => source is null;
    public static bool NotNull<T>(this T source) => source is not null;
    public static string FullName(this Customer customer)
    {
        if (customer.IsNull())
        {
            return "";
        }

        return $"{customer.FirstName} {customer.LastName}";
    }
    public static bool IsEqual(this Customer customer, Customer comparer)
    {
        if (customer.IsNull() || comparer.IsNull())
        {
            return false;
        }

        return customer.Id == comparer.Id;
    }
    public static TResult GetValueOrDefault<TSource, TResult>(this TSource obj, Func<TSource, TResult> selector)
    {
        if (obj is null)
        {
            return default;
        }

        return selector(obj);
    }
    public static int[] ToIntArray(this string val)
    {
        var csv = val.Split(',');

        if (!csv.Any())
        {
            return Array.Empty<int>();
        }

        return Array.ConvertAll(val.Split(','), static s =>
        {
            _ = int.TryParse(s, out var n);
            return n;
        }).Distinct().ToArray();
    }
}