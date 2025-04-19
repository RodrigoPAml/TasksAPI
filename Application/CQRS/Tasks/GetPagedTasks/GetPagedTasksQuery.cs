using MediatR;
using Domain.Base;
using Domain.Enums;
using Domain.Models.Responses;
using Task = Domain.Entities.Task;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Tasks.GetPagedTasks
{
    /// <summary>
    /// Query to paged tasks, with filters and ordering
    /// </summary>
    public class GetPagedTasksQuery : IRequest<Result<GetPagedResponse<Task>>>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Required]
        [Range(1, 100)]
        public int PageSize { get; set; }

        [Range(1, int.MaxValue)]
        public int? CategoryId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public TaskStatusEnum? Status { get; set; }

        [Range(0, int.MaxValue)]
        public TaskPriorityEnum? Priority { get; set; }

        [Range(0, int.MaxValue)]
        public TaskOrderByEnum? Ordering { get; set; }
    }
}
