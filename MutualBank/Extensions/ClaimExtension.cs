using System.Security.Claims;

namespace MutualBank.Extensions
{
    public static class ClaimExtension
    {
        public static int GetId(this ClaimsPrincipal User)
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

        }
    }
}
