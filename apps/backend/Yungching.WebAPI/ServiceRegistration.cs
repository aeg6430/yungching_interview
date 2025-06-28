using Yungching.Application.IServices;
using Yungching.Application.Services;
using Yungching.Infrastructure.Contexts;
using Yungching.Infrastructure.IRepositories;
using Yungching.Infrastructure.Repositories;

namespace Yungching.WebAPI
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<DapperContext>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IStoreService, StoreService>();
        }
    }
}
