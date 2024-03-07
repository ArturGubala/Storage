using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage.Core.Repositories;
using Storage.Infrastructure.DAL.Models;
using Storage.Infrastructure.DAL.Repositories;

namespace Storage.Infrastructure.DAL
{
    internal static class Extensions
    {
        private const string OptionSourceDataSectionName = "sourceCsvData";

        public static IServiceCollection AddSQLite(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SourceCsvDataOptions>(configuration.GetRequiredSection(OptionSourceDataSectionName));
            services.AddScoped<ISourceDataRepository, SourceDataFromCSVRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IPriceRepository, PriceRepository>();
            services.AddSingleton<StorageDbContext>();

            // TODO: Make sure that this could be run from this extension and the way of call that is correctly.
            var dbContext = new StorageDbContext(configuration);
            dbContext.InitializeDatabase().Wait();

            return services;
        }
    }
}
