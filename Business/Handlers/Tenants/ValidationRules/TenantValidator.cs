
using Business.Handlers.Tenants.Commands;
using FluentValidation;

namespace Business.Handlers.Tenants.ValidationRules
{

    public class CreateTenantValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.LogoUrl).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();

        }
    }
    public class UpdateTenantValidator : AbstractValidator<UpdateTenantCommand>
    {
        public UpdateTenantValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.LogoUrl).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();

        }
    }
}