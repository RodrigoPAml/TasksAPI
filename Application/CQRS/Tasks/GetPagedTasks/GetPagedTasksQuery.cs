using MediatR;
using Domain.Base;
using Domain.Models.Responses;
using Task = Domain.Entities.Task;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Tasks.GetPagedTasks
{
    /// <summary>
    /// Query to paged tasks
    /// </summary>
    public class GetPagedTasksQuery : IRequest<Result<GetPagedResponse<Task>>>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Required]
        [Range(1, 100)]
        public int PageSize { get; set; }
    }
}
