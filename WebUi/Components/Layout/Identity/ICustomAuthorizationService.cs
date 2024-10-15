using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;



namespace WebUi.Components.Layout.Identity
{
    public interface ICustomAuthorizationService
    {
        bool CustomClaimChecker(ClaimsPrincipal user, string specification);
    }
}
