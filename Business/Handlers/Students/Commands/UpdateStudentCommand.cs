
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Students.ValidationRules;


namespace Business.Handlers.Students.Commands
{


    public class UpdateStudentCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int PersonId { get; set; }
        public string StudentNumber { get; set; }
        public System.DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; }

        public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, IResult>
        {
            private readonly IStudentRepository _studentRepository;
            private readonly IMediator _mediator;

            public UpdateStudentCommandHandler(IStudentRepository studentRepository, IMediator mediator)
            {
                _studentRepository = studentRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateStudentValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
            {
                var isThereStudentRecord = await _studentRepository.GetAsync(u => u.Id == request.Id);


                isThereStudentRecord.TenantId = request.TenantId;
                isThereStudentRecord.PersonId = request.PersonId;
                isThereStudentRecord.StudentNumber = request.StudentNumber;
                isThereStudentRecord.EnrollmentDate = request.EnrollmentDate;
                isThereStudentRecord.IsActive = request.IsActive;


                _studentRepository.Update(isThereStudentRecord);
                await _studentRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

