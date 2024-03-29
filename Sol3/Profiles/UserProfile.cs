﻿namespace Sol3.Profiles
{
    public class UserDTO
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class UserPwdUpdDTO
    {
        public string Login { get; set; } = null!;
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
    public class UserInfoDTO
    {
        public string Login { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
