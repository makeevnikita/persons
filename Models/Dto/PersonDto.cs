namespace Skills.Models.Dto;

public class PersonDto
{
    public long? Id { get; set; }
    
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public List<Skill>? Skills { get; set; }
}
