
using Business.Handlers.People.Commands;
using FluentValidation;

namespace Business.Handlers.People.ValidationRules
{

    public class CreatePersonValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.PhotoUrl).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();

        }
    }
    public class UpdatePersonValidator : AbstractValidator<UpdatePersonCommand>
    {
        public UpdatePersonValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.PhotoUrl).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();

        }
    }
}