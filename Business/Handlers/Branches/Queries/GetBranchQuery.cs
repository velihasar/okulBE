
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


namespace Business.Handlers.Branches.Queries
{
    public class GetBranchQuery : IRequest<IDataResult<Branch>>
    {
        public int Id { get; set; }

        public class GetBranchQueryHandler : IRequestHandler<GetBranchQuery, IDataResult<Branch>>
        {
            private readonly IBranchRepository _branchRepository;
            private readonly IMediator _mediator;

            public GetBranchQueryHandler(IBranchRepository branchRepository, IMediator mediator)
            {
                _branchRepository = branchRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Branch>> Handle(GetBranchQuery request, CancellationToken cancellationToken)
            {
                var branch = await _branchRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Branch>(branch);
            }
        }
    }
}
