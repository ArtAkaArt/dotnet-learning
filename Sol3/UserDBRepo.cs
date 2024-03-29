﻿using UserContextLib;
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
        public async Task<User> FindUser(string login)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Login == login);
        }
        public async Task<UserInfoDTO> RegisterUser(UserDTO userReg)
        {
            if (userReg == null) 
                throw new ArgumentNullException("Пустой UserDTO");
            var user = new User();
            CreatePasswordHash(userReg.Password, out byte[] hash, out byte[] salt);
            user.Login = userReg.Login;
            user.Password = hash;
            user.PasswordSalt = salt;
            user.Role = "User";
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return new UserInfoDTO {Login = user.Login, Role = user.Role };

        }
        public async Task UpdateUser(UserDTO userUpd, User user)
        {
            if (user == null)
                throw new ArgumentNullException("Пустой UserDTO");
            CreatePasswordHash(userUpd.Password, out byte[] hash, out byte[] salt);
            user.PasswordSalt = salt;
            user.Password = hash;
            await context.SaveChangesAsync();

        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<bool> VerifyPwd (string passwordVer, User user)
        {
            if (user == null)
                return false;
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordVer));
                return computedHash.SequenceEqual(user.Password);
            }  
        }
    } 
}
