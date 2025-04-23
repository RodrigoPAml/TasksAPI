using Infra.Base;
using Infra.Contexts;
using Infra.Entities;
using Domain.Enums;
using Domain.Models.Tasks;
using Domain.Models.Responses;
using Infra.Implementations.Query;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using TaskEntity = Domain.Entities.Task;

namespace Infra.Implementations.Repositories
{
    /// <summary>
    /// Repository implementation for the Task entity
    /// </summary>
    public class TaskRepository : Repository<DbTask>, ITaskRepository
    {
        public TaskRepository(DatabaseContext ctx, IServiceProvider provider) : base(ctx.Tasks, provider)
        {
        }

        public async Task<bool> ExistsByName(string name, int? userId, int? currentId)
        {
            var filter = new Filter<DbTask>(x => x.Name == name);

            if (userId.HasValue && userId != default)
                filter.And(x => x.UserId == userId.Value);

            if (currentId.HasValue && currentId != default)
                filter.And(x => x.Id != currentId.Value);

            return await _dbSet.AnyAsync(filter.GetExpression());
        }

        public new async Task<TaskEntity> GetById(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
                return null;

            return _mapper.Map<TaskEntity>(entity);
        }

        public async Task<List<TaskEntity>> GetByUser(int userId)
        {
            var entities = await _dbSet
                .Where(x => x.UserId == userId)
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();

            return entities
               .Select(x => _mapper.Map<TaskEntity>(x))
               .ToList();
        }
       
        public async Task<GetPagedResponse<TaskEntity>> GetPaged(int pageNumber, int pageSize, TasksFilterModel filter, TaskOrderByEnum? ordering)
        {
            var queryFilter = new Filter<DbTask>();

            if(filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Name))
                    queryFilter.And(x => x.Name.Contains(filter.Name));

                if (filter.Status != null)
                    queryFilter.And(x => x.Status == (int)filter.Status);

                if(filter.Priority != null)
                    queryFilter.And(x => x.Priority == (int)filter.Priority);

                if(filter.CategoryId != null)
                    queryFilter.And(x => x.CategoryId == filter.CategoryId);

                if (filter.UserId != null)
                    queryFilter.And(x => x.UserId == filter.UserId);
            }

            var query = _dbSet
                .Where(queryFilter.GetExpression())
                .AsNoTrackingWithIdentityResolution();

            switch(ordering)
            {
                case TaskOrderByEnum.NameAsc:
                    query = query.OrderBy(x => x.Name);
                    break;
                case TaskOrderByEnum.NameDesc:
                    query = query.OrderByDescending(x => x.Name);
                    break;
        
                case TaskOrderByEnum.DueDateAsc:
                    query = query.OrderBy(x => x.DueDate);
                    break;
                case TaskOrderByEnum.DueDateDesc:
                    query = query.OrderByDescending(x => x.DueDate);
                    break;
                case TaskOrderByEnum.PriorityAsc:
                    query = query.OrderBy(x => x.Priority);
                    break;
                case TaskOrderByEnum.PriorityDesc:
                    query = query.OrderByDescending(x => x.Priority);
                    break;
                case TaskOrderByEnum.IdAsc:
                    query = query.OrderBy(x => x.Id);
                    break;
                case TaskOrderByEnum.IdDesc:
                default:
                    query = query.OrderByDescending(x => x.Id);
                    break;
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new GetPagedResponse<TaskEntity>
            {
                Count = totalCount,
                Items = items.Select(x => _mapper.Map<TaskEntity>(x)).ToList(),
                Pages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        public async Task Create(TaskEntity task)
        {
            var entity = _mapper.Map<DbTask>(task);
            await CreateAsync(entity);
        }

        public async Task Update(TaskEntity task)
        {
            var currentEntity = await _dbSet
                .Where(x => x.Id == task.Id)
                .FirstOrDefaultAsync();

            _mapper.Map(task, currentEntity);

            await UpdateAsync(currentEntity);
        }

        public new async Task Delete(int id)
        {
            await DeleteAsync(id);
        }
    }
}