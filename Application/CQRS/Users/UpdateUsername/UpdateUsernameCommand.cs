using Domain.Base;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Users.UpdateUsername
{
    /// <summary>
    /// Command to update an Username
    /// </summary>
    public class UpdateUsernameCommand : IRequest<Operation>
    {
        [Required]
        public string NewUsername { get; set; }
    }
}
