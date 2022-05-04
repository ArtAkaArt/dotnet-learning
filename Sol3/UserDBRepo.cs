using UserContextLib;
using Microsoft.EntityFrameworkCore;
using Sol3.Profiles;
using System.Security.Cryptography;

namespace Sol3
{
    public class UserDBRepo
    {
        private readonly UserContext context;
        public UserDBRepo(UserContext context)
        {
            this.context = context;
        }
        public async Task<User> FindUser(string lgn)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Login == lgn);
        }
        public async Task RegisterUser(UserDTO userReg)
        {
            if (userReg != null) 
            {
                var user = new User();
                CreatePasswordHash(userReg.Password, out byte[] hash, out byte[] salt);
                user.Login = userReg.Login;
                user.Password = hash;
                user.PasswordSalt = salt;
                user.Email = userReg.Email;
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
        public async Task<bool> UpdateUnit(UserDTO userUpd)
        {
            var user = await FindUser(userUpd.Login);
            if (user != null)
            {
                CreatePasswordHash(userUpd.Password, out byte[] hash, out byte[] salt);
                user.PasswordSalt = salt;
                user.Password = hash;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        private void CreatePasswordHash(string pwd, out byte[] pwdHash, out byte[] pwdSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                pwdSalt = hmac.Key;
                pwdHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pwd));
            }
        }
        public async Task<bool> VerifyUser (UserDTO userVer)
        {
            var user = await FindUser(userVer.Login);
            if (user == null)
                return false;
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userVer.Password));
                return computedHash.SequenceEqual(user.Password);
            }  
        }
    } 
}
