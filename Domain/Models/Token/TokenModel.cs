using Domain.Enums;

namespace Domain.Models.Token
{
    public class TokenModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public ProfileTypeEnum Profile { get; set; }
    }
}
