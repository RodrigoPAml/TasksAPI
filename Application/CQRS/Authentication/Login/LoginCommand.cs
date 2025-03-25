using MediatR;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Authentication.Login
{
    public class LoginCommand : IRequest<Result<string>>
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
