namespace Models.Dto;

public class PersonDto
{
    public long Id { get; set; }
    
    public string Name { get; set; }

    public string DisplayName { get; set; }
}

public class PersonDetail
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public SkillDto[] Skills { get; set; }
    
}

public class UpdatePerson
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public SkillDto[] Skills { get; set; }
}

public class CreatePerson 
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public List<BaseSkill> Skills { get; set; }
}