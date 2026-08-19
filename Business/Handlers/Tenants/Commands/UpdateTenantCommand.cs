
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
using Business.Handlers.Tenants.ValidationRules;


namespace Business.Handlers.Tenants.Commands
{


    public class UpdateTenantCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }

        public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, IResult>
        {
            private readonly ITenantRepository _tenantRepository;
            private readonly IMediator _mediator;

            public UpdateTenantCommandHandler(ITenantRepository tenantRepository, IMediator mediator)
            {
                _tenantRepository = tenantRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateTenantValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
            {
                var isThereTenantRecord = await _tenantRepository.GetAsync(u => u.Id == request.Id);


                isThereTenantRecord.Name = request.Name;
                isThereTenantRecord.Code = request.Code;
                isThereTenantRecord.LogoUrl = request.LogoUrl;
                isThereTenantRecord.IsActive = request.IsActive;


                _tenantRepository.Update(isThereTenantRecord);
                await _tenantRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

