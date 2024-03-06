using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage.Infrastructure.DAL.Models;

namespace Storage.Infrastructure.DAL
{
    internal static class Extensions
    {
        private const string OptionSourceDataSectionName = "sourceCsvData";

        public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SourceCsvDataOptions>(configuration.GetRequiredSection(OptionSourceDataSectionName));

            return services;
        }
    }
}
