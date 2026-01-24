using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SeriesDB.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class SeriesDBDbContextFactory : IDesignTimeDbContextFactory<SeriesDBDbContext>
{
    public SeriesDBDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        SeriesDBEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<SeriesDBDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new SeriesDBDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SeriesDB.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
