
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
using Business.Handlers.StudentParents.ValidationRules;


namespace Business.Handlers.StudentParents.Commands
{


    public class UpdateStudentParentCommand : IRequest<IResult>
    {
        public int StudentId { get; set; }
        public int ParentId { get; set; }
        public string Relationship { get; set; }
        public bool IsPrimary { get; set; }

        public class UpdateStudentParentCommandHandler : IRequestHandler<UpdateStudentParentCommand, IResult>
        {
            private readonly IStudentParentRepository _studentParentRepository;
            private readonly IMediator _mediator;

            public UpdateStudentParentCommandHandler(IStudentParentRepository studentParentRepository, IMediator mediator)
            {
                _studentParentRepository = studentParentRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateStudentParentValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateStudentParentCommand request, CancellationToken cancellationToken)
            {
                var isThereStudentParentRecord = await _studentParentRepository.GetAsync(u => u.StudentId == request.StudentId);


                isThereStudentParentRecord.ParentId = request.ParentId;
                isThereStudentParentRecord.Relationship = request.Relationship;
                isThereStudentParentRecord.IsPrimary = request.IsPrimary;


                _studentParentRepository.Update(isThereStudentParentRecord);
                await _studentParentRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

