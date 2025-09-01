using Dal;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// קריאת appsettings.json וחיבור למסד הנתונים
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// רישום ה־Controllers
builder.Services.AddControllers();

var app = builder.Build();

// בדיקה שה־API רץ
app.MapGet("/", () => "API is running...");

// מיפוי כל ה־Controllers
app.MapControllers();

app.Run();
