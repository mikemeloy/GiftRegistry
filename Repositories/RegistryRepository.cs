﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using LinqToDB;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Data;

namespace i7MEDIA.Plugin.Widgets.Registry.Repositories;

public class RegistryRepository : IRegistryRepository
{
    private readonly IRepository<GiftRegistry> _registry;
    private readonly IRepository<GiftRegistryItem> _registryItem;
    private readonly IRepository<GiftRegistryType> _registryType;
    private readonly IRepository<Customer> _customer;
    private readonly IRepository<Product> _product;
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;

    public RegistryRepository(IRepository<GiftRegistryType> registryType, IStoreContext storeContext, IWorkContext workContext, IRepository<GiftRegistry> registry, IRepository<GiftRegistryItem> registryItem, IRepository<Customer> customer, IRepository<Product> product)
    {
        _product = product;
        _registry = registry;
        _customer = customer;
        _workContext = workContext;
        _registryItem = registryItem;
        _storeContext = storeContext;
        _registryType = registryType;
    }

    public async Task<IEnumerable<GiftRegistry>> GetCurrentCustomerRegistriesAsync()
    {
        var customer = await _workContext.GetCurrentCustomerAsync();

        return await _registry.GetAllAsync(query =>
                from r in query
                where r.CustomerId == customer.Id && !r.Deleted
                select r);
    }

    public async Task<RegistryDTO> GetRegistryByIdAsync(int registryId)
    {
        var registry = await _registry.GetByIdAsync(registryId);

        return registry.ToDTO();
    }

    public async Task InsertRegistryAsync(RegistryDTO registry)
    {
        var storeId = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var customer = await _workContext.GetCurrentCustomerAsync();
        var entity = registry.ToEntity();

        entity.StoreId = storeId;
        entity.CreatedDate = DateTime.UtcNow;
        entity.CustomerId = customer.Id;
        entity.Search = entity.GetQueryText(customer);

        await _registry.InsertAsync(entity);
    }

    public async Task UpdateRegistryAsync(RegistryDTO registryDTO)
    {
        var registry = await _registry.GetByIdAsync(registryDTO.Id);

        if (registry.IsNull())
        {
            return;
        }


        var customer = await _workContext.GetCurrentCustomerAsync();
        var entity = registryDTO.ToEntity();
        entity.Id = registry.Id;
        entity.CustomerId = registry.CustomerId;
        entity.Search = entity.GetQueryText(customer);

        await _registry.UpdateAsync(entity);
    }

    public async Task InsertRegistryItemAsync(int registryId, int productId, int quantity)
    {
        var item = new GiftRegistryItem()
        {
            RegistryId = registryId,
            ProductId = productId,
            Quantity = quantity,
            CreatedDate = DateTime.UtcNow
        };

        await _registryItem.InsertAsync(item);
    }

    public async Task UpdateRegistryItemAsync(GiftRegistryItem item)
    {
        await _registryItem.UpdateAsync(item);
    }

    public async Task DeleteRegistryAsync(int registryId)
    {
        var entity = await _registry.GetByIdAsync(registryId);
        entity.Deleted = true;
        await _registry.UpdateAsync(entity);
    }

    public async Task<GiftRegistryItem> GetRegistryItemByIdAsync(int registryItemId)
    {
        return await _registryItem.GetByIdAsync(registryItemId);
    }

    public async Task DeleteRegistryItemAsync(int registryItemId)
    {
        var entity = await _registryItem.GetByIdAsync(registryItemId);
        entity.Deleted = true;
        await _registryItem.UpdateAsync(entity);
    }

    public async Task<bool> GetRegistryOwnerAssociationAsync(int registryId)
    {
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();
        var customer = (from reg in _registry.Table
                        join cust in _customer.Table on reg.CustomerId equals cust.Id
                        where reg.Id == registryId
                        select cust).FirstOrDefault();

        if (customer.IsNull() || currentCustomer.IsNull())
        {
            return false;
        }

        return customer.IsEqual(currentCustomer);
    }

    public async Task<IList<RegistryListItem>> QueryAsync(string query)
    {
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();

        return (from reg in _registry.Table
                join cust in _customer.Table on reg.CustomerId equals cust.Id
                where reg.Deleted == false && reg.Search.Contains(query)
                select new RegistryListItem
                {
                    Id = reg.Id,
                    Owner = cust.FullName(),
                    Name = reg.Name,
                    Description = reg.Description,
                    EventDate = reg.EventDate,
                    CanModify = cust.IsEqual(currentCustomer),
                }).Take(20).ToList();
    }

    public async Task<List<RegistryItemViewModel>> GetRegistryItemsByIdAsync(int registryId)
    {
        var query = from reg in _registryItem.Table
                    join prod in _product.Table on reg.ProductId equals prod.Id
                    where reg.RegistryId == registryId && !reg.Deleted
                    select new RegistryItemViewModel
                    {
                        Id = reg.Id,
                        Name = prod.Name,
                        Description = prod.ShortDescription,
                        Price = prod.Price,
                        ProductId = prod.Id,
                        CartItemId = reg.CartItemId,
                        Purchased = reg.IsPurchased()
                    };


        return await query.ToListAsync();
    }

    public List<RegistryTypeDTO> GetRegistryTypes()
    {
        var query = from ty in _registryType.Table
                    where ty.Deleted == false
                    select new RegistryTypeDTO(ty.Id, ty.Name, ty.Description);


        return query.ToList();
    }
}