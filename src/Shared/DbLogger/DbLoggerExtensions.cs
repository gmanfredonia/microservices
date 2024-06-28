using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace DbLogger;

public static class DbLoggerExtensions
{
    public static ILoggingBuilder AddDbLogger(this ILoggingBuilder builder, Action<DbLoggerOptions> options)
    {
        builder.Services.Configure(options);
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DbLoggerProvider>());        

        return builder;
    }
    public static ILoggingBuilder AddDbLogger(this ILoggingBuilder builder, DbLoggerOptions options)
        => AddDbLogger(builder, opt => opt = options);
}