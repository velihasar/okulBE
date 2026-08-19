
using Business.Handlers.TenantUsers.Commands;
using FluentValidation;

namespace Business.Handlers.TenantUsers.ValidationRules
{

    public class CreateTenantUserValidator : AbstractValidator<CreateTenantUserCommand>
    {
        public CreateTenantUserValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();

        }
    }
    public class UpdateTenantUserValidator : AbstractValidator<UpdateTenantUserCommand>
    {
        public UpdateTenantUserValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();

        }
    }
}