using Microsoft.EntityFrameworkCore;
using RiaServer.Model;

public class CustomerContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public string DbPath { get; }

    public CustomerContext()
    {
        DbPath = "customers.db";
    }

    // The following configures EF to use a Sqlite database file in the project folder.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
