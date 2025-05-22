using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Stores;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface INopServices
{
    public Task<string> GetProductImageUrlAsync(int productId);
    public Task<Customer> GetCurrentCustomerAsync();
    public Task<Store> GetStoreAsync();
}