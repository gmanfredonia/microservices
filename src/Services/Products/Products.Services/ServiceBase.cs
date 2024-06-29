using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Services;

public abstract class ServiceBase(IConfiguration configuration, ILogger<ServiceBase> logger, IMapper mapper, IStringLocalizer localizer)
{
    //protected readonly IPrincipal userPrincipal;
    protected readonly IStringLocalizer localizer = localizer;
    protected readonly IConfiguration configuration = configuration;
    protected readonly ILogger<ServiceBase> logger = logger;
    protected readonly IMapper mapper = mapper;
    //protected readonly IServiceCache serviceCache;    
}
