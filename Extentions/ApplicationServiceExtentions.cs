using blog_website_api.Data.Entities;
using blog_website_api.Helpers.MappingProfiles;
using blog_website_api.Interfaces;
using blog_website_api.Repositories;
using blog_website_api.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace blog_website_api.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
        {
            services.AddDbContext<BlogDbContext>(opt =>
                opt.UseNpgsql(config.GetConnectionString("PostgresDb"))
            );

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
