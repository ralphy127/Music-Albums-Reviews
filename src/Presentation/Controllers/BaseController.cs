namespace MAR.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MAR.Domain.Interfaces;
using MAR.Domain.Models;

public class BaseController<TModel> : ControllerBase, IBaseController<TModel> where TModel : BaseModel
{
    private readonly IBaseService<TModel> _service;

    private readonly ILoggerAdapter<TModel> _logger;

    public BaseController(IBaseService<TModel> service, ILoggerAdapter<TModel> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<TModel>>> GetAll()
    {
        var enumerable = await _service.GetAllAsync();
        
        if (!enumerable.Any())
        {
            return NotFound();
        }

        return Ok(enumerable.ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TModel>> GetById(int id) 
    {
        var entity =  await _service.GetByIdAsync(id);

        if (entity is null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    [HttpGet("{fieldName}/{value}")]
    public async Task<ActionResult<TModel>> GetByField<TField>(string fieldName, TField value)
    {
        var entity = await _service.GetByFieldAsync(fieldName, value);

        if(entity is null)
        {
            return NotFound();
        }

        return Ok(entity);
    }

    [HttpPost]
    public async Task<ActionResult> Add(TModel entity)
    {
        if (entity == null)
        {
            return BadRequest("Entity cannot be null.");
        }

        try
        {
            await _service.AddAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding the entity.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

    [HttpPut]
    public async Task<ActionResult> Update(TModel entity)
    {
        if (entity?.Id is null)
        {
            return BadRequest("Entity must not be null and have a valid Id.");
        }

        try
        {
            var existingEntity = await _service.GetByIdAsync(entity.Id);
            if (existingEntity is null)
            {
                return NotFound();
            }

            await _service.UpdateAsync(entity);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the entity.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(TModel entity)
    {
        if (entity?.Id is null)
        {
            return BadRequest("Entity must not be null and have a valid Id.");
        }

        var existingEntity = await _service.GetByIdAsync(entity.Id);
        if (existingEntity is null)
        {
            return NotFound();
        }

        await _service.DeleteAsync(existingEntity);
        return NoContent();
    }
}