using Domain.Entities;

namespace Domain.Models.Users
{
    public class UpdateUserPasswordModel
    {
        public User User { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordHashed { get; set; }
    }
}
