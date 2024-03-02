using Microsoft.AspNetCore.Mvc;
using Skills.Repositories;
using Skills.Models.Dto;
using Skills.Models;



namespace skill.Controllers;

[ApiController]
[Route("api/v1/persons")]
public class PersonController : ControllerBase
{
    private readonly IPersonRepository _personRepository;
    
    public PersonController(IPersonRepository PersonRepository)
    {
        _personRepository = PersonRepository;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] long id)
    {
        PersonDto person = await _personRepository.GetById(id);

        return new JsonResult(person);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<PersonDto> persons = await _personRepository.GetAll();

        return new JsonResult(persons);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long id)
    {
        await _personRepository.Delete(id);

        return new JsonResult(new { result = "success" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long id, [FromBody] PersonDto person)
    {
        await _personRepository.Update(id, person);

        return new JsonResult(new { result = "success" });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PersonDto person)
    {
        await _personRepository.Create(person);

        return new JsonResult(new { result = "success" });
    }

    [HttpPut("add_skill/{id}")]
    public async Task<IActionResult> AddSkill([FromRoute] long id, [FromBody] Skill skill)
    {
        await _personRepository.AddSkillToPerson(id, skill);

        return new JsonResult(new { result = "success" });
    }

    [HttpPut("remove_skill/{id}")]
    public async Task<IActionResult> RemoveSkill([FromRoute] long id, [FromBody] string name)
    {
        await _personRepository.RemoveSkillFromPerson(id, name);

        return new JsonResult(new { result = "success" });
    }
}
