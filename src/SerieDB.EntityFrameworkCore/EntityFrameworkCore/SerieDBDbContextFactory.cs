using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SerieDB.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class SerieDBDbContextFactory : IDesignTimeDbContextFactory<SerieDBDbContext>
{
    public SerieDBDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        SerieDBEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<SerieDBDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new SerieDBDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SerieDB.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables();

        return builder.Build();
    }
}
