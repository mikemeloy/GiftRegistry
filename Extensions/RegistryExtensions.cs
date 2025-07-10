using System.Collections.Generic;
using System.Linq;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Models;
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
            EventTypeId = source.EventType,
            Sponsor = source.Sponsor,
            ShippingOptionId = source.ShippingOption,
            ConsultantId = source.ConsultantId,
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
            EventType: source.EventTypeId,
            Sponsor: source.Sponsor,
            ShippingOption: source.ShippingOptionId,
            ConsultantId: source.ConsultantId
        );
    }

    public static string GetQueryText(this GiftRegistry source, Customer customer)
    {
        if (source.IsNull() || customer.IsNull())
        {
            return "";
        }

        return $"{source.Name} {source.EventDate.ToLongDateString()} {customer.FirstName} {customer.LastName} {source.Sponsor}";
    }

    public static List<RegistryItemViewModel> SortRegistryItems(this List<RegistryItemViewModel> source)
    {
        return source
            .OrderBy(ri => ri.Fulfilled)
            .ThenBy(ri => ri.Name)
            .ToList();
    }

    public static string GetRegistryOrderEmailBody(this RegistryData source) => $"Hello {source.Consultant?.Name},<br/><br/> You are listed as the consultant for <i>“{source.RegistryName}”</i>, <i>{source.ProductName}</i> has been purchased on order #{source.OrderId}.";

    public static string GetRegistryOrderEmailSubject(this RegistryData source) => $"An item on “{source.RegistryName}” has been purchased!";

    public static string GetRegistryAdminNote(this RegistryData source) => $"The item ({source.ProductName}) was purchased on behalf of the registry ({source.RegistryName})";
}