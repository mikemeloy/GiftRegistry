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
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Data;

namespace i7MEDIA.Plugin.Widgets.Registry.Repositories;

public class RegistryRepository : IRegistryRepository
{
    private readonly IRepository<GiftRegistry> _registry;
    private readonly IRepository<GiftRegistryItem> _registryItem;
    private readonly IRepository<GiftRegistryType> _registryType;
    private readonly IRepository<GiftRegistryItemOrder> _registryItemOrder;
    private readonly IRepository<GiftRegistryConsultant> _registryConsultant;
    private readonly IRepository<GiftRegistryShippingOption> _registryShippingOption;
    private readonly IRepository<OrderItem> _orderItem;
    private readonly IRepository<Order> _order;
    private readonly IRepository<Customer> _customer;
    private readonly IRepository<Product> _product;
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;

    public RegistryRepository(IRepository<OrderItem> orderItem, IRepository<GiftRegistryType> registryType, IStoreContext storeContext, IWorkContext workContext, IRepository<GiftRegistry> registry, IRepository<GiftRegistryItem> registryItem, IRepository<Customer> customer, IRepository<Product> product, IRepository<GiftRegistryItemOrder> registryItemOrder, IRepository<GiftRegistryConsultant> registryConsultant, IRepository<GiftRegistryShippingOption> registryShippingType, IRepository<Order> order)
    {
        _product = product;
        _registry = registry;
        _customer = customer;
        _orderItem = orderItem;
        _order = order;
        _workContext = workContext;
        _registryItem = registryItem;
        _storeContext = storeContext;
        _registryType = registryType;
        _registryItemOrder = registryItemOrder;
        _registryConsultant = registryConsultant;
        _registryShippingOption = registryShippingType;
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
        entity.CreatedDate = registry.CreatedDate;
        entity.ConsultantId = registry.ConsultantId;
        entity.AdminNotes = registry.AdminNotes;

        await _registry.UpdateAsync(entity);
    }

