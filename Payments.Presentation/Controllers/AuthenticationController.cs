using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Interfaces;
using Payments.Presentation.Authentication;

namespace Payments.Presentation.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IIdentityService _authenticationService;
        private readonly JwtTokenManager _jwtTokenManager;

        public AuthenticationController(IIdentityService authenticationService, JwtTokenManager jwtTokenManager)
        {
            _authenticationService = authenticationService;
            _jwtTokenManager = jwtTokenManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            var auth = await _authenticationService.AuthenticateAsync(username, password);

            if (!auth.IsAuthenticated)
            {
                return BadRequest(
                    new
                    {
                        Message = "invalid username or password."
                    });
            }

            var userClaims = await _authenticationService.GetUserClaimsAsync(username);
            var userRoles = await _authenticationService.GetUserRolesAsync(username);

            var token = _jwtTokenManager.CreateJwtToken(username, auth.UserId, userRoles, userClaims);

            return Ok(
                new
                {
                    Username = username,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                });
        }
    }
}
