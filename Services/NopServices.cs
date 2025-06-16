using System;
using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using LinqToDB.Common;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Stores;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Media;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class NopServices : INopServices
{
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;
    private readonly IPictureService _pictureService;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly IGenericAttributeService _genericAttributeService;

    public NopServices(IStoreContext storeContext, IWorkContext workContext, IPictureService pictureService, IProductService productService, ICustomerService customerService, IGenericAttributeService genericAttributeService)
    {
        _pictureService = pictureService;
        _workContext = workContext;
        _productService = productService;
        _storeContext = storeContext;
        _productService = productService;
        _customerService = customerService;
        _genericAttributeService = genericAttributeService;
    }

    public async Task<string> GetProductImageUrlAsync(int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        var picture = await _pictureService.GetProductPictureAsync(product, "");

        if (picture.IsNull())
        {
            return "";
        }

        var imageUri = await _pictureService.GetPictureUrlAsync(picture.Id);

        return imageUri;
    }

    public async Task<Customer> GetCurrentCustomerAsync() => await _workContext.GetCurrentCustomerAsync();

    public async Task<Store> GetStoreAsync() => await _storeContext.GetCurrentStoreAsync();

    public async Task<Product> GetProductByIdAsync(int productId) => await _productService.GetProductByIdAsync(productId);

    public async Task<Customer> GetCustomerByIdAsync(int customerId) => await _customerService.GetCustomerByIdAsync(customerId);

    public async Task<int[]> GetRegistryItemAttributeAsync(Customer customer, int storeId = 0)
    {
        var giftRegistryAttribute = await _genericAttributeService.GetAttributeAsync(
                                                entity: customer,
                                                key: RegistryDefaults.GiftRegistryAttribute,
                                                storeId: storeId,
                                                defaultValue: ""
                                            );

        return giftRegistryAttribute.ToIntArray();
    }

    public async Task AddRegistryItemAttributeAsync(Customer customer, int registryItemId, int storeId = 0)
    {
        var registryItems = await GetRegistryItemAttributeAsync(customer);
        var added = registryItems.Append(registryItemId).ToArray();

        await _genericAttributeService.SaveAttributeAsync(
            entity: customer,
            key: RegistryDefaults.GiftRegistryAttribute,
            storeId: storeId,
            value: string.Join(',', added)
        );
    }

    public async Task RemoveRegistryItemAttributeAsync(Customer customer, int registryItemId, int storeId = 0)
    {
        var registryItems = await GetRegistryItemAttributeAsync(customer);
        var removed = registryItems.Except(registryItems).ToArray();

        await _genericAttributeService.SaveAttributeAsync(
            entity: customer,
            key: RegistryDefaults.GiftRegistryAttribute,
            storeId: storeId,
            value: string.Join(',', removed)
        );
    }

    public async Task ClearRegistryItemAttributeAsync(Customer customer, int storeId = 0)
    {
        await _genericAttributeService.SaveAttributeAsync(
            entity: customer,
            key: RegistryDefaults.GiftRegistryAttribute,
            storeId: storeId,
            value: ""
        );
    }
}