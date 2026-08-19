
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
using Business.Handlers.Parents.ValidationRules;


namespace Business.Handlers.Parents.Commands
{


    public class UpdateParentCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int PersonId { get; set; }
        public bool IsActive { get; set; }

        public class UpdateParentCommandHandler : IRequestHandler<UpdateParentCommand, IResult>
        {
            private readonly IParentRepository _parentRepository;
            private readonly IMediator _mediator;

            public UpdateParentCommandHandler(IParentRepository parentRepository, IMediator mediator)
            {
                _parentRepository = parentRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateParentValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateParentCommand request, CancellationToken cancellationToken)
            {
                var isThereParentRecord = await _parentRepository.GetAsync(u => u.Id == request.Id);


                isThereParentRecord.TenantId = request.TenantId;
                isThereParentRecord.PersonId = request.PersonId;
                isThereParentRecord.IsActive = request.IsActive;


                _parentRepository.Update(isThereParentRecord);
                await _parentRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

