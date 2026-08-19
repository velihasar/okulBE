
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
using Business.Handlers.TenantUsers.ValidationRules;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.TenantUsers.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTenantUserCommand : IRequest<IResult>
    {

        public int TenantId { get; set; }
        public int UserId { get; set; }
        public int? BranchId { get; set; }
        public bool IsActive { get; set; }


        public class CreateTenantUserCommandHandler : IRequestHandler<CreateTenantUserCommand, IResult>
        {
            private readonly ITenantUserRepository _tenantUserRepository;
            private readonly IMediator _mediator;
            public CreateTenantUserCommandHandler(ITenantUserRepository tenantUserRepository, IMediator mediator)
            {
                _tenantUserRepository = tenantUserRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateTenantUserValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreateTenantUserCommand request, CancellationToken cancellationToken)
            {
                var isThereTenantUserRecord = _tenantUserRepository.Query().Any(u => u.TenantId == request.TenantId);

                if (isThereTenantUserRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedTenantUser = new TenantUser
                {
                    TenantId = request.TenantId,
                    UserId = request.UserId,
                    BranchId = request.BranchId,
                    IsActive = request.IsActive,

                };

                _tenantUserRepository.Add(addedTenantUser);
                await _tenantUserRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}