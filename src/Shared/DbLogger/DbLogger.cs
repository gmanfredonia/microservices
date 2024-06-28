using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Threading;
using System.Collections.Concurrent;
using System.Data;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DbLogger;

public sealed class DbLogger : ILogger
{
    public DbLogger(string categoryName, Func<DbLoggerOptions> getCurrentOptions, IConfiguration configuration)
    {
        this.categoryName = categoryName;
        this.getCurrentOptions = getCurrentOptions;
        this.configuration = configuration;

        if (!logInitialied)
        {
            logInitialied = true;
            _ = Task.Run(() => FlushAsync(getCurrentOptions, configuration));
        }
    }

    private readonly string categoryName;
    private readonly Func<DbLoggerOptions> getCurrentOptions;
    private readonly IConfiguration configuration;

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => default;

    public bool IsEnabled(LogLevel logLevel) =>
        logLevel != LogLevel.None;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        int threadId;
        JsonObject item;
        DbLoggerOptions options;

        if (!IsEnabled(logLevel)) return;

        options = getCurrentOptions();
        if (options.EventId == 0 || options.EventId == eventId.Id)
        {
            threadId = Environment.CurrentManagedThreadId;
            item = [];

            foreach (string field in options.LogFields)
                switch (field)
                {
                    case "LogLevel":
                        item["LogLevel"] = logLevel.ToString();
                        break;
                    case "ThreadId":
                        item["ThreadId"] = threadId;
                        break;
                    case "TraceId":                        
                        item["TraceId"] = exception?.Data["TraceId"]?.ToString() ?? string.Empty;
                        break;
                    case "EventId":
                        item["EventId"] = eventId.Id;
                        break;
                    case "EventName":
                        item["EventName"] = eventId.Name ?? string.Empty;
                        break;
                    case "CategoryName":
                        item["CategoryName"] = categoryName;
                        break;
                    case "Message":
                        item["Message"] = formatter is null ? string.Empty : formatter(state, exception);
                        break;
                    case "ExceptionMessage":
                        item["ExceptionMessage"] = exception?.Message ?? string.Empty;
                        break;
                    case "ExceptionStackTrace":
                        item["ExceptionStackTrace"] = exception?.StackTrace ?? string.Empty;
                        break;
                    case "ExceptionSource":
                        item["ExceptionSource"] = exception?.Source ?? string.Empty;
                        break;
                }

            itemsToLog.Add(item);
        }
    }

    private static async Task FlushAsync(Func<DbLoggerOptions> getCurrentOptions, IConfiguration configuration)
    {
        string connectionString;
        DbLoggerOptions options;

        foreach (JsonObject values in itemsToLog.GetConsumingEnumerable())
        {
            options = getCurrentOptions();
            connectionString = null;
            if (!string.IsNullOrEmpty(options.ConnectionString))
                connectionString = options.ConnectionString;
            else if (!string.IsNullOrEmpty(options.ConnectionStringName))
                connectionString = configuration.GetSection($"ConnectionStrings:{options.ConnectionStringName}").Value;

            if (string.IsNullOrEmpty(connectionString))
                break;

            await using SqlConnection connection = new(connectionString);
            await connection.OpenAsync().ConfigureAwait(false);

            await using SqlCommand command = new();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = $"INSERT INTO {options.LogTable} ([logValues], [logCreated]) VALUES (@Values, @Created)";

            command.Parameters.Add(new SqlParameter("@Values", JsonSerializer.Serialize(values)));
            command.Parameters.Add(new SqlParameter("@Created", DateTimeOffset.Now));

            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }

    private static bool logInitialied;
    private static readonly BlockingCollection<JsonObject> itemsToLog = [];
}