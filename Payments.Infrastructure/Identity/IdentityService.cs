using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Payments.Application.Interfaces;
using Payments.Infrastructure.Identity;

namespace Payments.Presentation.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(bool, string)> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return default;

            var user = await _userManager.FindByNameAsync(username);

            return (user != null 
                && await _userManager.CheckPasswordAsync(user, password), user?.Id);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return default;

            var user = await _userManager.FindByNameAsync(username);

            return user != null
                ? await _userManager.GetRolesAsync(user)
                : null;
        }

        public async Task<IEnumerable<Claim>> GetUserClaimsAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return default;

            var user = await _userManager.FindByNameAsync(username);

            return user != null
                ? await _userManager.GetClaimsAsync(user)
                : null;
        }
    }
}
