using FluentValidation;
using Skills.Models;

namespace Skills.Validators;

public class SkillValidator : AbstractValidator<Skill>
{
    public SkillValidator()
    {
        RuleFor(w => w.Level).InclusiveBetween((byte)1, (byte)10);
    }
}