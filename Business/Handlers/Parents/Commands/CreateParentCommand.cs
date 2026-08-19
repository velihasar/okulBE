
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
using Business.Handlers.Parents.ValidationRules;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.Parents.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateParentCommand : IRequest<IResult>
    {

        public int TenantId { get; set; }
        public int PersonId { get; set; }
        public bool IsActive { get; set; }


        public class CreateParentCommandHandler : IRequestHandler<CreateParentCommand, IResult>
        {
            private readonly IParentRepository _parentRepository;
            private readonly IMediator _mediator;
            public CreateParentCommandHandler(IParentRepository parentRepository, IMediator mediator)
            {
                _parentRepository = parentRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateParentValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateParentCommand request, CancellationToken cancellationToken)
            {
                var isThereParentRecord = _parentRepository.Query().Any(u => u.TenantId == request.TenantId);

                if (isThereParentRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedParent = new Parent
                {
                    TenantId = request.TenantId,
                    PersonId = request.PersonId,
                    IsActive = request.IsActive,

                };

                _parentRepository.Add(addedParent);
                await _parentRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}