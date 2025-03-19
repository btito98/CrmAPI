using AutoMapper;
using CRM.Application.Exceptions;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.Interfaces;
using System.Linq.Expressions;
using CRM.Shared.Results;
using Microsoft.EntityFrameworkCore;

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

        public virtual async Task<Result<TEntityResultDTO>> AddAsync(TEntityDTO dto)
        {
            var entityMapped = _mapper.Map<TEntity>(dto);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var entityResult = await _repository.AddAsync(entityMapped);
                await _unitOfWork.CommitAsync();
                return Result<TEntityResultDTO>.Success(_mapper.Map<TEntityResultDTO>(entityResult));
            }
            catch (Exception )
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<Result<IEnumerable<TEntityResultDTO>>> AddRangeAsync(IEnumerable<TEntityDTO> dtos)
        {
            var entitiesMapped = _mapper.Map<IEnumerable<TEntity>>(dtos);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entitiesResult = await _repository.AddRangeAsync(entitiesMapped);
                await _unitOfWork.CommitAsync();
                return Result<IEnumerable<TEntityResultDTO>>.Success(_mapper.Map<IEnumerable<TEntityResultDTO>>(entitiesResult));
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<Result<IEnumerable<TEntityResultDTO>>> FindAsync(Expression<Func<TEntityDTO, bool>> predicate)
        {
            var predicateMapped = _mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            var entities = await _repository.Find(predicateMapped).ToListAsync();

            if (!entities.Any())
                return Result<IEnumerable<TEntityResultDTO>>.Failure(new Error("BaseService.FindAsync", "Nenhum resultado encontrado."));

            return Result<IEnumerable<TEntityResultDTO>>.Success(_mapper.Map<IEnumerable<TEntityResultDTO>>(entities));
        }

        public virtual async Task<Result<TEntityResultDTO>> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return Result<TEntityResultDTO>.Failure(
                    new Error("BaseService.GetByIdAsync", $"{typeof(TEntity).Name} com ID {id} não foi encontrado."));

            return Result<TEntityResultDTO>.Success(_mapper.Map<TEntityResultDTO>(entity));
        }

        public virtual async Task<Result> RemoveAsync(Guid id, string usuarioAlteracao)
        {
            if (string.IsNullOrWhiteSpace(usuarioAlteracao))
                return Result.Failure(new Error("BaseService.RemoveAsync", "Usuário de alteração não pode ser nulo ou vazio."));

            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return Result.Failure(new Error("BaseService.RemoveAsync", $"{typeof(TEntity).Name} com ID {id} não foi encontrado."));

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
                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Failure(new Error("BaseService.RemoveAsync", ex.Message));
            }
        }

        public virtual async Task<Result<TEntityResultDTO>> UpdateAsync(Guid id, TEntityDTO dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return Result<TEntityResultDTO>.Failure(
                    new Error("BaseService.UpdateAsync", $"{typeof(TEntity).Name} com ID {id} não foi encontrado."));

            var entityMapped = _mapper.Map(dto, entity);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var entityResult = await _repository.UpdateAsync(entityMapped);
                await _unitOfWork.CommitAsync();
                return Result<TEntityResultDTO>.Success(_mapper.Map<TEntityResultDTO>(entityResult));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result<TEntityResultDTO>.Failure(new Error("BaseService.UpdateAsync", ex.Message));
            }
        }
    }
}