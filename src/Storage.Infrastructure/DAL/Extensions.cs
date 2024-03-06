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

        public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SourceCsvDataOptions>(configuration.GetRequiredSection(OptionSourceDataSectionName));
            services.AddScoped<ISourceDataRepository, SourceDataFromCSVRepository>();

            return services;
        }
    }
}