    public async Task<(int? OldConsultant, int? NewConsultant)> UpdateRegistryAsync(RegistryEditAdminModel source)
    {
        var registry = await _registry.GetByIdAsync(source.Id);

        if (registry.IsNull())
        {
            return (0, 0);
        }

        var oldConsultant = registry.ConsultantId;

        registry.Name = source.Name;
        registry.Description = source.Summary;
        registry.ConsultantId = source.ConsultantId;
        registry.AdminNotes = source.AdminNotes;
        registry.ShippingOptionId = source.DeliveryMethodId;
        registry.EventTypeId = source.EventTypeId;
        registry.EventDate = source.EventDate;
        registry.Notes = source.ClientNotes;
        registry.Sponsor = source.Sponsor;

        await _registry.UpdateAsync(registry);

        return (oldConsultant, source.ConsultantId);
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

    public async Task UpdateRegistryItemAsync(RegistryItemDTO item)
    {
        var registryItem = await _registryItem.GetByIdAsync(item.Id);

        registryItem.Quantity = item.Quantity;

        await _registryItem.UpdateAsync(registryItem);
    }

    public async Task DeleteRegistryAsync(int registryId)
    {
        var entity = await _registry.GetByIdAsync(registryId);
        entity.Deleted = !entity.Deleted;
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

    public async Task<(bool IamOwner, string OwnerName)> GetRegistryOwnerAssociationAsync(int registryId)
    {
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();
        var customer = (from reg in _registry.Table
                        join cust in _customer.Table on reg.CustomerId equals cust.Id
                        where reg.Id == registryId
                        select cust).FirstOrDefault();

        if (customer.IsNull() || currentCustomer.IsNull())
        {
            return (IamOwner: false, OwnerName: "");
        }

        return (IamOwner: customer.IsEqual(currentCustomer), OwnerName: customer.FullName());
    }

    public async Task<IList<RegistryViewModel>> AdminQueryAsync(string q)
    {
        var query = from reg in _registry.Table
                    join cust in _customer.Table on reg.CustomerId equals cust.Id
                    join con in _registryConsultant.Table on reg.ConsultantId equals con.Id into consultantGroup
                    from consultant in consultantGroup.DefaultIfEmpty()
                    join del in _registryShippingOption.Table on reg.ShippingOptionId equals del.Id into shippingOptionGroup
                    from shippingOption in shippingOptionGroup.DefaultIfEmpty()
                    where reg.Search.Contains(q)
                    select new RegistryViewModel
                    {
                        Id = reg.Id,
                        Name = reg.Name,
                        EventDate = reg.EventDate,
                        ConsultantName = consultant.GetValueOrDefault(c => c.Name),
                        ConsultantEmail = consultant.GetValueOrDefault(c => c.Email),
                        ShippingMethod = shippingOption.GetValueOrDefault(s => s.Name),
                        Notes = reg.Notes,
                        AdminNotes = reg.AdminNotes,
                        Deleted = reg.Deleted
                    };

        return await query.ToListAsync();
    }

    public async Task<IList<RegistryListItem>> QueryAsync(string query)
    {
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();

        return (from reg in _registry.Table
                join cust in _customer.Table on reg.CustomerId equals cust.Id
                where reg.Deleted == false && reg.Search.Contains(query)
                orderby reg.Name ascending
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

    public async Task<IList<RegistryListItem>> QueryAsync(string query, DateTime start, DateTime end)
    {
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();

        if (start.IsMinDate())
        {
            start = DateTime.MinValue;
        }

        if (end.IsMinDate())
        {
            end = DateTime.MaxValue;
        }

        return (from reg in _registry.Table
                join cust in _customer.Table on reg.CustomerId equals cust.Id
                where (string.IsNullOrWhiteSpace(query) || reg.Search.Contains(query))
                && (reg.EventDate >= start && reg.EventDate <= end)
                orderby reg.Name ascending
                select new RegistryListItem
                {
                    Id = reg.Id,
                    Owner = cust.FullName(),
                    Name = reg.Name,
                    Description = reg.Description,
                    EventDate = reg.EventDate,
                    CanModify = cust.IsEqual(currentCustomer),
                }).ToList();
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
                        Quantity = reg.Quantity,
                        ProductId = prod.Id,
                        CartItemId = reg.CartItemId,
                        Purchased = (from order in _registryItemOrder.Table
                                     where order.RegistryItemId == reg.Id
                                     select order.Quantity
                                    ).Sum()
                    };

        return await query.ToListAsync();
    }

    public async Task<List<RegistryOrderViewModel>> GetRegistryOrdersByIdAsync(int registryId)
    {
        var query = from reg in _registryItem.Table
                    join itemOrder in _registryItemOrder.Table on reg.Id equals itemOrder.RegistryItemId
                    join order in _order.Table on itemOrder.OrderId equals order.Id
                    join cust in _customer.Table on order.CustomerId equals cust.Id
                    where reg.RegistryId == registryId && !reg.Deleted
                    select new RegistryOrderViewModel()
                    {
                        RegistryId = reg.Id,
                        OrderId = order.Id,
                        OrderTotal = order.OrderTotal,
                        FullName = cust.FullName(),
                        OrderDate = order.CreatedOnUtc
                    };

        return await query.ToListAsync();
    }

    public async Task InsertRegistryItemOrderAsync(int orderId, int registryId, int productId, int quantity)
    {
        await _registryItemOrder.InsertAsync(new GiftRegistryItemOrder()
        {
            OrderId = orderId,
            RegistryItemId = productId,
            Quantity = quantity
        });
    }

    public async Task UpdateConsultantAsync(int? id, string name, string email, bool deleted = false)
    {
        var consultant = await _registryConsultant.GetByIdAsync(id);

        if (consultant.IsNull())
        {
            return;
        }

        consultant.Name = name;
        consultant.Email = email;
        consultant.Deleted = deleted;

        await _registryConsultant.UpdateAsync(consultant);
    }

    public async Task InsertConsultantAsync(string name, string email)
    {
        await _registryConsultant.InsertAsync(new GiftRegistryConsultant
        {
            Name = name,
            Email = email
        });
    }

    public async Task InsertRegistryTypeAsync(string name, string description)
    {
        await _registryType.InsertAsync(new GiftRegistryType
        {
            Name = name,
            Description = description
        });
    }

    public async Task UpdateRegistryTypeAsync(int? id, string name, string description, bool deleted = false)
    {
        var registryType = await _registryType.GetByIdAsync(id);

        if (registryType.IsNull())
        {
            return;
        }

        registryType.Name = name;
        registryType.Description = description;
        registryType.Deleted = deleted;

        await _registryType.UpdateAsync(registryType);
    }

    public async Task InsertShippingOptionAsync(string name, string description)
    {
        await _registryShippingOption.InsertAsync(new GiftRegistryShippingOption
        {
            Name = name,
            Description = description
        });
    }

    public async Task UpdateShippingOptionAsync(int? id, string name, string description, bool deleted)
    {
        var shippingOption = await _registryShippingOption.GetByIdAsync(id);

        if (shippingOption.IsNull())
        {
            return;
        }

        shippingOption.Name = name;
        shippingOption.Description = description;
        shippingOption.Deleted = deleted;

        await _registryShippingOption.UpdateAsync(shippingOption);
    }

    public async Task<IEnumerable<RegistryTypeDTO>> GetRegistryTypesAsync()
    {
        var query = from ty in _registryType.Table
                    where ty.Deleted == false
                    orderby ty.SortOrder descending, ty.Name
                    select new RegistryTypeDTO(ty.Id, ty.Name, ty.Description, ty.Deleted);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<RegistryConsultantDTO>> GetRegistryConsultantsAsync()
    {
        var query = from c in _registryConsultant.Table
                    where c.Deleted == false
                    orderby c.SortOrder descending, c.Name

                    select new RegistryConsultantDTO(c.Id, c.Name, c.Email, c.Deleted);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<RegistryShippingOptionDTO>> GetRegistryShippingOptionsAsync()
    {
        var query = from c in _registryShippingOption.Table
                    where c.Deleted == false
                    orderby c.SortOrder descending, c.Name
                    select new RegistryShippingOptionDTO(c.Id, c.Name, c.Description, c.Deleted);

        return await query.ToListAsync();
    }

    public async Task<GiftRegistryConsultant> GetConsultantByIdAsync(int? consultantId)
    {
        return await _registryConsultant.GetByIdAsync(consultantId);
    }

    public async Task<RegistryEditAdminModel> GetAdminFieldsAsync(int id)
    {
        var registry = await _registry.GetByIdAsync(id);
        var items = await GetRegistryItemsByIdAsync(id);
        var orders = await GetRegistryOrdersByIdAsync(id);

        return new RegistryEditAdminModel(
                Id: id,
                DeliveryMethodId: registry.ShippingOptionId,
                EventTypeId: registry.EventTypeId,
                ConsultantId: registry.ConsultantId,
                AdminNotes: registry.AdminNotes,
                ClientNotes: registry.Notes,
                Name: registry.Name,
                Summary: registry.Description,
                EventDate: registry.EventDate,
                Sponsor: registry.Sponsor,
                RegistryItems: items,
                RegistryOrders: orders
            );
    }

    public async Task<List<GiftReceiptOrderItem>> GetGiftReceiptOrderItemsAsync(int orderId)
    {
        var query = from oi in _orderItem.Table
                    join p in _product.Table on oi.ProductId equals p.Id
                    where oi.OrderId == orderId
                    select new GiftReceiptOrderItem(oi.Quantity, p.Name, p.Sku);

        return await query.ToListAsync();
    }
}