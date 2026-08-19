
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

namespace Business.Handlers.TenantUsers.Queries
{

    public class GetTenantUsersQuery : IRequest<IDataResult<IEnumerable<TenantUser>>>
    {
        public class GetTenantUsersQueryHandler : IRequestHandler<GetTenantUsersQuery, IDataResult<IEnumerable<TenantUser>>>
        {
            private readonly ITenantUserRepository _tenantUserRepository;
            private readonly IMediator _mediator;

            public GetTenantUsersQueryHandler(ITenantUserRepository tenantUserRepository, IMediator mediator)
            {
                _tenantUserRepository = tenantUserRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<TenantUser>>> Handle(GetTenantUsersQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<TenantUser>>(await _tenantUserRepository.GetListAsync());
            }
        }
    }
}