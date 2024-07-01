using Products.Services.Abstractions;

namespace Products.Services;

public sealed class ServiceManager(Lazy<IServiceProducts> serviceProducts) : IServiceManager
{
    public IServiceProducts ServiceProducts => this.serviceProducts.Value;
    
    private readonly Lazy<IServiceProducts> serviceProducts = serviceProducts;
}