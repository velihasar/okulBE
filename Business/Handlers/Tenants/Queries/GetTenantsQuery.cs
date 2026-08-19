
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

namespace Business.Handlers.Tenants.Queries
{

    public class GetTenantsQuery : IRequest<IDataResult<IEnumerable<Tenant>>>
    {
        public class GetTenantsQueryHandler : IRequestHandler<GetTenantsQuery, IDataResult<IEnumerable<Tenant>>>
        {
            private readonly ITenantRepository _tenantRepository;
            private readonly IMediator _mediator;

            public GetTenantsQueryHandler(ITenantRepository tenantRepository, IMediator mediator)
            {
                _tenantRepository = tenantRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Tenant>>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Tenant>>(await _tenantRepository.GetListAsync());
            }
        }
    }
}