using Application.Extension.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace WebUi.Components.Layout.Identity
{
    internal sealed class IdentityRevalidatingAuthStateProvider(ILoggerFactory loggerFactory,
       IServiceScopeFactory scopeFactory,IOptions<IdentityOptions> options )
        : RevalidatingServerAuthenticationStateProvider(loggerFactory)
      
    {
        protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(20);
        protected async override Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authentificationState,CancellationToken
            cancellationToken)
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            return await ValidateSecurityStampAsync(userManager, authentificationState.User);

        }
        private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            if(user is null)
            {
                return false;
            }
            else if (!userManager.SupportsUserSecurityStamp)
            {
                return true;
            }
            else
            {
                var principalStamp = principal.FindFirstValue(options.Value.ClaimsIdentity.SecurityStampClaimType);
                var userStamp = await userManager.GetSecurityStampAsync(user);
                return principalStamp == userStamp;
            }
        }
    }
}
