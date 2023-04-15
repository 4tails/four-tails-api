using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FourTails.Api.Configurations;

public static class JwtConfiguration
{
    public static void AddJwtConfiguration(this IServiceCollection service, IConfiguration configuration)
    {
        service
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetSection("JwtOptions:1:ValidIssuer").Value,
                    ValidAudience = configuration.GetSection("JwtOptions:2:ValidAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtOptions:0:IssuerSigningKey").Value))
                };
            });
    }
}