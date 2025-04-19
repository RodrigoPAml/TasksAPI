using Domain.Enums;

namespace Domain.Models.Tasks
{
    public class TasksFilterModel
    {
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public TaskStatusEnum? Status { get; set; }
        public TaskPriorityEnum? Priority { get; set; }
    }
}