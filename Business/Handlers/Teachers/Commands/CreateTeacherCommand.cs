
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
using Business.Handlers.Teachers.ValidationRules;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.Teachers.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTeacherCommand : IRequest<IResult>
    {

        public int TenantId { get; set; }
        public int PersonId { get; set; }
        public System.DateTime StartDate { get; set; }
        public bool IsActive { get; set; }


        public class CreateTeacherCommandHandler : IRequestHandler<CreateTeacherCommand, IResult>
        {
            private readonly ITeacherRepository _teacherRepository;
            private readonly IMediator _mediator;
            public CreateTeacherCommandHandler(ITeacherRepository teacherRepository, IMediator mediator)
            {
                _teacherRepository = teacherRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateTeacherValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
            {
                var isThereTeacherRecord = _teacherRepository.Query().Any(u => u.TenantId == request.TenantId);

                if (isThereTeacherRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedTeacher = new Teacher
                {
                    TenantId = request.TenantId,
                    PersonId = request.PersonId,
                    StartDate = request.StartDate,
                    IsActive = request.IsActive,

                };

                _teacherRepository.Add(addedTeacher);
                await _teacherRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}