using Infra.Base;
using Infra.Contexts;
using Infra.Entities;
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
       
        public async Task<GetPagedResponse<TaskEntity>> GetPaged(int userId, int pageNumber, int pageSize)
        {
            var query = _dbSet
                .Where(x => x.UserId == userId)
                .AsNoTrackingWithIdentityResolution();

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
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