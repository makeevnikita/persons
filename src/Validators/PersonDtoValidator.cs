using FluentValidation;
using Skills.Models.Dto;
using Skills.Models;



namespace Skills.Validators;

public class PersonDtoValidator : AbstractValidator<PersonDto>
{
    public PersonDtoValidator()
    {
        RuleForEach(w => w.Skills).SetValidator(new SkillValidator());
    }
}