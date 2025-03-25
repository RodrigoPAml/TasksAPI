namespace Domain.Models.Users
{
    public class CreateUserModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string HashedPassword { get; set; }
    }
}
