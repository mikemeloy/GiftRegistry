using System.Collections.Generic;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using Nop.Core.Domain.Customers;

namespace i7MEDIA.Plugin.Widgets.Registry.Extensions;

public static class RegistryExtensions
{
    public static GiftRegistry ToEntity(this RegistryDTO source)
    {
        return new GiftRegistry
        {
            Name = source.Name,
            Description = source.Description,
            EventDate = source.EventDate,
            Notes = source.Notes,
            EventType = source.EventType
        };
    }

    public static RegistryDTO ToDTO(this GiftRegistry source)
    {
        if (source.IsNull())
        {
            return null;
        }

        return new(
            Id: source.Id,
            Name: source.Name,
            Description: source.Description,
            EventDate: source.EventDate,
            Notes: source.Notes,
            EventType: source.EventType
        );
    }

    public static RegistryDTO AddRegistryItems(this RegistryDTO source, IList<RegistryItemDTO> registryItems)
    {
        if (source.IsNull())
        {
            return null;
        }

        source.RegistryItems = registryItems;

        return source;
    }

    public static RegistryDTO AddCustomerInfo(this RegistryDTO source, Customer customer)
    {
        if (source.IsNull())
        {
            return null;
        }

        if (customer.IsNull())
        {
            return source;
        }

        source.Owner = $"{customer.FirstName} {customer.LastName}";

        return source;
    }

    public static RegistryItemDTO ToDTO(this GiftRegistryItem source)
    {
        if (source.IsNull())
        {
            return null;
        }

        return new RegistryItemDTO(
            Id: source.Id,
            ProductId: source.ProductId,
            CartItemId: source.CartItemId
        );
    }

    public static string GetQueryText(this GiftRegistry source, Customer customer)
    {
        if (source.IsNull() || customer.IsNull())
        {
            return "";
        }

        return $"{source.Name} {source.Description} {source.EventDate.ToLongDateString()} {customer.FirstName} {customer.LastName}";
    }
}