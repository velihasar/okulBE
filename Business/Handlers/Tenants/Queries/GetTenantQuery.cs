
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


namespace Business.Handlers.Tenants.Queries
{
    public class GetTenantQuery : IRequest<IDataResult<Tenant>>
    {
        public int Id { get; set; }

        public class GetTenantQueryHandler : IRequestHandler<GetTenantQuery, IDataResult<Tenant>>
        {
            private readonly ITenantRepository _tenantRepository;
            private readonly IMediator _mediator;

            public GetTenantQueryHandler(ITenantRepository tenantRepository, IMediator mediator)
            {
                _tenantRepository = tenantRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Tenant>> Handle(GetTenantQuery request, CancellationToken cancellationToken)
            {
                var tenant = await _tenantRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Tenant>(tenant);
            }
        }
    }
}
