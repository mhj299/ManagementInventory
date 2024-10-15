using System.Security.Claims;

namespace WebUi.Components.Layout.Identity
{
    public class CustomAuthorizationService : ICustomAuthorizationService
    {
        private string specificClaim;

        public bool CustomClaimChecker(ClaimsPrincipal user, string specification)
        {
            if (!user.Identity!.IsAuthenticated) return false;

            var getClaim = user.HasClaim(_ => _.Type == specificClaim);
            if (!getClaim) return false;

            var getState = user.Claims.FirstOrDefault(_ => _.Type == specificClaim)!.Value;

            return Convert.ToBoolean(getState) is true;
        }
    }


}