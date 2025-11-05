using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using static Serilog.Sinks.MSSqlServer.ColumnOptions;


namespace $safeprojectname$.Common.Logging
{
    public static class SerilogConfig
    {
        /// <summary>
        /// Configura o Serilog a partir do builder do ASP.NET Core (.NET 9).
        /// Chame no Program.cs antes de builder.Build().
        /// </summary>
        public static void Configure(WebApplicationBuilder builder)
        {
            var cfg = builder.Configuration;
            var env = builder.Environment;

            // BasePath: em Core, use ContentRoot (equivalente a AppDomain.BaseDirectory do app)
            var root = env.ContentRootPath ?? AppContext.BaseDirectory;
            var logsPath = Path.Combine(root, "App_Data", "logs");
            Directory.CreateDirectory(logsPath);

            // Nível mínimo
            var levelText = cfg["Serilog:MinimumLevel"] ?? "Information";
            if (!Enum.TryParse(levelText, true, out LogEventLevel minLevel))
                minLevel = LogEventLevel.Information;

            // Retenção de arquivos
            var retentionText = cfg["Serilog:RetentionDays"] ?? "14";
            _ = int.TryParse(retentionText, out var retentionDays);
            if (retentionDays <= 0) retentionDays = 14;

            // MSSQL
            var connectionString = cfg.GetConnectionString("DefaultConnection");
            var tableName = cfg["Serilog:SqlTable"] ?? "Logs";
            var autoCreate = (cfg["Serilog:SqlAutoCreate"] ?? "true")
                .Equals("true", StringComparison.OrdinalIgnoreCase);

            // === ColumnOptions (compatível v5/v6 do sink) ===
            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn { ColumnName = "App",         DataType = SqlDbType.NVarChar, DataLength = 64,  AllowNull = true },
                    new SqlColumn { ColumnName = "UserId",      DataType = SqlDbType.NVarChar, DataLength = 128, AllowNull = true },
                    new SqlColumn { ColumnName = "RequestId",   DataType = SqlDbType.NVarChar, DataLength = 64,  AllowNull = true },
                    new SqlColumn { ColumnName = "PaymentId",   DataType = SqlDbType.NVarChar, DataLength = 128, AllowNull = true },
                    new SqlColumn { ColumnName = "CustomerId",  DataType = SqlDbType.NVarChar, DataLength = 128, AllowNull = true },
                    new SqlColumn { ColumnName = "CorrelationId", DataType = SqlDbType.NVarChar, DataLength = 64, AllowNull = true }
                }
            };
            columnOptions.TimeStamp.NonClusteredIndex = true;
            columnOptions.Store.Add(StandardColumn.LogEvent); // guarda o JSON do evento

            // === SinkOptions ===
            var sinkOptions = new MSSqlServerSinkOptions
            {
                TableName = tableName,
                SchemaName = "dbo",
                AutoCreateSqlTable = autoCreate,
                BatchPostingLimit = 50
                // (não usar Period aqui)
            };

            // === LoggerConfiguration ===
            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Is(minLevel)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("App", "PayBridge")
                .WriteTo.File(
                    path: Path.Combine(logsPath, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: retentionDays,
                    shared: true)
                .WriteTo.Debug()
                .WriteTo.Console();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                loggerConfig = loggerConfig.WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: sinkOptions,
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: minLevel
                );
            }

            Log.Logger = loggerConfig.CreateLogger();

            Log.Information("Serilog initialized. Level={Level}, FilePath={Path}, SqlTable={Table}, SqlAutoCreate={AutoCreate}",
                minLevel, logsPath, tableName, autoCreate);

            AppDomain.CurrentDomain.ProcessExit += (_, __) => SafeFlush();
            AppDomain.CurrentDomain.DomainUnload += (_, __) => SafeFlush();
        }

        public static IDisposable Push(string name, object value) =>
            LogContext.PushProperty(name, value ?? "null");

        public static void SafeFlush()
        {
            try { Log.CloseAndFlush(); }
            catch { /* ignore */ }
        }
    }
}
