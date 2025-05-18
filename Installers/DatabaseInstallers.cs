namespace LeUs.Installers
{
    public class DatabaseInstallers : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var identityConnectionString = configuration.GetConnectionString("IdentityConnection");
            var assemblyName = $"{typeof(DatabaseInstallers).Assembly.GetName().Name}";
            services.AddDatabaseCustomize<PortalContext>($"{connectionString}", assemblyName,
                databaseKind: DatabaseKind.Postges, useHangfire: true);
            services.AddDatabaseIdentity($"{identityConnectionString}", assemblyName,
                databaseKind: DatabaseKind.Postges);
            services.AddCookieOrJwtBearer($"{configuration["AppSettings:Secret"]}");
            services.AddCors();
            services.GetApplicationSettings(configuration);
            var gpsSettingsConfiguration = configuration.GetSection(nameof(ApiSetting));
            services.Configure<ApiSetting>(gpsSettingsConfiguration);
        }
    }
}