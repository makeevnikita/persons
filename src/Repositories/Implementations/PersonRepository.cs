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

    /// <summary>
    /// Создаёт Person вместе с объектами Skill, если они указаны
    /// </summary>
    /// <param name="person"></param>
    /// <returns></returns>
    public async Task Create(PersonDto person)
    {
        var createdPerson = new Person
        {
            Name = person.Name,
            DisplayName = person.DisplayName,
            Skills = person.Skills == null ? null : person.Skills.Select(w => new Skill { Name = w.Name, Level = w.Level }).ToList()
        };

        await _context.AddAsync(createdPerson);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Удаляет Person
    /// Если Person с указанным id не найден, то возвращает ответ NotFoundException
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task Delete(long id)
    {
        Person? person = _context.Person.FirstOrDefault(w => w.Id == id);

        if (person == null)
        {
            throw new NotFoundException($"Person with id not found = {id}");
        }

        _context.Person.Remove(person);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Возвращает массив Person
    /// </summary>
    /// <returns></returns>
    public async Task<List<PersonDto>> GetAll()
    {
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

    /// <summary>
    /// Возвращает Person
    /// Если Person с указанным id не найден, то возвращает ответ NotFoundException
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<PersonDto> GetById(long id)
    {
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

    /// <summary>
    /// Обновляет Name и DisplayName у Person
    /// Если Person с указанным id не найден, то возвращает NotFoundException
    /// Добавляет скилл, если он отсутствует в БД, но есть в теле запроса
    /// Удаляет скилл, если он присутствует в БД, но отсутствует в теле запроса
    /// Изменяет уровень скила, если он присутствует в БД и в теле запроса
    /// </summary>
    /// <param name="id"></param>
    /// <param name="person"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task Update(long id, PersonDto person)
    {
        Person? updatedPerson = await _context.Person.FirstOrDefaultAsync(w => w.Id == id);

        if (updatedPerson == null)
        {
            throw new NotFoundException($"Person with id not found = {id}");
        }

        updatedPerson.Name = person.Name;
        updatedPerson.DisplayName = person.DisplayName;
        
        foreach (var skill in person.Skills)
        {
            var updatedSkill = updatedPerson.Skills.FirstOrDefault(w => w.Name == skill.Name);

            if (updatedSkill == null)
            {
                updatedPerson.Skills.Add(skill);
            }
            else if (updatedSkill.Level != skill.Level)
            {
                updatedSkill.Level = skill.Level;
            }
        }

        updatedPerson.Skills = updatedPerson.Skills.IntersectBy(
            person.Skills.Select(p => p.Name), u => u.Name).ToList();

        _context.Person.Update(updatedPerson);

        await _context.SaveChangesAsync();
    }
}
