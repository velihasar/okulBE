
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
using Business.Handlers.Branches.ValidationRules;


namespace Business.Handlers.Branches.Commands
{


    public class UpdateBranchCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, IResult>
        {
            private readonly IBranchRepository _branchRepository;
            private readonly IMediator _mediator;

            public UpdateBranchCommandHandler(IBranchRepository branchRepository, IMediator mediator)
            {
                _branchRepository = branchRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateBranchValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
            {
                var isThereBranchRecord = await _branchRepository.GetAsync(u => u.Id == request.Id);


                isThereBranchRecord.TenantId = request.TenantId;
                isThereBranchRecord.Name = request.Name;
                isThereBranchRecord.Address = request.Address;
                isThereBranchRecord.Phone = request.Phone;
                isThereBranchRecord.IsActive = request.IsActive;


                _branchRepository.Update(isThereBranchRecord);
                await _branchRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

