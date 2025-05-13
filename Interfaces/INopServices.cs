using System.Threading.Tasks;
using Nop.Core.Domain.Customers;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface INopServices
{
    public Task<string> GetProductImageUrlAsync(int productId);
    public Task<Customer> GetCurrentCustomerAsync();
}