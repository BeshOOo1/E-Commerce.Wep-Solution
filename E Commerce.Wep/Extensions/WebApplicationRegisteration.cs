using E_Commerce.Domain.Contract;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace E_Commerce.Wep.Extensions
{
    public static class WebApplicationRegisteration
    {
        public static async Task<WebApplication> MigarateDatbaseAsync(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var dbContextService = Scope.ServiceProvider.GetRequiredService<StoreDbConext>();
            var PendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
               await dbContextService.Database.MigrateAsync();
            return app;
        }
        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
           await using var Scope = app.Services.CreateAsyncScope();
            var DataIntializerService = Scope.ServiceProvider.GetRequiredService<IDataIntializer>();
            await DataIntializerService.IntializeAsync();
            return app;
        }
    }
}
