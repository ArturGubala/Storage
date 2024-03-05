using Microsoft.Extensions.DependencyInjection;

namespace Storage.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
