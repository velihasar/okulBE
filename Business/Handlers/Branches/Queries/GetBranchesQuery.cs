
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.Branches.Queries
{

    public class GetBranchesQuery : IRequest<IDataResult<IEnumerable<Branch>>>
    {
        public class GetBranchesQueryHandler : IRequestHandler<GetBranchesQuery, IDataResult<IEnumerable<Branch>>>
        {
            private readonly IBranchRepository _branchRepository;
            private readonly IMediator _mediator;

            public GetBranchesQueryHandler(IBranchRepository branchRepository, IMediator mediator)
            {
                _branchRepository = branchRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Branch>>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Branch>>(await _branchRepository.GetListAsync());
            }
        }
    }
}