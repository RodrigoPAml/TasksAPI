using Domain.Enums;

namespace Domain.Models.Tasks
{
    public class UpdateTaskInfoModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskStatusEnum Status { get; set; }
        public TaskPriorityEnum Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int CategoryId { get; set; }
    }
}
