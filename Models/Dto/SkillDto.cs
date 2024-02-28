using System.ComponentModel.DataAnnotations;



namespace Models.Dto;

public class BaseSkill
{
    [Range(0, 10)]
    public byte Level { get; set; }

    public string Name { get; set; }
}

public class SkillDto : BaseSkill
{
    public long Id { get; set; }
}
