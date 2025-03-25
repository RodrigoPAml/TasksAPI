using Domain.Entities;

namespace Domain.Models.Users
{
    public class UpdateUsernameModel
    {
        public User User { get; set; }
        public string NewUsername { get; set; }
    }
}
