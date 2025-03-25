using AutoMapper;
using Infra.Implementations.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Base
{
    /// <summary>
    /// Commom implementation for repositories to use
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> where T : BaseDbEntity, new()
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly IMapper _mapper;

        public Repository(DbSet<T> dbSet, IServiceProvider provider)
        {
            _dbSet = dbSet;
            _mapper = provider.GetService<IMapper>();
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _dbSet
               .AnyAsync(x => x.Id == id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet
                .Where(x => x.Id == id)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            if (_dbSet.Local.Any(e => e.Id == entity.Id))
                throw new Exception("Entity is already added and tracked");

            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            // Find tracked locally
            var existingEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);

            if (existingEntity != null)
            {
                if (!ReferenceEquals(existingEntity, entity))
                    throw new Exception("The entity being tracked is not the same instance as the provided entity");

                // If entity is tracked locally and is the same reference then no action is needed
                return;
            }
            
            // No tracked entity, lets find if exists in the db
            if (!await _dbSet.AnyAsync(x => x.Id == entity.Id))
                throw new Exception("Entity not found to update");

            // Attach
            _dbSet.Attach(entity);
            _dbSet.Entry(entity).State = EntityState.Modified;
        }

        public async Task UpdateSomeFieldsAsync(T entity, Fields<T> fields)
        {
            var count = fields?.Count();

            if (fields == null || count == 0)
                throw new Exception("No fields to update");

            // Find tracked locally
            var existingEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);

            if (existingEntity != null)
            {
                if (!ReferenceEquals(existingEntity, entity))
                    throw new Exception("The entity being tracked is not the same instance as the provided entity");

                // If entity is tracked locally and is the same reference then no action is needed
                // Because the entity itself is already begin tracked
                return;
            }

            // No tracked entity, lets find if exists in the db
            if (!await _dbSet.AnyAsync(x => x.Id == entity.Id))
                throw new Exception("Entity not found to update");

            _dbSet.Attach(entity);

            var entry = _dbSet.Entry(entity);

            foreach (var property in fields.GetNames())
            {
                var propertyEntry = entry.Property(property);

                if (propertyEntry != null)
                    propertyEntry.IsModified = true;
                else
                    throw new ArgumentException($"Property '{property}' does not exist on the entity.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            await Task.Run(() =>
                _dbSet.Remove(new()
                {
                    Id = id
                })
            );
        }

        public bool ExistsById(int id)
        {
            return _dbSet
               .Any(x => x.Id == id);
        }

        public T GetById(int id)
        {
            return _dbSet
                .Where(x => x.Id == id)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault();
        }

        public void Create(T entity)
        {
            if (_dbSet.Local.Any(e => e.Id == entity.Id))
                throw new Exception("Entity is already added");

            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            // Find tracked locally
            var existingEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);

            if (existingEntity != null)
            {
                if (!ReferenceEquals(existingEntity, entity))
                    throw new Exception("The entity being tracked is not the same instance as the provided entity");

                // If entity is tracked locally and is the same reference then no action is needed
                return;
            }

            // No tracked entity, lets find if exists in the db
            if (!_dbSet.Any(x => x.Id == entity.Id))
                throw new Exception("Entity not found to update");

            // Attach
            _dbSet.Attach(entity);
            _dbSet.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateSomeFields(T entity, Fields<T> fields)
        {
            var count = fields?.Count();

            if (fields == null || count == 0)
                throw new Exception("No fields to update");

            // Find tracked locally
            var existingEntity = _dbSet.Local.FirstOrDefault(e => e.Id == entity.Id);

            if (existingEntity != null)
            {
                if (!ReferenceEquals(existingEntity, entity))
                    throw new Exception("The entity being tracked is not the same instance as the provided entity");

                // If entity is tracked locally and is the same reference then no action is needed
                // Because the entity itself is already begin tracked
                return;
            }

            // No tracked entity, lets find if exists in the db
            if (!_dbSet.Any(x => x.Id == entity.Id))
                throw new Exception("Entity not found to update");

            _dbSet.Attach(entity);

            var entry = _dbSet.Entry(entity);

            foreach (var property in fields.GetNames())
            {
                var propertyEntry = entry.Property(property);

                if (propertyEntry != null)
                    propertyEntry.IsModified = true;
                else
                    throw new ArgumentException($"Property '{property}' does not exist on the entity.");
            }
        }

        public void Delete(int id)
        {
            _dbSet.Remove(new()
            {
                Id = id
            });
        }
    }
}
