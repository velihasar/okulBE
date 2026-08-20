
using Business.Handlers.People.Commands;
using FluentValidation;

namespace Business.Handlers.People.ValidationRules
{

    public class CreatePersonValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
    public class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}