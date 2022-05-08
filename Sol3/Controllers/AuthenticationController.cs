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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Auth([FromBody]UserDTO userLog)
        {
            var user = await repo.FindUser(userLog.Login);
            if (user == null)
                return NotFound("User не найден");
            if (!await repo.VerifyPwd(userLog.Password, user))
                return BadRequest("Password не подходит");

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login), new Claim (ClaimTypes.Role, user.Role) };
            var asd = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(120)),
                signingCredentials: new SigningCredentials(asd, SecurityAlgorithms.HmacSha256));
            /* не уверен как тут именно надо делать
            Response.Cookies.Append("Authorization", "bearer " + new JwtSecurityTokenHandler().WriteToken(jwt));
            Response.Headers.Authorization = "bearer " + new JwtSecurityTokenHandler().WriteToken(jwt); 
            */
            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
        [HttpPost("api/user/password/update"), Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<ActionResult> UpdatePwd(UserPwdUpdDTO userUpd)
        {
            var name = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (name.Value == userUpd.Login)
                return BadRequest("Нельзя менять пароль другого пользователя");
            var user = await repo.FindUser(userUpd.Login);
            if (user == null)
                return NotFound("User не найден");
            
            if (!await repo.VerifyPwd(userUpd.currentPassword, user))
                return BadRequest("Password не подходит");

            var userDto = new UserDTO { Login = userUpd.Login, Password = userUpd.newPassword };
            if (!await repo.UpdateUser(userDto, user))
                return StatusCode(501);
            return NoContent();
        }
        [HttpGet("api/user/current"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfoDTO))]
        public ActionResult ShowInfo()
        {
            var user = HttpContext.User;
            string name = user.FindFirst(ClaimTypes.Name).Value;
            string role = user.FindFirst(ClaimTypes.Role).Value;
            var info = new UserInfoDTO { Login = name, Role = role };
            return Ok(info);
        }
    }
}
