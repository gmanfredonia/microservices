using Admin.Services.Abstractions;

namespace Admin.Services;

public sealed class ServiceManager(Lazy<IServiceSecurity> serviceSecurity) : IServiceManager
{
    public IServiceSecurity ServiceSecurity => this.serviceSecurity.Value;    

    private readonly Lazy<IServiceSecurity> serviceSecurity = serviceSecurity;    
}