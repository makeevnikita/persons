using Skills.Models;
using Skills.Models.Dto;

namespace Skills.Repositories;

public interface IPersonRepository
{
    public Task Create(PersonDto person);

    public Task Delete(long id);

    public Task<List<PersonDto>> GetAll();

    public Task <PersonDto> GetById(long id);

    public Task Update(long id, PersonDto person);
}