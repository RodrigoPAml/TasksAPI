using Infra.Base;
using Infra.Entities;
using Domain.Entities;
using Infra.Contexts;
using Domain.Interfaces.Repositories;
using Infra.Implementations.Query;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infra.Implementations.Repositories
{
    /// <summary>
    /// Repository implementation for the Category entity
    /// </summary>
    public class CategoryRepository : Repository<DbCategory>, ICategoryRepository
    {
        public CategoryRepository(DatabaseContext ctx, IServiceProvider provider) : base(ctx.Categories, provider)
        {
        }

        public async Task<bool> ExistsByName(string name, int? userId, int? currentId)
        {
            var filter = new Filter<DbCategory>(x => x.Name == name);

            if(userId.HasValue && userId != default)
                filter.And(x => x.UserId == userId.Value);

            if(currentId.HasValue && currentId != default)
                filter.And(x => x.Id != currentId.Value);
            
            return await _dbSet.AnyAsync(filter.GetExpression());
        }

        public async Task<bool> IsUsedInTasks(int id)
        {
            var filter = new Filter<DbCategory>(x => x.Id == id && x.Tasks.Any());

            return await _dbSet.AnyAsync(filter.GetExpression());
        }

        public new async Task<Category> GetById(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
                return null;

            return _mapper.Map<Category>(entity);
        }

        public async Task<bool> ExistsById(int id, int? userId)
        {
            var filter = new Filter<DbCategory>(x => x.Id == id);

            if (userId.HasValue && userId != default)
                filter.And(x => x.UserId == userId.Value);

            return await _dbSet.AnyAsync(filter.GetExpression());
        }

        public async Task<List<Category>> GetByUser(int userId)
        {
            var entities = await _dbSet
                .Where(x => x.UserId == userId)
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();

            return entities
                .Select(x => _mapper.Map<Category>(x))
                .ToList();
        }

        public async Task Create(Category category)
        {
            var entity = _mapper.Map<DbCategory>(category);
            await CreateAsync(entity);
        }

        public async Task Update(Category category)
        {
            var currentEntity = await _dbSet
                .Where(x => x.Id == category.Id)
                .FirstOrDefaultAsync();

            _mapper.Map(category, currentEntity);

            await UpdateAsync(currentEntity);
        }

        public new async Task Delete(int id)
        {
            await DeleteAsync(id);
        }
    }
}