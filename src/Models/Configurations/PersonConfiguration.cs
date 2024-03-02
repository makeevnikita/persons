using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Skills.Models.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.Property(prop => prop.Skills)
            .HasConversion
            (
                value => JsonConvert.SerializeObject(
                    value, 
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                value => JsonConvert.DeserializeObject<List<Skill>>(
                    value,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            )
            .HasDefaultValue(new List<Skill> {});
    }
}