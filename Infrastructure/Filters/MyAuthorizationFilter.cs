namespace LeUs.Infrastructure.Filters;

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        return httpContext.User.IsInRole(RoleConstants.AdministratorRole);
    }
}