using System.Threading.RateLimiting;
using LeUs.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.OpenApi.Models;

namespace LeUs.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            services.AddRazorComponents()
                .AddInteractiveServerComponents();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            services.AddCascadingAuthenticationState();
            services.AddScoped<IdentityUserAccessor>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddInfrastructureService();
            services.AddMediatRService(assemblies);
            services.AddManagers(assemblies);
            services.AddScoped<IServerCurrentUserService, ServerCurrentUserService>();
            services.AddLazyCache();
            services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);
            services.AddHttpClient("Gps",
                c =>
                {
                    c.BaseAddress =
                        new Uri("https://api.dmxsmart.com/api/openapi/v12/services");
                });
            services.AddHttpClient("uspsZone", c => { c.BaseAddress = new Uri("https://postcalc.usps.com/"); });
            services.AddHttpClient("UnitedBridge", c => { c.BaseAddress = new Uri("https://united-bridge.com"); });
            services.AddHealthChecks();
            // services.AddHostedService<SystemWorker>();
            // services.AddRateLimiter(options =>
            // {
            //     options.AddPolicy("PerUserPolicy", context =>
            //     {
            //         var user = context.User.Identity;
            //         var userId = user?.IsAuthenticated == true
            //             ? user.Name
            //             : context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
            //         return RateLimitPartition.GetFixedWindowLimiter(userId, _ => new FixedWindowRateLimiterOptions
            //         {
            //             PermitLimit = 10,
            //             Window = TimeSpan.FromSeconds(5),
            //             QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            //             QueueLimit = 2
            //         });
            //     });
            // });
        }
    }
}