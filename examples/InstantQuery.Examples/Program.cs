using InstantQuery.Examples;
using InstantQuery.Examples.DAL;
using InstantQuery.Examples.Orders;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDbContext<ExamplesDbContext>(options =>
{
    var connection = new SqliteConnection("Filename=:memory:");
    connection.Open();
    options.UseSqlite(connection);
    options.LogTo(Console.WriteLine);
});

//builder.Services.AddSwaggerDocument();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);
    c.DescribeAllParametersInCamelCase();
    c.UseAllOfToExtendReferenceSchemas();
    c.SwaggerGeneratorOptions.ParameterFilters.Add(new SwaggerNullableParameterFilter());
});

builder.Services.AddAutoMapper(typeof(OrderDetailsMapProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.UseCors(b => b
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:44422")
    .AllowCredentials());

app.Run();
