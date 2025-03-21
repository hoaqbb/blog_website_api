using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace blog_website_api.Extentions
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecretKey"]))
                };

                opts.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        ctx.Request.Cookies.TryGetValue("accessToken", out string accessToken);
                        if (!string.IsNullOrEmpty(accessToken))
                            ctx.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
