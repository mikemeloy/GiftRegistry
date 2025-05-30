﻿using System;
using System.Collections.Generic;

namespace i7MEDIA.Plugin.Widgets.Registry.Models;

public record RegistryList()
{
    public IEnumerable<RegistryListItem> RegistryItems { get; set; }
}

public record RegistryListItem
{
    public int Id { get; set; }
    public string Owner { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool CanModify { get; set; }
    public DateTime EventDate { get; set; }
};