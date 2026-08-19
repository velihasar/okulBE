
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
using Business.Handlers.Teachers.ValidationRules;


namespace Business.Handlers.Teachers.Commands
{


    public class UpdateTeacherCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int PersonId { get; set; }
        public System.DateTime StartDate { get; set; }
        public bool IsActive { get; set; }

        public class UpdateTeacherCommandHandler : IRequestHandler<UpdateTeacherCommand, IResult>
        {
            private readonly ITeacherRepository _teacherRepository;
            private readonly IMediator _mediator;

            public UpdateTeacherCommandHandler(ITeacherRepository teacherRepository, IMediator mediator)
            {
                _teacherRepository = teacherRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateTeacherValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
            {
                var isThereTeacherRecord = await _teacherRepository.GetAsync(u => u.Id == request.Id);


                isThereTeacherRecord.TenantId = request.TenantId;
                isThereTeacherRecord.PersonId = request.PersonId;
                isThereTeacherRecord.StartDate = request.StartDate;
                isThereTeacherRecord.IsActive = request.IsActive;


                _teacherRepository.Update(isThereTeacherRecord);
                await _teacherRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

