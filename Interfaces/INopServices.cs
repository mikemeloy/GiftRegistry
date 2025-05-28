using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Stores;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface INopServices
{
    public Task<string> GetProductImageUrlAsync(int productId);
    public Task<Customer> GetCurrentCustomerAsync();
    public Task<Store> GetStoreAsync();
    public Task<Product> GetProductByIdAsync(int productId);
    public Task<Customer> GetCustomerByIdAsync(int customerId);
    public Task<int[]> GetRegistryItemAttributeAsync(Customer customer, int storeId = 0);
    public Task AddRegistryItemAttributeAsync(Customer customer, int registryItemId, int storeId = 0);
    public Task RemoveRegistryItemAttributeAsync(Customer customer, int registryItemId, int storeId = 0);
    public Task ClearRegistryItemAttributeAsync(Customer customer, int storeId = 0);
}