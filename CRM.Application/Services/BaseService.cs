using AutoMapper;
using CRM.Application.Exceptions;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace CRM.Application.Services
{
    public class BaseService<TEntityDTO, TEntity, TEntityResultDTO> :
        IBaseService<TEntityDTO, TEntity, TEntityResultDTO> where TEntity : BaseEntity
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public virtual async Task<TEntityResultDTO> AddAsync(TEntityDTO dto)
        {
            var entityMapped = _mapper.Map<TEntity>(dto);

            var entityResult = await _repository.AddAsync(entityMapped);

            return _mapper.Map<TEntityResultDTO>(entityResult);
        }

        public virtual async Task<IEnumerable<TEntityResultDTO>> AddRangeAsync(IEnumerable<TEntityDTO> dtos)
        {
            var entitiesMapped = _mapper.Map<IEnumerable<TEntity>>(dtos);

            var entitiesResult = await _repository.AddRangeAsync(entitiesMapped);

            return _mapper.Map<IEnumerable<TEntityResultDTO>>(entitiesResult);
        }

        public virtual async Task<IEnumerable<TEntityResultDTO>> FindAsync(Expression<Func<TEntityDTO, bool>> predicate)
        {
            var predicateMapped = _mapper.Map<Expression<Func<TEntity, bool>>>(predicate);

            var entities = await _repository.FindAsync(predicateMapped);

            return _mapper.Map<IEnumerable<TEntityResultDTO>>(entities);
        }

        public virtual async Task<TEntityResultDTO?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            return _mapper.Map<TEntityResultDTO>(entity);
        }

        public virtual async Task RemoveAsync(Guid id, string usuarioAlteracao)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null) throw new NotFoundException("Entity not found");

            await _repository.RemoveAsync(entity, usuarioAlteracao);
        }

        public virtual async Task<TEntityResultDTO> UpdateAsync(Guid id, TEntityDTO dto, string usuarioAlteracao)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null) throw new NotFoundException("Entity not found");

            var entityMapped = _mapper.Map(dto, entity);

            var entityResult = await _repository.UpdateAsync(entityMapped, usuarioAlteracao);

            return _mapper.Map<TEntityResultDTO>(entityResult);
        }
    }
}
