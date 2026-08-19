
using Business.Handlers.StudentParents.Commands;
using FluentValidation;

namespace Business.Handlers.StudentParents.ValidationRules
{

    public class CreateStudentParentValidator : AbstractValidator<CreateStudentParentCommand>
    {
        public CreateStudentParentValidator()
        {
            RuleFor(x => x.ParentId).NotEmpty();
            RuleFor(x => x.Relationship).NotEmpty();
            RuleFor(x => x.IsPrimary).NotEmpty();

        }
    }
    public class UpdateStudentParentValidator : AbstractValidator<UpdateStudentParentCommand>
    {
        public UpdateStudentParentValidator()
        {
            RuleFor(x => x.ParentId).NotEmpty();
            RuleFor(x => x.Relationship).NotEmpty();
            RuleFor(x => x.IsPrimary).NotEmpty();

        }
    }
}