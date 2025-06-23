using System;
using System.Collections.Generic;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;


public class RegistryItemViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CartItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
    public bool Purchased { get; set; }
}

public class RegistryViewModel
{
    public int Id { get; set; }
    public bool IamOwner { get; set; }
    public string Name { get; set; }
    public DateTime EventDate { get; set; }
    public int EventType { get; set; }
    public string Description { get; set; }
    public string Notes { get; set; }
    public string PluginVersion { get; set; }
    public string Owner { get; set; }
    public string Sponsor { get; set; }
    public int ShippingOption { get; set; }
    public string ConsultantName { get; set; }
    public string ConsultantEmail { get; set; }
    public IEnumerable<RegistryItemViewModel> RegistryItems { get; set; }
}