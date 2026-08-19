
using Business.Handlers.Students.Commands;
using FluentValidation;

namespace Business.Handlers.Students.ValidationRules
{

    public class CreateStudentValidator : AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.PersonId).NotEmpty();
            RuleFor(x => x.StudentNumber).NotEmpty();
            RuleFor(x => x.EnrollmentDate).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();

        }
    }
    public class UpdateStudentValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.PersonId).NotEmpty();
            RuleFor(x => x.StudentNumber).NotEmpty();
            RuleFor(x => x.EnrollmentDate).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();

        }
    }
}