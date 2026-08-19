
using Business.BusinessAspects;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Business.Handlers.Students.ValidationRules;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.Students.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateStudentCommand : IRequest<IResult>
    {

        public int TenantId { get; set; }
        public int PersonId { get; set; }
        public string StudentNumber { get; set; }
        public System.DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; }


        public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, IResult>
        {
            private readonly IStudentRepository _studentRepository;
            private readonly IMediator _mediator;
            public CreateStudentCommandHandler(IStudentRepository studentRepository, IMediator mediator)
            {
                _studentRepository = studentRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateStudentValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
            {
                var isThereStudentRecord = _studentRepository.Query().Any(u => u.TenantId == request.TenantId);

                if (isThereStudentRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedStudent = new Student
                {
                    TenantId = request.TenantId,
                    PersonId = request.PersonId,
                    StudentNumber = request.StudentNumber,
                    EnrollmentDate = request.EnrollmentDate,
                    IsActive = request.IsActive,

                };

                _studentRepository.Add(addedStudent);
                await _studentRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}