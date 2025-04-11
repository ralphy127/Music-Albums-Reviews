namespace MAR.Domain.Interfaces;

using Microsoft.AspNetCore.Mvc;
using MAR.Domain.Models;

public interface IBaseController<TModel> where TModel : BaseModel
{
    Task<ActionResult<List<TModel>>> GetAll();

    Task<ActionResult<TModel>> GetById(int id);

    Task<ActionResult<TModel>> GetByField<TField>(string fieldName, TField value);

    Task<ActionResult> Add(TModel entity);

    Task<ActionResult> Update(TModel entity);
    
    Task<ActionResult> Delete(TModel entity);
}