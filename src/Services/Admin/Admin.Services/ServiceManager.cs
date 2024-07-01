using Admin.Services.Abstractions;

namespace Admin.Services;

public sealed class ServiceManager(Lazy<IServiceUsers> serviceSecurity) : IServiceManager
{
    public IServiceUsers ServiceUsers => this.serviceSecurity.Value;    

    private readonly Lazy<IServiceUsers> serviceSecurity = serviceSecurity;    
}