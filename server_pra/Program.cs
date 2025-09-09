using Dal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

using BL.Api;
using BL.Services;
using Dal.Api;
using Dal.Services;

var builder = WebApplication.CreateBuilder(args);

//// רישום DbContext עם חיבור מתוך appsettings.json
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular",
//        policy =>
//        {
//            policy.WithOrigins("http://localhost:4200") // ה־Frontend שלך
//                  .AllowAnyHeader()
//                  .AllowAnyMethod();
//        });
//});

builder.Services.AddDbContext<AppDbContext>(options =>
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
builder.Services.AddScoped<IDalSystem, DalSystemService>();
builder.Services.AddScoped<IDalImportStatus, DalImportStatusService>();
builder.Services.AddScoped<IDalImportDataSource, DalImportDataSourceService>();
builder.Services.AddScoped<IDalDataSourceType, DalDataSourceTypeService>();
builder.Services.AddScoped<IBlImportStatus, BlImportStatusService>();
builder.Services.AddScoped<IBlSystem, BlSystemService>();
builder.Services.AddScoped<IBlDataSourceType, BlDataSourceTypeService>();
builder.Services.AddScoped<IBlTabImportDataSource, BlTabImportDataSourceService>();
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
