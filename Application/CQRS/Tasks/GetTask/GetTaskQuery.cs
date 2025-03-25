using MediatR;
using Domain.Base;
using Task = Domain.Entities.Task;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Tasks.GetTask
{
    /// <summary>
    /// Query to get a Task
    /// </summary>
    public class GetTaskQuery : IRequest<Result<Task>>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
