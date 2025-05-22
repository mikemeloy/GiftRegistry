using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Stores;
using Nop.Data;
using Nop.Services.Media;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class NopServices : INopServices
{
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;
    private readonly IPictureService _pictureService;
    private readonly IRepository<Product> _product;
    public NopServices(IStoreContext storeContext, IWorkContext workContext, IPictureService pictureService, IRepository<Product> product)
    {
        _pictureService = pictureService;
        _workContext = workContext;
        _storeContext = storeContext;
        _product = product;
    }

    public async Task<string> GetProductImageUrlAsync(int productId)
    {
        var product = await _product.GetByIdAsync(productId);

        var picture = await _pictureService.GetProductPictureAsync(product, "");
        var imageUri = await _pictureService.GetPictureUrlAsync(picture.Id);

        return imageUri;
    }

    public async Task<Customer> GetCurrentCustomerAsync()
    {
        var currentCustomer = await _workContext.GetCurrentCustomerAsync();

        return currentCustomer;
    }

    public async Task<Store> GetStoreAsync()
    {
        var store = await _storeContext.GetCurrentStoreAsync();

        return store;
    }
}