using Microsoft.EntityFrameworkCore;

namespace Skills.Models;

public class Person
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public List<Skill> Skills { get; set; }
}

public class Skill
{
    public long id { get; set; }

    public string Name { get; set; }

    public byte Level { get; set; }

    public Person Person { get; set; }

    public long PersonId { get; set; }
}

public class SkillContext : DbContext
{
    public DbSet<Person> Person { get; set; }

    public DbSet<Skill> Skill { get; set; }

    public SkillContext(DbContextOptions<SkillContext> options) : base(options)
    {

    }
}
