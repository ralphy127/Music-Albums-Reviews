namespace MAR.Domain.Interfaces;

public interface IBaseService<TType> where TType : class {
    Task<IEnumerable<TType>> GetAllAsync();

    Task<TType?> GetByIdAsync(int id);

    Task<TType?> GetByFieldAsync<TField>(string fieldName, TField value);
    
    Task AddAsync(TType entity);
    
    Task UpdateAsync(TType entity);

    Task DeleteAsync(TType entity);
}