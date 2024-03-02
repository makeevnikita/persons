using System.ComponentModel.DataAnnotations;



namespace Skills.Models;

public class Skill
{
    public string Name { get; set; }

    public byte Level { get; set; }
}
