using elfBeauty.App.Interfaces;
using elfBeauty.Core.IRepository;
using elfBeauty.Core.Repository;
using elfBeauty.Infra.AestheticSvc;
using elfBeauty.Infra.Persistence;
using elfBeauty.Middleware.Logging;
using elfBeauty.Middleware.Monitoring;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<AestheticDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("BreweryDB")));

// Add component
builder.Services.AddHttpClient<IAestheticSvc, AestheticSvc>();
builder.Services.AddScoped<IBreweryCacheRepo, BreweryCacheRepo>();
builder.Services.AddScoped<IBreweryDbRepo, BreweryDbRepo>();
builder.Services.AddScoped<IAestheticSvc, AestheticSvc>();

// Seed SQLite DB, lazy loading
builder.Services.AddHostedService<BreweryDbSyncSvc>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Performance monitoring middleware
app.UseMiddleware<PerformanceMonitor>();

// Error handling logging middleware
app.UseMiddleware<ErrorHandlingLog>();

// Custom logging middleware
app.UseMiddleware<RequestResponseLog>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
