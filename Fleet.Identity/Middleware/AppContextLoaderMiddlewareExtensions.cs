using Microsoft.AspNetCore.Builder;

namespace Regira.Fleet.Identity.Middleware;

public static class AppContextLoaderMiddlewareExtensions
{
    public static IApplicationBuilder UseAppContextLoader(this IApplicationBuilder builder)
        => builder.UseMiddleware<AppContextLoaderMiddleware>();
}