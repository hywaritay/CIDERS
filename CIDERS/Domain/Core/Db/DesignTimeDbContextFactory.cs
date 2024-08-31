using CIDERS.Domain.Core.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class CiderContextFactory : IDesignTimeDbContextFactory<CiderContext>
{
    public CiderContext CreateDbContext(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Get the connection string
        var connectionString = configuration.GetConnectionString("cider");

        // Configure DbContextOptionsBuilder
        var optionsBuilder = new DbContextOptionsBuilder<CiderContext>();
        optionsBuilder.UseSqlServer(connectionString);

        // Create and return the DbContext
        return new CiderContext(optionsBuilder.Options);
    }
}
