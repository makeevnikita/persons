using Skills.Models;
using Skills.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Skills.Exceptions;



namespace Skills.Repositories;

public class PersonRepository : IPersonRepository
{
    private SkillContext _context;

    public PersonRepository(SkillContext context)
    {
        _context = context;
    }

    public async Task Create(PersonDto person)
    {
        // Создаёт объект Person

        var createdPerson = new Person
        {
            Name = person.Name,
            DisplayName = person.DisplayName,
            Skills = person.Skills == null ? null : person.Skills.Select(w => new Skill { Name = w.Name, Level = w.Level }).ToList()
        };

        await _context.AddAsync(createdPerson);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        // Удаляет Person

        Person? person = _context.Person.FirstOrDefault(w => w.Id == id);

        if (person == null)
        {
            throw new NotFoundException($"Person with id not found = {id}");
        }

        _context.Person.Remove(person);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PersonDto>> GetAll()
    {
        // Возвращает массив Person и Skill

        List<PersonDto> persons = await _context.Person
            .Select(person => new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                DisplayName = person.DisplayName,
                Skills = person.Skills
            }).ToListAsync();

        return persons;
    }

    public async Task<PersonDto> GetById(int id)
    {
        // Возврщает объект Person и Skill

        PersonDto? person = await _context.Person.Where(w => w.Id == id)
            .Select(person => new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                DisplayName = person.DisplayName,
                Skills = person.Skills
            }).FirstOrDefaultAsync();
        
        if (person == null)
        {
            throw new NotFoundException($"Person with id not found = {id}");
        }

        return person;
    }

    public async Task Update(long id, PersonDto person)
    {
        // Обновляет объект Person

        Person? updatedPerson = await _context.Person.FirstOrDefaultAsync(w => w.Id == id);

        if (updatedPerson == null)
        {
            throw new NotFoundException($"Person with id not found = {id}");
        }

        updatedPerson.Name = person.Name;
        updatedPerson.DisplayName = person.DisplayName;

        _context.Person.Update(updatedPerson);

        await _context.SaveChangesAsync();
    }
}