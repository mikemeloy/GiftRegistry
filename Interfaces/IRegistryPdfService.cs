using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.DTOs;

namespace i7MEDIA.Plugin.Widgets.Registry.Interfaces;

public interface IRegistryPdfService
{
    public Task<byte[]> GenerateGiftReceiptAsync(int id);
    public Task<byte[]> GenerateRegistryItemReport(int registryId);
    public Task<byte[]> GenerateRegistryOrderReport(int registryId);
    public Task<byte[]> GenerateRegistryReportAsync(ReportRequestDTO request);
}