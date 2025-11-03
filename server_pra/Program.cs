<<<<<<< HEAD
﻿    using BL;
=======
using BL;
>>>>>>> origin/main
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
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog - only for controllers
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Fatal() // Block everything by default
    .MinimumLevel.Override("server.Controllers", Serilog.Events.LogEventLevel.Information) // Only controllers
    .MinimumLevel.Override("server_pra.Services.FileCheckerBackgroundService", Serilog.Events.LogEventLevel.Fatal) // Block FileChecker
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("LogsConnection"),
        tableName: "SerilogLogs",
        autoCreateSqlTable: true)
    .CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddDbContext<Dal.Models.AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,              // כמה פעמים לנסות מחדש
            maxRetryDelay: TimeSpan.FromSeconds(10), // הפסקה מקסימלית בין ניסיונות
            errorNumbersToAdd: null        // אפשר להשאיר null כדי לכסות שגיאות נפוצות
        )
    )
);

// שירותים נוספים
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // כתובת הלקוח
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// Add the missing connectionString variable initialization at the top of the file.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IDal>(sp =>
{
    var context = sp.GetRequiredService<Dal.Models.AppDbContext>();
    return new DalManager(context);
});
//builder.Services.AddScoped<IDal, DalManager>();  // המימוש שלך של ה־DAL
builder.Services.AddScoped<IBl, BlManager>();  // המימוש שלך של ה־BL
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
// Register the concrete service so you can resolve it for manual testing,
// while still registering it as a hosted service.
builder.Services.AddSingleton<FileCheckerBackgroundService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<FileCheckerBackgroundService>());

builder.Services.AddScoped<DalFileStatusService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy
            .WithOrigins("http://localhost:54515")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
//builder.Services.AddScoped<IBl>(sp => new BlManager(sp.GetRequiredService<IDal>()));

builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddScoped<ErrorReportService>();

var app = builder.Build();






if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// סוואגר
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngular");

// ודאי שאת משתמשת במדיניות הנכונה
app.UseCors("AllowAngular");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
