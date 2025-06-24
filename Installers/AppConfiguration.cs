using Scalar.AspNetCore;

namespace LeUs.Installers
{
    public static class ApplicationBuilderExtensions
    {
        public static void CustomApplicationConfiguration(this WebApplication app)
        {
            app.UseSwagger(opt =>
            {
                opt.RouteTemplate = "openapi/{documentName}.json";
            });
            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Scalar Example";
                opt.Theme = ScalarTheme.Mars;
                opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
            });
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseStatusCodePagesWithRedirects("/404");
            app.UseCors();
            //app.UseHttpsRedirection();
            app.UseAntiforgery();
            app.UseStaticFiles();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            app.MapAdditionalIdentityEndpoints();
            app.MapControllers();
            app.UseResponseCompression();
            // app.UseAuthentication();
            // app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = [new MyAuthorizationFilter()]
            });
            app.MapHealthChecks("/health");
            //app.UseRateLimiter();
            app.Run();
        }
    }
}