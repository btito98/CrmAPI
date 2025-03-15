using CRM.Domain.Entities;
using System.Linq.Expressions;

namespace CRM.Application.Interfaces
{
    public interface IBaseService<TEntityDTO, TEntity, TEntityResultDTO> where TEntity : BaseEntity
    {
        Task<TEntityResultDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntityResultDTO>> GetAllAsync();
        Task<IEnumerable<TEntityResultDTO>> FindAsync(Expression<Func<TEntityDTO, bool>> predicate);
        Task<TEntityResultDTO> AddAsync(TEntityDTO dto);
        Task<IEnumerable<TEntityResultDTO>> AddRangeAsync(IEnumerable<TEntityDTO> entities);
        Task<TEntityResultDTO> UpdateAsync(Guid id, TEntityDTO entity, string usuarioAlteracao);
        Task RemoveAsync(Guid id, string usuarioAlteracao);
    }
}
