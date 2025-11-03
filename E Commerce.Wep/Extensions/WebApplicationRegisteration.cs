using E_Commerce.Domain.Contract;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Wep.Extensions
{
    public static class WebApplicationRegisteration
    {
        public static WebApplication MigarateDatbase(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var DbContextService = Scope.ServiceProvider.GetRequiredService<StoreDbConext>();
            if (DbContextService.Database.GetPendingMigrations().Any())
                DbContextService.Database.Migrate();
            return app;
        }
        public static WebApplication SeedDatabase(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var DataIntializerService = Scope.ServiceProvider.GetRequiredService<IDataIntializer>();
            DataIntializerService.Intialize();
            return app;
        }
    }
}
