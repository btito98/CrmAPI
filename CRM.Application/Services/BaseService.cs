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
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repository = unitOfWork.GetRepository<TEntity>();
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public virtual async Task<TEntityResultDTO> AddAsync(TEntityDTO dto)
        {
            var entityMapped = _mapper.Map<TEntity>(dto);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var entityResult = await _repository.AddAsync(entityMapped);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<TEntityResultDTO>(entityResult);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntityResultDTO>> AddRangeAsync(IEnumerable<TEntityDTO> dtos)
        {
            var entitiesMapped = _mapper.Map<IEnumerable<TEntity>>(dtos);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entitiesResult = await _repository.AddRangeAsync(entitiesMapped);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<IEnumerable<TEntityResultDTO>>(entitiesResult);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntityResultDTO>> FindAsync(Expression<Func<TEntityDTO, bool>> predicate)
        {
            var predicateMapped = _mapper.Map<Expression<Func<TEntity, bool>>>(predicate);

            var entities = _repository.Find(predicateMapped);

            return _mapper.Map<IEnumerable<TEntityResultDTO>>(entities);
        }


        public virtual async Task<TEntityResultDTO?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} com ID {id} não foi encontrado.");
            return _mapper.Map<TEntityResultDTO>(entity);
        }

        public virtual async Task RemoveAsync(Guid id, string usuarioAlteracao)
        {
            if (string.IsNullOrWhiteSpace(usuarioAlteracao))
                throw new ArgumentException("Usuário de alteração não pode ser nulo ou vazio.", nameof(usuarioAlteracao));

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"{typeof(TEntity).Name} com ID {id} não foi encontrado.");

            if (entity is BaseEntity baseEntity)
            {
                baseEntity.Update(usuarioAlteracao);
                baseEntity.Deactivate();
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _repository.RemoveAsync(entity); 
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<TEntityResultDTO> UpdateAsync(Guid id, TEntityDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException($"{typeof(TEntity).Name} com ID {id} não foi encontrado.");

            var entityMapped = _mapper.Map(dto, entity);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entityResult = await _repository.UpdateAsync(entityMapped);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<TEntityResultDTO>(entityResult);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}