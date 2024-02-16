using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NeoSmart.Caching.Sqlite;
using NeoSmart.Caching.Sqlite.AspNetCore;
using RiaServer.Models;
using RiaServer.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<CustomerService>();

builder.Services.AddSqliteCache(options =>
{
    options.CachePath = @"customers.db";
}); ;


var app = builder.Build();// initialize, with default settings

var cache = app.Services.GetService(typeof(SqliteCache));
List<Customer> cachedCustomers = new List<Customer>();
if (cache != null)
{
    var cacheService = (SqliteCache)cache;
    var bytes = cacheService.Get("cachedCustomers");
    var jsonUtfReader = new Utf8JsonReader(bytes);
    cachedCustomers = JsonSerializer.Deserialize<List<Customer>>(ref jsonUtfReader) ?? [];
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/customers", ([FromBody] IEnumerable<Customer> customers, [FromServices] SqliteCache cache, [FromServices] CustomerService customerService) =>
{

    foreach (var customer in customers)
    {
        customerService.ValidateCustomerFields(customer, cachedCustomers);
        customerService.AddCustomerInOrder(customer, cachedCustomers);
    }
    return true;
})
.WithName("PostCustomers")
.WithOpenApi();

app.MapGet("/customers", ([FromServices] IDistributedCache cache) =>
{
    var bytes = cache.Get("cachedCustomers") ?? [];
    var jsonUtfReader = new Utf8JsonReader(bytes);
    List<Customer> cachedCustomers = JsonSerializer.Deserialize<List<Customer>>(ref jsonUtfReader) ?? [];
    return cachedCustomers;

})
.WithName("GetCustomers")
.WithOpenApi();

app.Lifetime.ApplicationStopping.Register(() =>
{
    OnShutdown();
});

app.Run();

void OnShutdown()
{
    if (cache != null)
    {
        var cacheService = (SqliteCache)cache;
        cacheService.Set("cachedCustomers", JsonSerializer.SerializeToUtf8Bytes(cachedCustomers));
    }
}
