using blog_website_api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace blog_website_api.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
        {
            services.AddDbContext<BlogDbContext>(opt =>
                opt.UseNpgsql(config.GetConnectionString("PostgresDb"))
            );
        
            return services;
        }
    }
}
