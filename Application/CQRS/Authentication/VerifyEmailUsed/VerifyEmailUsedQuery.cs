using MediatR;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Authentication.VerifyEmailUsed
{
    /// <summary>
    /// Query to verify if a email is in use
    /// </summary>
    public class VerifyEmailUsedQuery : IRequest<Result<bool>>
    {
        [Required]
        public string Email { get; set; }
    }
}
