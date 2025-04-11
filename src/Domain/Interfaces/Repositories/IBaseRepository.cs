namespace MAR.Domain.Interfaces;

using MAR.Domain.Models;

public interface IBaseRepository<TType> where TType : BaseModel
{
    Task<IEnumerable<TType>> GetAllAsync();

    Task<TType?> GetByIdAsync(int id);

    Task<TType?> GetByFieldAsync<TField>(string fieldName, TField value);

    Task AddAsync(TType entity);

    Task UpdateAsync(TType entity);

    Task DeleteAsync(TType entity);
}