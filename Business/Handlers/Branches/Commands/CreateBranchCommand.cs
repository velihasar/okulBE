
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
using Business.Handlers.Branches.ValidationRules;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.Branches.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateBranchCommand : IRequest<IResult>
    {

        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }


        public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, IResult>
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
            public async Task<IResult> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
            {
                var isThereBranchRecord = _branchRepository.Query().Any(u => u.TenantId == request.TenantId);

                if (isThereBranchRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedBranch = new Branch
                {
                    TenantId = request.TenantId,
                    Name = request.Name,
                    Address = request.Address,
                    Phone = request.Phone,
                    IsActive = request.IsActive,

                };

                _branchRepository.Add(addedBranch);
                await _branchRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}