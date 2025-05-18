using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Leus.Controllers;

public class TokenController(
    ITokenService identityService,
    IValidator<TokenRequest> tokenVal,
    IValidator<RefreshTokenRequest> refreshTokenVal) : AnonymousApiController
{
    [HttpPost("login")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IResult> LocalLogin(TokenRequest model)
    {
        var valResult = await tokenVal.ValidateAsync(model);
        if (!valResult.IsValid)
        {
            var problemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Instance = "api/token/login"
            };
            return Results.Problem(problemDetails);
        }

        var response = await identityService.LocalLoginAsync(model);
        return Results.Ok(response);
    }

    [HttpPost("refresh")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public async Task<IResult> LocalRefresh([FromBody] RefreshTokenRequest model)
    {
        var valResult = await refreshTokenVal.ValidateAsync(model);
        if (!valResult.IsValid)
        {
            var problemDetails = new HttpValidationProblemDetails(valResult.ToDictionary())
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Instance = "api/token/refresh"
            };
            return Results.Problem(problemDetails);
        }

        var response = await identityService.GetLocalRefreshTokenAsync(model);
        return Results.Ok(response);
    }
}