using Skills.Models;
using Models.Dto;
using Microsoft.EntityFrameworkCore;
using Skills.Exceptions;



namespace Skills.Repositories;

public class PersonRepository
{
    private SkillContext _context;

    public PersonRepository(SkillContext context)
    {
        _context = context;
    }

    public async Task Create(CreatePerson createPerson)
    {
        var person = new Person
        { 
            Name = createPerson.Name,
            DisplayName = createPerson.DisplayName
        };

        await _context.Person.AddAsync(person);

        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                await _context.SaveChangesAsync();

                foreach (var skill in createPerson.Skills)
                {
                    Skill newSkill = new Skill
                    {
                        Name = skill.Name,
                        Level = skill.Level,
                        PersonId = person.Id
                    };

                    await _context.Skill.AddAsync(newSkill);
                }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                throw;
            }
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            Person? person = _context.Person.FirstOrDefault(w => w.Id == id);

            if (person == null)
            {
                throw new NotFoundException($"Person with id not found = {id}");
            }

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<List<PersonDto>> GetAll()
    {
        try
        {
            List<PersonDto> persons = await _context.Person
                .Select(person => new PersonDto
                { 
                    Id = person.Id,
                    Name = person.Name,
                    DisplayName = person.DisplayName
                }).ToListAsync();

            return persons;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<PersonDetail> GetById(int id)
    {
        try
        {
            PersonDetail? person = await _context.Person.Where(w => w.Id == id)
                .Select(person => new PersonDetail
                { 
                    Id =  person.Id,
                    Name = person.Name,
                    DisplayName = person.DisplayName,
                    Skills = _context.Skill.Where(w => w.Person.Id == person.Id)
                        .Select(skill => new SkillDto
                        {
                            Id = skill.id,
                            Name = skill.Name,
                            Level = skill.Level
                        }).ToArray()
                }).FirstOrDefaultAsync();
            
            if (person == null)
            {
                throw new NotFoundException($"Person with id not found = {id}");
            }

            return person;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task Update(long id, UpdatePerson updatedPerson)
    {
        Person? person = null;

        try
        {
            person = _context.Person.FirstOrDefault(w => w.Id == id);

            if (person == null)
            {
                throw new NotFoundException($"Person with id not found = {id}");
            }

            person.Name = updatedPerson.Name;
            person.DisplayName = updatedPerson.DisplayName;

            _context.Person.Update(person);
        }
        catch (Exception ex)
        {
            throw;
        }
        
        

        foreach (var skill in updatedPerson.Skills)
        {
            Skill updatedSkill = new Skill
            { 
                id = skill.Id, 
                Name = skill.Name, 
                Level = skill.Level 
            };
            
            _context.Entry(updatedSkill).Property(w => w.Level).IsModified = true;
            _context.Entry(updatedSkill).Property(w => w.Name).IsModified = true;
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}