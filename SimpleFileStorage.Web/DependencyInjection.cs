﻿using System.ComponentModel.Design.Serialization;
using Microsoft.EntityFrameworkCore;
using SimpleFileStorage.Web.AppServices;
using SimpleFileStorage.Web.AppServices.Contract;
using SimpleFileStorage.Web.DataAccess;
using SimpleFileStorage.Web.DataAccess.Contract;
using SimpleFileStorage.Web.Infrastructure;

namespace SimpleFileStorage.Web;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string is required.");
        }
        
        services
            .AddAppServices()
            .AddInfrastructure(connectionString)
            .AddDataAccess();
        
        return services;
    }

    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StoredFilesContext>();
        context.Database.Migrate();
        return host;
    }
    
    private static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IGuidProvider, GuidProvider>();
        services.AddTransient<IFileService, FileService>();
        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<StoredFilesContext>(options => options.UseNpgsql(connectionString));
        return services;
    }

    private static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IFileSystemRepository, FileSystemRepository>();
        return services;
    }
}