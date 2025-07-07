using System.Threading.Tasks;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IRegistryPdfService
{
    public Task<byte[]> GenerateGiftReceiptAsync(int id);
}