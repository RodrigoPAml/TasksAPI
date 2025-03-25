using Task = Domain.Entities.Task;

namespace Domain.Models.Tasks
{
    public class UpdateTaskModel
    {
        public Task Task { get; set; }
        public UpdateTaskInfoModel Info { get; set; }
    }
}
