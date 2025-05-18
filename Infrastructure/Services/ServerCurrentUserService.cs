using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace LeUs.Infrastructure.Services;

public class ServerCurrentUserService(AuthenticationStateProvider authenticationStateProvider, IHttpContextAccessor httpContextAccessor)
    : IServerCurrentUserService
{
    public async Task<string?> UserId()
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId.NotIsNullOrEmpty())
        {
            return userId;
        }
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.GetUserId();
    }
}