
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
using Business.Handlers.TenantUsers.ValidationRules;


namespace Business.Handlers.TenantUsers.Commands
{


    public class UpdateTenantUserCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int UserId { get; set; }
        public int? BranchId { get; set; }
        public bool IsActive { get; set; }

        public class UpdateTenantUserCommandHandler : IRequestHandler<UpdateTenantUserCommand, IResult>
        {
            private readonly ITenantUserRepository _tenantUserRepository;
            private readonly IMediator _mediator;

            public UpdateTenantUserCommandHandler(ITenantUserRepository tenantUserRepository, IMediator mediator)
            {
                _tenantUserRepository = tenantUserRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateTenantUserValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateTenantUserCommand request, CancellationToken cancellationToken)
            {
                var isThereTenantUserRecord = await _tenantUserRepository.GetAsync(u => u.Id == request.Id);


                isThereTenantUserRecord.TenantId = request.TenantId;
                isThereTenantUserRecord.UserId = request.UserId;
                isThereTenantUserRecord.BranchId = request.BranchId;
                isThereTenantUserRecord.IsActive = request.IsActive;


                _tenantUserRepository.Update(isThereTenantUserRecord);
                await _tenantUserRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

