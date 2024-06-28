using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Runtime.Versioning;

namespace DbLogger;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("Database")]
public sealed class DbLoggerProvider : ILoggerProvider
{
    public DbLoggerProvider(IOptionsMonitor<DbLoggerOptions> optionsMonitor, IConfiguration configuration)
    {
        this.configuration = configuration;
        this.currentOptions = optionsMonitor.CurrentValue;    
        this.onChangeOptions = optionsMonitor.OnChange(options => currentOptions = options);
        this.loggers = new(StringComparer.OrdinalIgnoreCase);
    }

    public ILogger CreateLogger(string categoryName) =>
        loggers.GetOrAdd(categoryName, name => new DbLogger(name, GetCurrentOptions, configuration));

    public void Dispose()
    {
        loggers.Clear();
        onChangeOptions.Dispose();
    }

    private DbLoggerOptions GetCurrentOptions() => currentOptions;

    private DbLoggerOptions currentOptions;
    private readonly IDisposable onChangeOptions;    
    private readonly IConfiguration configuration;
    private readonly ConcurrentDictionary<string, DbLogger> loggers;
}