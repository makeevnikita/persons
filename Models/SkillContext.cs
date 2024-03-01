using Skills.Models.Configurations;
using Microsoft.EntityFrameworkCore;



namespace Skills.Models;

public class SkillContext : DbContext
{
    public DbSet<Person> Person { get; set; }

    public SkillContext(DbContextOptions<SkillContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
    }
}
