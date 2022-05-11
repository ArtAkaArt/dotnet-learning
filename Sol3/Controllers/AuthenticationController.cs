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
        public AuthenticationController(UserDBRepo repo, ILogger<AuthenticationController> logger, KeysConfiguration KeyConfig)
        {
            this.repo = repo;
            this.logger = logger;
            key = KeyConfig.Key1;
        }
        /// <summary>
        /// LogIn метод
        /// </summary>
        /// <param name="userLog">UserDto: должен содержать логин и пароль</param>
        /// <returns></returns>
        [HttpPost("api/user/auth")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> Auth([FromBody]UserDTO userLog)
        {
            if (userLog == null)
                return BadRequest("User не передан");
            var user = await repo.FindUser(userLog.Login);
            if (user == null)
                return NotFound("User не найден");
            if (!await repo.VerifyPwd(userLog.Password, user))
                return BadRequest("Password не подходит");
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login), new Claim (ClaimTypes.Role, user.Role) };
            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(120)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));
            /* не уверен как тут именно надо делать
            Response.Cookies.Append("Authorization", "bearer " + new JwtSecurityTokenHandler().WriteToken(jwt));
            Response.Headers.Authorization = "bearer " + new JwtSecurityTokenHandler().WriteToken(jwt); 
            */
            return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
        }
        /// <summary>
        /// Изменение пароля пользователя
        /// </summary>
        /// <param name="userUpd">UserPwdUpdDTO: должен содержать логин, старый и новый пароль</param>
        /// <returns></returns>
        [HttpPost("api/user/password/update"), Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        public async Task<ActionResult> UpdatePwd(UserPwdUpdDTO userUpd)
        {
            if (userUpd == null)
                return BadRequest("User не передан");
            var name = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (name != userUpd.Login)
                return BadRequest("Нельзя менять пароль другого пользователя");
            var user = await repo.FindUser(userUpd.Login);
            if (user == null)
                return NotFound("User не найден");
            if (!await repo.VerifyPwd(userUpd.CurrentPassword, user))
                return BadRequest("Password не подходит");
            var userDto = new UserDTO { Login = userUpd.Login, Password = userUpd.NewPassword };
            await repo.UpdateUser(userDto, user);
            return Ok("Пароль обновен");
        }
        /// <summary>
        /// Вывод UserInfoDTO - клеймов аунтифицированного пользователя (логин и роль)
        /// </summary>
        /// <returns></returns>
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
