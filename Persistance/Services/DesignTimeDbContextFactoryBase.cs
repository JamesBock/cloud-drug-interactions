using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UWPLockStep.Persistance.Services
{
    //public abstract class DesignTimeDbContextFactoryBase<TContext> :
    //    IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    //{
    //    private const string ConnectionStringName = "MyAppConnectionString";
    //    private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

    //    public TContext CreateDbContext(string[] args)
    //    {
    //        var basePath = Directory.GetCurrentDirectory() + string.Format("{0}..{0}WebUI", Path.DirectorySeparatorChar);
    //        return Create(basePath, Environment.GetEnvironmentVariable(AspNetCoreEnvironment));
    //    }

    //    protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

    //    private TContext Create(string basePath, string environmentName)
    //    {

    //        IConfigurationRoot configuration = new ConfigurationBuilder()
    //            .SetBasePath(basePath)
    //            .AddJsonFile("appsettings.json")
    //            .AddJsonFile($"appsettings.Local.json", optional: true)
    //            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
    //            .AddEnvironmentVariables()
    //            .Build();

    //        var connectionString = configuration.GetConnectionString(ConnectionStringName);

    //        return Create(connectionString);
    //    }

    //    private TContext Create(string connectionString)
    //    {
    //        if (string.IsNullOrEmpty(connectionString))
    //        {
    //            throw new ArgumentException($"Connection string '{ConnectionStringName}' is null or empty.", nameof(connectionString));
    //        }

    //        Console.WriteLine($"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");

    //        var optionsBuilder = new DbContextOptionsBuilder<TContext>();

    //        optionsBuilder.UseSqlServer(connectionString);

    //        return CreateNewInstance(optionsBuilder.Options);
    //    }
    //}
}
