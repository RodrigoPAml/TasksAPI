using Domain.Base;
using Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Tasks.UpdateTask
{
    /// <summary>
    /// Command to update a task
    /// </summary>
    public class UpdateTaskCommand : IRequest<Operation>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public TaskStatusEnum Status { get; set; }

        [Required]
        public TaskPriorityEnum Priority { get; set; }

        public DateTime? DueDate { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
