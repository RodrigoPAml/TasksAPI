using MediatR;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Authentication.ResetPassword
{
    /// <summary>
    /// Command to reset an account password
    /// </summary>
    public class ResetPasswordCommand : IRequest<Operation>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int VerificationCode { get; set; }
    }
}
