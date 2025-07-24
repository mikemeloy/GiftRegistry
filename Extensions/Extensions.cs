using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using i7MEDIA.Plugin.Widgets.Registry.Models;
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
        if (obj.IsNull())
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
    public static bool IsMinDate(this DateTime source)
    {
        return source == DateTime.MinValue;
    }
    public static string ToCurrency(this decimal source)
    {
        return source.ToString("C", CultureInfo.CurrentCulture);
    }
    public static string ToProductAttributeXml(this IEnumerable<RegistryProductAttribute> source)
    {
        if (source.IsNull())
        {
            return null;
        }

        XDocument xmlPerson = new(
            new XElement("Attributes",
                source.Select(attr =>
                    new XElement("ProductAttribute",
                        new XAttribute("ID", attr.AttributeId),
                        new XElement("ProductAttributeValue",
                            new XElement("Value", attr.AttributeValue)
                        )
                    )
                )
            )
        );

        return xmlPerson.ToString();
    }
}