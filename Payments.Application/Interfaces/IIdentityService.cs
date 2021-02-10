using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Payments.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool IsAuthenticated, string UserId)> AuthenticateAsync(string username, string password);

        Task<IEnumerable<string>> GetUserRolesAsync(string username);

        Task<IEnumerable<Claim>> GetUserClaimsAsync(string username);
    }
}
