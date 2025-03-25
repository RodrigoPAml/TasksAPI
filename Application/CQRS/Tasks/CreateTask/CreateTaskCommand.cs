using MediatR;
using Domain.Base;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.CQRS.Tasks.CreateTask
{
    /// <summary>
    /// Command to create a new Task
    /// </summary>
    public class CreateTaskCommand : IRequest<Operation>
    {
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
