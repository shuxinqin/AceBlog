using Ace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AceBlog.Web.Common
{
    public class UserSession : AceSession<int?>
    {
        public string AccountName { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }

        public List<Claim> ToClaims()
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("UserId", this.UserId == null ? "" : this.UserId.ToString()));
            claims.Add(new Claim("AccountName", this.AccountName ?? ""));
            claims.Add(new Claim("Name", this.Name ?? ""));
            claims.Add(new Claim("NickName", this.NickName ?? ""));

            return claims;
        }

        public static UserSession Parse(IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claims)
            {
                string userId = claims.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value ?? "";

                UserSession session = new UserSession()
                {
                    UserId = string.IsNullOrEmpty(userId) ? null : (int?)int.Parse(userId),
                    AccountName = claims.Claims.FirstOrDefault(x => x.Type == "AccountName")?.Value ?? "",
                    Name = claims.Claims.FirstOrDefault(x => x.Type == "Name")?.Value ?? "",
                    NickName = claims.Claims.FirstOrDefault(x => x.Type == "NickName")?.Value ?? "",
                };
                return session;
            }

            throw new ArgumentException(message: "The principal must be a ClaimsPrincipal", paramName: nameof(principal));
        }
    }
}
