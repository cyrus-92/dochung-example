using Cyrus.Test.Infrastructure;
using Cyrus.Test.Service.Products;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cyrus.Test.Service
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDatabaseService, MSSQLDatabaseService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }

    }
}
