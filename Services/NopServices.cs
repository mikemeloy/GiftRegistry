using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Stores;
using Nop.Services.Catalog;
using Nop.Services.Media;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class NopServices : INopServices
{
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;
    private readonly IPictureService _pictureService;
    private readonly IProductService _productService;
    public NopServices(IStoreContext storeContext, IWorkContext workContext, IPictureService pictureService, IProductService productServide)
    {
        _pictureService = pictureService;
        _workContext = workContext;
        _productService = productServide;
        _storeContext = storeContext;
        _productService = productServide;
    }

    public async Task<string> GetProductImageUrlAsync(int productId)
    {
        var product = await _productService.GetProductByIdAsync(productId);
        var picture = await _pictureService.GetProductPictureAsync(product, "");
        var imageUri = await _pictureService.GetPictureUrlAsync(picture.Id);

        return imageUri;
    }

    public async Task<Customer> GetCurrentCustomerAsync() => await _workContext.GetCurrentCustomerAsync();

    public async Task<Store> GetStoreAsync() => await _storeContext.GetCurrentStoreAsync();

    public Task<Product> GetProductByIdAsync(int productId) => _productService.GetProductByIdAsync(productId);
}