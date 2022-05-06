namespace Sol3.Profiles
{
    public class UserDTO
    {
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class UserPwdUpdDTO
    {
        public string Login { get; set; } = null!;
        public string currentPassword { get; set; } = null!;
        public string newPassword { get; set; } = null!;
    }
}
