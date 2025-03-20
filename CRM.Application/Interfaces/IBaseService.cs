using CRM.Domain.Entities;
using CRM.Shared.Results;
using System.Linq.Expressions;

namespace CRM.Application.Interfaces
{
    public interface IBaseService<TEntityDTO, TEntity, TEntityResultDTO> where TEntity : BaseEntity
    {
        Task<Result<TEntityResultDTO>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<TEntityResultDTO>>> FindAsync(Expression<Func<TEntityDTO, bool>> predicate);
        Task<Result<TEntityResultDTO>> AddAsync(TEntityDTO dto);
        Task<Result<IEnumerable<TEntityResultDTO>>> AddRangeAsync(IEnumerable<TEntityDTO> dtos);
        Task<Result<TEntityResultDTO>> UpdateAsync(Guid id, TEntityDTO entity);
        Task<Result> RemoveAsync(Guid id, string usuarioAlteracao);
    }
}