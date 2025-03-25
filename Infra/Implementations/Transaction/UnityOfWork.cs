using Infra.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using Application.Interfaces.Transaction;

namespace Infra.Implementations.Transaction
{
    /// <summary>
    /// Transaction control implementation
    /// </summary>
    public class UnityOfWork : IUnityOfWork
    {
        private readonly DatabaseContext _db;

        private IDbContextTransaction _transaction = null;

        public UnityOfWork(DatabaseContext db)
        {
            _db = db;
        }

        public async Task Begin()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task Commit()
        {
            await _transaction?.CommitAsync();
        }

        public async Task Rollback()
        {
            await _transaction?.RollbackAsync();
        }
    }
}