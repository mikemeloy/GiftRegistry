using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Nop.Core.Domain.Catalog;
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
    public static string RegistryOwners(this Customer customer, string sponsor)
    {
        if (customer.IsNull())
        {
            return "";
        }

        if (string.IsNullOrEmpty(sponsor))
        {
            return customer.FullName();
        }

        return $"{customer.FullName()} & {sponsor}";
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
    public static AttributeControlType ControlType(this RegistryProductAttribute source) => (AttributeControlType)source.ControlTypeId;
    public static string ToProductAttributeXml(this IEnumerable<RegistryProductAttribute> source)
    {
        if (source.IsNull())
        {
            return null;
        }

        XDocument xmlPerson = new(
            new XElement("Attributes",
                source
                .Where(attr => !string.IsNullOrEmpty(attr.AttributeValue))
                .Select(attr =>
                {
                    if (attr.ControlType() == AttributeControlType.Datepicker)
                    {
                        var isDate = DateTime.TryParse(attr.AttributeValue, out var date);

                        return new XElement("ProductAttribute",
                            new XAttribute("ID", attr.AttributeId),
                                new XElement("ProductAttributeValue",
                                    new XElement("Value", isDate ? date.ToLongDateString() : attr.AttributeValue)
                                )
                        );
                    }

                    return new XElement("ProductAttribute",
                           new XAttribute("ID", attr.AttributeId),
                           new XElement("ProductAttributeValue",
                               new XElement("Value", attr.AttributeValue)
                           )
                       );
                })
            )
        );

        return xmlPerson.ToString();
    }
    public static Guid ToSafeGuid(this RegistrySettingsModel source)
    {
        if (source.IsNull())
        {
            return new();
        }

        _ = Guid.TryParse(source.ProductKey, out var productKey);

        return productKey;
    }
}