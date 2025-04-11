namespace MAR.Application.Services;

using Microsoft.Extensions.Logging;
using MAR.Domain.Models;
using MAR.Domain.Interfaces;

public class BaseService<TModel> : IBaseService<TModel> where TModel : BaseModel
{
    protected readonly IBaseRepository<TModel> _repository;
    protected readonly ILoggerAdapter<TModel> _logger;

    public BaseService(IBaseRepository<TModel> repository, ILoggerAdapter<TModel> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
         _logger.LogInformation(LogEvent.GetAll, "Getting entities of type: {TypeName}", typeof(TModel).Name);
        IEnumerable<TModel> collection = await _repository.GetAllAsync();

        if ((collection is null) || (!collection.Any()))
        {
            _logger.LogWarning((int)LogEvent.GetAllNotFound, "No entities of type: {TypeName} were found", typeof(TModel).Name);
            return new List<TModel>();
        }

        return collection;
    }

    public async Task<TModel?> GetByIdAsync(int id)
    {
        _logger.LogInformation(LogEvent.GetById, "Getting entity of type: {TypeName} and ID: {Id}", typeof(TModel).Name, id);

        TModel? entity = await _repository.GetByIdAsync(id);

        if (entity is null)
        {
            _logger.LogWarning((int)LogEvent.GetByIdNotFound, "Entity of type: {TypeName} and ID: {Id} was not found", typeof(TModel).Name, id);
        }

        return entity;
    }

    public async Task<TModel?> GetByFieldAsync<TField>(string fieldName, TField value)
    {   
        _logger.LogInformation(LogEvent.GetByField, "Getting entity of type: {TypeName} by field: {FieldName} with value: {Value}", typeof(TModel).Name, fieldName, value);

        TModel? entity = await _repository.GetByFieldAsync(fieldName, value);

        if (entity is null)
        {
            _logger.LogWarning((int)LogEvent.GetByFieldNotFound, "Entity of type: {TypeName} with field: {FieldName} and value: {Value} was not found", typeof(TModel).Name, fieldName, value);
        }

        return entity;
    }
    
    public virtual async Task AddAsync(TModel entity)
    {
        if (entity is null)
        {
            _logger.LogError((int)LogEvent.AddNullEntity, "Attempted to add a null entity of type: {TypeName}", typeof(TModel).Name);
            throw new ArgumentNullException(nameof(entity));
        }

        _logger.LogInformation(LogEvent.AddEntity, "Adding entity of type: {TypeName}", typeof(TModel).Name);
        await _repository.AddAsync(entity);
        _logger.LogInformation(LogEvent.AddNullEntity, "Successfully added entity of type: {TypeName}", typeof(TModel).Name);
    }

    public async Task UpdateAsync(TModel entity)
    {
        if (entity is null)
        {
            _logger.LogError((int)LogEvent.UpdateNullEntity, "Attempted to update a null entity of type: {TypeName}", typeof(TModel).Name);
            throw new ArgumentNullException(nameof(entity));
        }

        _logger.LogInformation(LogEvent.UpdateEntity, "Updating entity of type: {TypeName}", typeof(TModel).Name);
        await _repository.UpdateAsync(entity);
    }

    public virtual async Task DeleteAsync(TModel entity)
    {
        if (entity is null)
        {
            _logger.LogError((int)LogEvent.DeleteNullEntity, "Attempted to delete a null entity of type: {TypeName}", typeof(TModel).Name);
            throw new ArgumentNullException(nameof(entity));
        }

        _logger.LogInformation((int)LogEvent.DeleteEntity, "Deleting entity of type: {TypeName}", typeof(TModel).Name);
        await _repository.DeleteAsync(entity);
    }
}