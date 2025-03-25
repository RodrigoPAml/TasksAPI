using Domain.Base;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Users.UpdateUserPassword
{
    /// <summary>
    /// Command to update an User password
    /// </summary>
    public class UpdateUserPasswordCommand : IRequest<Operation>
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
