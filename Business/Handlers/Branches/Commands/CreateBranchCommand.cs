
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.Branches.FilterBranch;
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
using Business.Handlers.Branches.ValidationRules;
using Core.Entities.Concrete.Project;
using Core.Entities.Dtos.BranchDto;
using Core.Extensions;

namespace Business.Handlers.Branches.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateBranchCommand : IRequest<IDataResult<BranchCreateResponseDto>>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }


        public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, IDataResult<BranchCreateResponseDto>>
        {
            private readonly IBranchRepository _branchRepository;
            private readonly IMediator _mediator;
            public CreateBranchCommandHandler(IBranchRepository branchRepository, IMediator mediator)
            {
                _branchRepository = branchRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreateBranchValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<BranchCreateResponseDto>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
            {
                var tenantId = UserInfoExtensions.GetTenantIdOrZero();
                var userId = UserInfoExtensions.GetUserIdOrZero();

                var isThereBranchRecord = _branchRepository.Query().Any(BranchFiltersHelper.CreateBranchCommandFilter(request));

                if (isThereBranchRecord)
                    return new ErrorDataResult<BranchCreateResponseDto>(Messages.NameAlreadyExist);

                var addedBranch = new Branch
                {
                    TenantId = tenantId,
                    Name = request.Name,
                    Address = request.Address,
                    Phone = request.Phone,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = userId > 0 ? userId : null,
                    CreatedDate = System.DateTime.Now
                };

                _branchRepository.Add(addedBranch);
                await _branchRepository.SaveChangesAsync();

                var dto = new BranchCreateResponseDto
                {
                    Id = addedBranch.Id,
                    Name = addedBranch.Name,
                    Address = addedBranch.Address,
                    Phone = addedBranch.Phone
                };

                return new SuccessDataResult<BranchCreateResponseDto>(dto, Messages.Added);
            }
        }
    }
}