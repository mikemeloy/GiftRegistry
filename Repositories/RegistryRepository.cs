using System;
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
using Nop.Core.Domain.Customers;
using Nop.Data;

namespace i7MEDIA.Plugin.Widgets.Registry.Repositories;

public class RegistryRepository : IRegistryRepository
{
    private readonly IRepository<GiftRegistry> _registry;
    private readonly IRepository<GiftRegistryItem> _registryItem;
    private readonly IRepository<Customer> _customer;
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;

    public RegistryRepository(IStoreContext storeContext, IWorkContext workContext, IRepository<GiftRegistry> registry, IRepository<GiftRegistryItem> registryItem, IRepository<Customer> customer)
    {
        _registry = registry;
        _customer = customer;
        _workContext = workContext;
        _registryItem = registryItem;
        _storeContext = storeContext;
    }

    public async Task<IEnumerable<GiftRegistry>> GetCurrentCustomerRegistriesAsync()
    {
        var customer = await _workContext.GetCurrentCustomerAsync();

        return await _registry.GetAllAsync(query =>
                from r in query
                where r.CustomerId == customer.Id && !r.Deleted
                select r
          , cache => default);
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

        await _registry.InsertAsync(entity);
    }

    public async Task InsertRegistryItemAsync(int registryId, int productId)
    {
        var item = new GiftRegistryItem()
        {
            RegistryId = registryId,
            ProductId = productId,
            CreatedDate = DateTime.UtcNow
        };

        await _registryItem.InsertAsync(item);
    }

    public async Task DeleteRegistryAsync(int registryId)
    {
        var entity = await _registry.GetByIdAsync(registryId);
        entity.Deleted = true;
        await _registry.UpdateAsync(entity);
    }

    public Customer GetRegistryOwner(int registryId)
    {
        var customer = (from reg in _registry.Table
                        join cust in _customer.Table on reg.CustomerId equals cust.Id
                        where reg.Id == registryId
                        select cust).FirstOrDefault();

        if (customer.IsNull())
        {
            return null;
        }

        return customer;
    }

    public IList<RegistryListItem> Query(string query)
    {
        return (from reg in _registry.Table
                join cust in _customer.Table on reg.CustomerId equals cust.Id
                where reg.Name.Contains(query)
                select new RegistryListItem
                {
                    Owner = cust.FullName(),
                    Name = reg.Name,
                    Description = reg.Description,
                    EventDate = reg.EventDate,
                }).ToList();
    }

    public async Task<IList<RegistryItemDTO>> GetRegistryItemsByIdAsync(int registryId)
    {
        var items = await _registryItem.GetAllAsync(x => x.Where(i => i.RegistryId == registryId));

        return items
            .Select(ri => new RegistryItemDTO(ri.Id, ri.ProductId, ri.CartItemId, ri.OrderId))
            .ToList();
    }
}