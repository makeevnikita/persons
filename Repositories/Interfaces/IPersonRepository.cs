using Skills.Models.Dto;

namespace Skills.Repositories;

public interface IPersonRepository
{
    public Task Create(PersonDto person);

    public Task Delete(int id);

    public Task<List<PersonDto>> GetAll();

    public Task <PersonDto> GetById(int id);

    public Task Update(long id, PersonDto person);
}