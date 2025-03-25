using Domain.Base;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Users.DeleteUser
{
    /// <summary>
    /// Command to delete an User
    /// </summary>
    public class DeleteUserCommand : IRequest<Operation>
    {
        [Required]
        public string CurrentPassword { get; set; }
    }
}
