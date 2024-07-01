using Admin.Services.Abstractions;

namespace Admin.Services;

public sealed class ServiceManager(Lazy<IServiceSecurity> serviceSecurity,
                                   Lazy<IServiceProducts> serviceProducts) : IServiceManager
{
    public IServiceSecurity ServiceSecurity => this.serviceSecurity.Value;
    public IServiceProducts ServiceProducts => this.serviceProducts.Value;    

    private readonly Lazy<IServiceSecurity> serviceSecurity = serviceSecurity;
    private readonly Lazy<IServiceProducts> serviceProducts = serviceProducts;
}