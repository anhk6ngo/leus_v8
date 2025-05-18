using LeUs.Installers;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.InstallServicesFromAssemblyContainer<Program>(config);
var app = builder.Build();
app.CustomApplicationConfiguration();