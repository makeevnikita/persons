using System.ComponentModel.DataAnnotations;



namespace Skills.Models;

public class Skill
{
    public string Name { get; set; }

    [Range(1, 10)]
    public byte Level { get; set; }
}
