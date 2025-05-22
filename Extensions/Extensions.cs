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
}