using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using i7MEDIA.Plugin.Widgets.Registry.Data;
using i7MEDIA.Plugin.Widgets.Registry.Extensions;
using i7MEDIA.Plugin.Widgets.Registry.Interfaces;
using i7MEDIA.Plugin.Widgets.Registry.Models;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Stores;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;

namespace i7MEDIA.Plugin.Widgets.Registry.Services;

public class NopServices : INopServices
{
    private readonly ILogger_R _logger_R;
    private readonly IStoreContext _storeContext;
    private readonly IWorkContext _workContext;
    private readonly IPictureService _pictureService;
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly IGenericAttributeService _genericAttributeService;
    private readonly IEmailSender _emailSender;
    private readonly IOrderService _orderService;
    private readonly IEmailAccountService _emailAccountService;
    private readonly IProductAttributeService _productAttributeService;
    private readonly EmailAccountSettings _emailAccountSettings;

    public NopServices(EmailAccountSettings emailAccountSettings, IStoreContext storeContext, IWorkContext workContext, IPictureService pictureService, IProductService productService, ICustomerService customerService, IGenericAttributeService genericAttributeService, IEmailSender emailSender, IOrderService orderService, IEmailAccountService emailAccountService, ILogger_R logger_R, IProductAttributeService productAttributeService)
    {
        _emailSender = emailSender;
        _logger_R = logger_R;
        _workContext = workContext;
        _storeContext = storeContext;
        _orderService = orderService;
        _productService = productService;
        _pictureService = pictureService;
        _emailAccountService = emailAccountService;
        _productService = productService;
        _customerService = customerService;
        _genericAttributeService = genericAttributeService;
        _emailAccountSettings = emailAccountSettings;
        _productAttributeService = productAttributeService;
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

    public async Task InsertOrderNoteAsync(int orderId, string note)
    {
        try
        {
            await _orderService.InsertOrderNoteAsync(new() { Note = note, OrderId = orderId, CreatedOnUtc = DateTime.UtcNow });
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(InsertOrderNoteAsync), e);
        }
    }

    public async Task SendRegistryConsultantEmailAsync(string subject, string body, GiftRegistryConsultant consultant)
    {
        if (consultant.IsNull())
        {
            return;
        }

        try
        {
            var emailAccount = await _emailAccountService.GetEmailAccountByIdAsync(_emailAccountSettings.DefaultEmailAccountId);

            await _emailSender.SendEmailAsync(
                        emailAccount,
                        subject,
                        body,
                        fromAddress: emailAccount.Email,
                        fromName: emailAccount.DisplayName,
                        toAddress: consultant.Email,
                        toName: consultant.Name
                    );
        }
        catch (Exception e)
        {
            await _logger_R.LogErrorAsync(nameof(SendRegistryConsultantEmailAsync), e);
        }
    }

    public async Task<IEnumerable<RegistryProductAttribute>> GetProductAttributesByProductIdAsync(int productId)
    {
        var attr = await _productAttributeService.GetProductAttributeMappingsByProductIdAsync(productId);

        return attr
            .Select(attr => new RegistryProductAttribute(
                AttributeId: attr.Id,
                AttributeValue: attr.DefaultValue,
                IsRequired: attr.IsRequired,
                ControlTypeId: attr.AttributeControlTypeId
            ));
    }
}