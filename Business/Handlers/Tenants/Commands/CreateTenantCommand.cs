
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
using Business.Handlers.Tenants.ValidationRules;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.Tenants.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTenantCommand : IRequest<IResult>
    {

        public string Name { get; set; }
        public string Code { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }


        public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, IResult>
        {
            private readonly ITenantRepository _tenantRepository;
            private readonly IMediator _mediator;
            public CreateTenantCommandHandler(ITenantRepository tenantRepository, IMediator mediator)
            {
                _tenantRepository = tenantRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateTenantValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
            {
                var isThereTenantRecord = _tenantRepository.Query().Any(u => u.Name == request.Name);

                if (isThereTenantRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedTenant = new Tenant
                {
                    Name = request.Name,
                    Code = request.Code,
                    LogoUrl = request.LogoUrl,
                    IsActive = request.IsActive,

                };

                _tenantRepository.Add(addedTenant);
                await _tenantRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}