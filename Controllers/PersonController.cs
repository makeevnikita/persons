using Microsoft.AspNetCore.Mvc;
using Skills.Repositories;
using Skills.Exceptions;
using Skills.Models.Dto;



namespace skill.Controllers;

[ApiController]
[Route("api/v1/persons")]
public class PersonController : ControllerBase
{
    private readonly IPersonRepository _personRepository; 

    private readonly ILogger<PersonController> _logger;
    
    public PersonController(IPersonRepository PersonRepository, ILogger<PersonController> logger)
    {
        _personRepository = PersonRepository;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] long id)
    {
        try
        {
            PersonDto person = await _personRepository.GetById(id);

            if (person == null)
            {
                _logger.LogInformation($"Person with id {id} not found");

                return NotFound();
            }
            else
            {
                return new JsonResult(person);
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500);
        }

    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<PersonDto> persons = await _personRepository.GetAll();

            return new JsonResult(persons);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long id)
    {
        try
        {
            await _personRepository.Delete(id);
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation($"Person with id {id} not found");

            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500);
        }

        return new JsonResult(new { result = "success" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long id, [FromBody] PersonDto person)
    {
        try
        {
            await _personRepository.Update(id, person);

            return new JsonResult(new { result = "success" });
        }
        catch (NotFoundException ex)
        {
            _logger.LogInformation($"Person with id {id} not found");

            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonDto person)
    {
        try
        {
            await _personRepository.Create(person);

            return new JsonResult(new { result = "success" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            
            return StatusCode(500);
        }
    }
}