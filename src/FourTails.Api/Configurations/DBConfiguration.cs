using FourTails.Core.DomainModels;
using FourTails.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FourTails.Api.Configurations;

public static class DBConfiguration
{
    public static void AddDatabaseConfiguration(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<FTDBContext>(options => // add separate context class
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), builder =>
            {
                builder.MigrationsAssembly("FourTails.Api");
                builder.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "MigrationHistory");
            });
        });

        service.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<FTDBContext>();
    }

    public static void UseDBMigrationConfiguration(this IApplicationBuilder app)
    {
        var context = app
        .ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope()
        .ServiceProvider
        .GetService<FTDBContext>();

        if (context.Database.IsNpgsql())
        {
            context.Database.Migrate();
        }
    }
}