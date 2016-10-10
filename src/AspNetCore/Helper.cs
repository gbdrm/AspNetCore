using System;
using System.Security.Claims;

namespace AspNetCore
{
    public class Helper
    {
        public static Guid? GetCurrentUserId(ClaimsPrincipal user, string requestCookie = null)
        {
            var currentUser = user.FindFirstValue(ClaimTypes.NameIdentifier);

            Guid userId;
            if (Guid.TryParse(currentUser, out userId))
            {
                return userId;
            }

            if (Guid.TryParse(requestCookie, out userId))
            {
                return userId;
            }

            return null;
        }
    }
}