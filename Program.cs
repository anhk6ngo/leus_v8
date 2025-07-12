using LeUs.Installers;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.InstallServicesFromAssemblyContainer<Program>(config);
builder.WebHost.ConfigureKestrel(o =>
{
    o.Limits.MaxResponseBufferSize = 100*1024*1024;
});
var app = builder.Build();
app.CustomApplicationConfiguration();