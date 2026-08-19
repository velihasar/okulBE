
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete.Project;


namespace Business.Handlers.Parents.Queries
{
    public class GetParentQuery : IRequest<IDataResult<Parent>>
    {
        public int Id { get; set; }

        public class GetParentQueryHandler : IRequestHandler<GetParentQuery, IDataResult<Parent>>
        {
            private readonly IParentRepository _parentRepository;
            private readonly IMediator _mediator;

            public GetParentQueryHandler(IParentRepository parentRepository, IMediator mediator)
            {
                _parentRepository = parentRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Parent>> Handle(GetParentQuery request, CancellationToken cancellationToken)
            {
                var parent = await _parentRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Parent>(parent);
            }
        }
    }
}
