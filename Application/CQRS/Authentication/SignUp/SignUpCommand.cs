using MediatR;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Authentication.SignUp
{
    /// <summary>
    /// Command to create an account
    /// </summary>
    public class SignUpCommand : IRequest<Operation>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int VerificationCode { get; set; }
    }
}
