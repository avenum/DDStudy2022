using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extentions
{
    public static class ClaimPrincipalExt
    {
        public static T? GetClaimValue<T>(this ClaimsPrincipal user, string claim)
        {
            var value = user.Claims.FirstOrDefault(x => x.Type == claim)?.Value;
            if (value != null)
                return Utils.Convert<T>(value);
            else
                return default;

        }
    }
}
