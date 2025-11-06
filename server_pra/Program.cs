
using BL;
using BL.Api;
using BL.Services;
using Dal;
using Dal.Api;
using Dal.Models;
using Dal.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using server_pra.Models;
using server_pra.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Xml;





Console.WriteLine("🟢 Starting server build...");


var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("✅ WebApplicationBuilder created");


var columnOptions = new Serilog.Sinks.MSSqlServer.ColumnOptions();
columnOptions.AdditionalColumns = new List<Serilog.Sinks.MSSqlServer.SqlColumn>
{
    new Serilog.Sinks.MSSqlServer.SqlColumn("UserName", System.Data.SqlDbType.NVarChar, dataLength: 255)
};


Console.WriteLine("⚙️ Configuring Serilog...");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("server.Controllers", Serilog.Events.LogEventLevel.Information)
    .MinimumLevel.Override("server_pra.Services.FileCheckerBackgroundService", Serilog.Events.LogEventLevel.Fatal)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("LogsConnection"),
        sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
        {
            TableName = "SerilogLogs",
            AutoCreateSqlTable = true
        },
        columnOptions: columnOptions)
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

builder.Services.AddScoped<ValidationService>();

builder.Services.AddScoped<ErrorReportService>();
builder.Services.AddScoped<LoadBulkTable>();


// Hosted services
//builder.Services.AddSingleton<FileCheckerBackgroundService>();

builder.Services.AddScoped<FileCheckerService>();
//builder.Services.AddHostedService(provider => provider.GetRequiredService<FileCheckerService>());
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


Console.WriteLine("🚀 Testing the email...");
try
        {
            string fromAddress = "rachel87549@gmail.com"; // כתובת הג'ימייל שלך
            string appPassword = "ngtswaoklfefyrlv"; // בלי רווחים
            string toAddress = "racheli5426@gmail.com"; // כתובת הנמען
            string subject = "בדיקת שליחת מייל";
            string body = "שלום! זהו מייל בדיקה שנשלח דרך קוד C#.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587, // TLS
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, appPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            Console.WriteLine("המייל נשלח בהצלחה!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("שגיאה בשליחה: " + ex.Message);
        }
    
app.Run();
