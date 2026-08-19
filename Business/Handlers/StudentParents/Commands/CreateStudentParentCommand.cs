
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
using Business.Handlers.StudentParents.ValidationRules;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.StudentParents.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateStudentParentCommand : IRequest<IResult>
    {

        public int ParentId { get; set; }
        public string Relationship { get; set; }
        public bool IsPrimary { get; set; }


        public class CreateStudentParentCommandHandler : IRequestHandler<CreateStudentParentCommand, IResult>
        {
            private readonly IStudentParentRepository _studentParentRepository;
            private readonly IMediator _mediator;
            public CreateStudentParentCommandHandler(IStudentParentRepository studentParentRepository, IMediator mediator)
            {
                _studentParentRepository = studentParentRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateStudentParentValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateStudentParentCommand request, CancellationToken cancellationToken)
            {
                var isThereStudentParentRecord = _studentParentRepository.Query().Any(u => u.ParentId == request.ParentId);

                if (isThereStudentParentRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedStudentParent = new StudentParent
                {
                    ParentId = request.ParentId,
                    Relationship = request.Relationship,
                    IsPrimary = request.IsPrimary,

                };

                _studentParentRepository.Add(addedStudentParent);
                await _studentParentRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}