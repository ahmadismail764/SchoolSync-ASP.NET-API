using SchoolSync.Domain.IServices;
using Microsoft.AspNetCore.Authorization;

namespace SchoolSync.API.Middleware;

public class TokenValidationMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        // Check if the endpoint allows anonymous access
        var endpoint = context.GetEndpoint();
        var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

        // Skip token validation for anonymous endpoints
        if (allowAnonymous)
        {
            await _next(context);
            return;
        }

        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrEmpty(token))
        {
            var isValid = await tokenService.IsTokenValidAsync(token);
            if (!isValid)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token has been revoked");
                return;
            }
        }

        await _next(context);
    }
}
