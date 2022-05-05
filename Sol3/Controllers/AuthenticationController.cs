using Microsoft.AspNetCore.Mvc;
using Sol3.Profiles;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Sol3.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private UserDBRepo repo;
        private readonly ILogger<AuthenticationController> logger;
        private readonly string key;
        public AuthenticationController(UserDBRepo repo, ILogger<AuthenticationController> logger, IConfiguration conf)
        {
            this.repo = repo;
            this.logger = logger;
            key = conf["SecretKeys:Key1"];
        }
        [HttpPost("api/user/auth")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Auth([FromBody]UserDTO userLog)
        {
            if (!await repo.VerifyUser(userLog))
                return NotFound("User не найден");

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userLog.Login) };
            var asd = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(120)),
                signingCredentials: new SigningCredentials(asd, SecurityAlgorithms.HmacSha256));
            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
    }
}
