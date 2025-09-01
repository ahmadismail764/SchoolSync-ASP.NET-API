using SchoolSync.Domain.IRepositories;
using System.Security.Claims;

namespace SchoolSync.API.Middleware
{
    public class UserValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public UserValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUserRepo userRepo)
        {
            // Skip validation for non-authenticated requests
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                await _next(context);
                return;
            }

            // Skip validation for auth endpoints
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (path.Contains("/auth/login") || path.Contains("/auth/register")))
            {
                await _next(context);
                return;
            }

            // Get user ID from JWT claims
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid token: User ID not found");
                return;
            }

            // Check if user exists and is not deleted
            var user = await userRepo.GetAsync(userId);
            if (user == null || user.IsDeleted)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("User account is no longer valid");
                return;
            }

            await _next(context);
        }
    }
}
