using MediatR;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Tasks.DeleteTask
{
    /// <summary>
    /// Command to delete a Task
    /// </summary>
    public class DeleteTaskCommand : IRequest<Operation>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
