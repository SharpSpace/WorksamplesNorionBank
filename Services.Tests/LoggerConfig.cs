using Microsoft.Extensions.Logging;

namespace Services.Tests;

public static class LoggerConfig
{
    public static ILogger<T> CreateLogger<T>() =>
        LoggerFactory
            .Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddConsole();
            })
            .CreateLogger<T>();
}