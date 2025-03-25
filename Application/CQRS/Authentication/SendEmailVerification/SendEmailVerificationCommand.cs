using MediatR;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Authentication.SendEmailVerification
{
    /// <summary>
    /// Command to send a verification for the new account email
    /// </summary>
    public class SendEmailVerificationCommand : IRequest<Operation>
    {
        [Required]
        public string Email { get; set; }
    }
}
