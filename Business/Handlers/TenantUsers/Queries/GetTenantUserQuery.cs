
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


namespace Business.Handlers.TenantUsers.Queries
{
    public class GetTenantUserQuery : IRequest<IDataResult<TenantUser>>
    {
        public int Id { get; set; }

        public class GetTenantUserQueryHandler : IRequestHandler<GetTenantUserQuery, IDataResult<TenantUser>>
        {
            private readonly ITenantUserRepository _tenantUserRepository;
            private readonly IMediator _mediator;

            public GetTenantUserQueryHandler(ITenantUserRepository tenantUserRepository, IMediator mediator)
            {
                _tenantUserRepository = tenantUserRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<TenantUser>> Handle(GetTenantUserQuery request, CancellationToken cancellationToken)
            {
                var tenantUser = await _tenantUserRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<TenantUser>(tenantUser);
            }
        }
    }
}
