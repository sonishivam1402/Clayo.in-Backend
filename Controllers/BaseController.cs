using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace e_commerce_backend.Controllers
{
    public class BaseController : Controller
    {
        protected Guid? GetUserId()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                              ?? User.FindFirst("UserId")?.Value;

            return userIdClaim != null ? Guid.Parse(userIdClaim) : (Guid?)null;
        }

        protected Guid? GetRoleId()
        {
            var roleIdClaim = User.FindFirst("RoleId")?.Value;

            return roleIdClaim != null ? Guid.Parse(roleIdClaim) : (Guid?)null;
        }

        protected Boolean GetRoleAccess()
        {
            var roleIdClaim = User.FindFirst("RoleId")?.Value;
            if(roleIdClaim != null && roleIdClaim == "7f7d8021-923b-429c-b76c-22462fd34b55")
                return true;

            return false;
        }
    }
}
