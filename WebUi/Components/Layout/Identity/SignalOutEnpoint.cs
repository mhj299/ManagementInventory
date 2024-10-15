using Application.Extension.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebUi.Components.Layout.Identity
{
internal static class SignalOutEndpoint
    {
        public static void  MapSignOutEndpoint(this IEndpointRouteBuilder endpoints)
        {
            ArgumentNullException.ThrowIfNull(endpoints);
            var accountGroup = endpoints.MapGroup("/Account");
            accountGroup.MapPost("/Logout", async (ClaimsPrincipal user, SignInManager<ApplicationUser> SignInManager) =>
            {
                await SignInManager.SignOutAsync();
                return TypedResults.LocalRedirect("/Account/Login");
            });
           
        }
    }
}
