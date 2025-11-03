using BL;
using BL.Api;
using BL.Services;
using Dal;
using Dal.Api;
using Dal.Models;
using Dal.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using server_pra.Models;
using server_pra.Services;
using Serilog;
using System;

Console.WriteLine("🟢 Starting server build...");

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("✅ WebApplicationBuilder created");

// Configure Serilog - only for controllers
Console.WriteLine("⚙️ Configuring Serilog...");
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Fatal()
    .MinimumLevel.Override("server.Controllers", Serilog.Events.LogEventLevel.Information)
    .MinimumLevel.Override("server_pra.Services.FileCheckerBackgroundService", Serilog.Events.LogEventLevel.Fatal)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("LogsConnection"),
        tableName: "SerilogLogs",
        autoCreateSqlTable: true)
    .CreateLogger();

builder.Host.UseSerilog();
Console.WriteLine("✅ Serilog configured");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"🔌 Loaded connection string: {connectionString}");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);

// שירותים כלליים
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// הגדרות CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:54515", "http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// רישום שירותים - DAL ו-BL
builder.Services.AddScoped<IDal>(sp =>
{
    var context = sp.GetRequiredService<AppDbContext>();
    return new DalManager(context);
});
builder.Services.AddScoped<IBl, BlManager>();
builder.Services.AddScoped<IDalSystem, DalSystemService>();
builder.Services.AddScoped<IDalImportStatus, DalImportStatusService>();
builder.Services.AddScoped<IDalImportDataSource, DalImportDataSourceService>();
builder.Services.AddScoped<IDalDataSourceType, DalDataSourceTypeService>();
builder.Services.AddScoped<IDalFileStatus, DalFileStatusService>();
builder.Services.AddScoped<IDalImportControl, DalImportControlService>();
builder.Services.AddScoped<IDalImportProblem, DalImportProblemService>();
builder.Services.AddScoped<IBlImportStatus, BlImportStatusService>();
builder.Services.AddScoped<IBlSystem, BlSystemService>();
builder.Services.AddScoped<IBlDataSourceType, BlDataSourceTypeService>();
builder.Services.AddScoped<IBlTabImportDataSource, BlTabImportDataSourceService>();
builder.Services.AddScoped<IBlFileStatus, BlFileStatusService>();
builder.Services.AddScoped<IBlimportControl, BlImportControlService>();
builder.Services.AddScoped<IblDashboardService, BlDashboardService>();
builder.Services.AddScoped<IdalDashboard, DalDashboardService>();
builder.Services.AddScoped<DalFileStatusService>();
builder.Services.AddScoped<ErrorReportService>();

// Hosted services
builder.Services.AddSingleton<FileCheckerBackgroundService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<FileCheckerBackgroundService>());
builder.Services.AddHostedService<UpdateImportStatusService>();

// שירותי עזר נוספים
builder.Services.AddSingleton<ILoggerService, LoggerService>();

// Build
var app = builder.Build();
Console.WriteLine("✅ WebApplication built");

// הגדרות סביבה והרצה
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("🚀 Running the server...");
app.Run();
