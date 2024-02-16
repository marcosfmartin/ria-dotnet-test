using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NeoSmart.Caching.Sqlite;
using NeoSmart.Caching.Sqlite.AspNetCore;
using RiaServer.Model;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqliteCache(options =>
{
    options.CachePath = @"customers.db";
}); ;


var app = builder.Build();// initialize, with default settings
var dbContext = new CustomerContext();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("/customers", ([FromBody] IEnumerable<Customer> customers, [FromServices] SqliteCache cache) =>
{
    var bytes = cache.Get("cachedCustomers");
    var jsonUtfReader = new Utf8JsonReader(bytes);
    List<Customer> cachedCustomers = JsonSerializer.Deserialize<List<Customer>>(ref jsonUtfReader) ?? [];

    foreach (var customer in customers)
    {
        cache.Set("cachedCustomers", JsonSerializer.SerializeToUtf8Bytes<List<Customer>>(cachedCustomers));
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

app.Run();

