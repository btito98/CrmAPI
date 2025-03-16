using CRM.Domain.Entities;

namespace CRM.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        Task<int> SaveChangesAsync();
    }
}
