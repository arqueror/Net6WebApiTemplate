using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Utilities.Http
{
    public static class HttpHelpers
    {
        public static async Task<bool> IsConnectedtoInternet()
        {
            bool IsConnectedtoInternet;
            try
            {
                using (var client = new HttpClient())
                {
                    using (await client.GetAsync("https://github.com/favicon.ico"))
                    {
                        IsConnectedtoInternet = true;
                    }
                }
            }
            catch
            {
                IsConnectedtoInternet = false;
            }
            
            return IsConnectedtoInternet;
        }


        /// <summary>
        /// Checks if current user has an specific role assigned
        /// </summary>
        /// <param name="role">role to look</param>
        /// <returns>true if current user claims principal is in specified role</returns>
        public static bool HasPermission(this ClaimsPrincipal currentUser, string role ="Administrator")
        {
            return currentUser.IsInRole(role);
        }

        /// <summary>
        /// Checks for custom claims within current user claims principal
        /// </summary>
        /// <param name="principal">current claims principal</param>
        /// <param name="claimType">claim type</param>
        /// <param name="claimValue">claim value</param>
        /// <returns>true if current user claims principal is in specified role</returns>
        public static bool HasPermission(this ClaimsPrincipal principal, string claimType = ClaimTypes.Role, string claimValue = "Administrator")
        {
            //HasClaim("role", "specificRole")
            return ((ClaimsIdentity)principal.Identity)?.HasClaim(claimType, claimValue) ?? false;
        }
    }
}
