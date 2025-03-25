using Domain.Base;
using Domain.Enums;
using Domain.Utils;

namespace Domain.Entities
{
    /// <summary>
    /// Task entity
    /// </summary>
    public sealed class Task : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public TaskStatusEnum Status { get; private set; }
        public TaskPriorityEnum Priority { get; private set; }
        public DateTime? DueDate { get; private set; }
        public int UserId { get; private set; }
        public int CategoryId { get; private set; }

        private Task() { }

        public static Result<Task> Create(
            string name,
            string description,
            TaskStatusEnum status,
            TaskPriorityEnum priority,
            DateTime? dueDate,
            int userId,
            int categoryId
        )
        {
            var result = ValidateAll(name, description, priority, status, userId, categoryId);

            if (!result.Success)
                return Result.MakeFailure<Task>(result.Message);

            var task = new Task()
            {
                Name = name,
                Description = description,
                Status = status,
                Priority = priority,
                DueDate = dueDate,
                UserId = userId,
                CategoryId = categoryId
            };

            return Result.MakeSuccess(task);
        }

        public Operation Update(
            string name,
            string description,
            TaskStatusEnum status,
            TaskPriorityEnum priority,
            DateTime? dueDate,
            int categoryId
        )
        {
            var result = ValidateAll(name, description, priority, status, UserId, categoryId);

            if (!result.Success)
                return Operation.MakeFailure(result.Message);

            Name = name;
            Description = description;
            Status = status;
            Priority = priority;
            DueDate = dueDate;
            CategoryId = categoryId;

            return Operation.MakeSuccess();
        }

        private static Operation ValidateAll(
            string name,
            string description,
            TaskPriorityEnum priority,
            TaskStatusEnum status,
            int userId,
            int categoryId
        )
        {
            var error = ValidateName(name);

            if (!string.IsNullOrEmpty(error))
                return Operation.MakeFailure(error);

            error = ValidateDescription(description);

            if (!string.IsNullOrEmpty(error))
                return Operation.MakeFailure(error);

            error = ValidateUserId(userId);

            if (!string.IsNullOrEmpty(error))
                return Operation.MakeFailure(error);

            error = ValidateCategoryId(categoryId);

            if (!string.IsNullOrEmpty(error))
                return Operation.MakeFailure(error);

            error = ValidateEnums(priority, status);

            if (!string.IsNullOrEmpty(error))
                return Operation.MakeFailure(error);

            return Operation.MakeSuccess();
        }

        private static string ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "Name must be provided";

            if (name.Length < 1)
                return "Name must be at least 1 character long";

            if (name.Length > 128)
                return "Name must be no more than 128 characters long";

            return string.Empty;
        }

        private static string ValidateDescription(string description)
        {
            if(description == null)
                return string.Empty;

            if (description.Length > 100_000)
                return "Description must be no more than 100000 characters long";

            return string.Empty;
        }

        private static string ValidateUserId(int userId)
        {
            if (userId <= 0)
                return "Task with invalid user";

            return string.Empty;
        }

        private static string ValidateCategoryId(int categoryId)
        {
            if (categoryId <= 0)
                return "Task with invalid category";

            return string.Empty;
        }

        private static string ValidateEnums(TaskPriorityEnum priorityEnum, TaskStatusEnum statusEnum)
        {
            if (!priorityEnum.IsInRange())
                return "Task with invalid priority";

            if (!statusEnum.IsInRange())
                return "Task with invalid status";

            return string.Empty;
        }
    }
}
