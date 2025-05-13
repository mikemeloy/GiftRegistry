using System.Collections.Generic;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;


public class RegistryItemViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int CartItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public bool Purchased { get; set; }
}

public class RegistryViewModel
{
    public RegistryViewModel() { }
    public bool IamOwner { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PluginVersion { get; set; }
    public string Owner { get; set; }
    public IEnumerable<RegistryItemViewModel> RegistryItems { get; set; }
}