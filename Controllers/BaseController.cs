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

            if(roleIdClaim != null && roleIdClaim == "7F7D8021923B429CB76C22462FD34B55")
                return true;

            return false;
        }
    }
}
