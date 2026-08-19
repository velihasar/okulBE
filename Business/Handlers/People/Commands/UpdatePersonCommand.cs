
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
using Business.Handlers.People.ValidationRules;


namespace Business.Handlers.People.Commands
{


    public class UpdatePersonCommand : IRequest<IResult>
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsActive { get; set; }

        public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, IResult>
        {
            private readonly IPersonRepository _personRepository;
            private readonly IMediator _mediator;

            public UpdatePersonCommandHandler(IPersonRepository personRepository, IMediator mediator)
            {
                _personRepository = personRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdatePersonValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
            {
                var isTherePersonRecord = await _personRepository.GetAsync(u => u.Id == request.Id);


                isTherePersonRecord.TenantId = request.TenantId;
                isTherePersonRecord.UserId = request.UserId;
                isTherePersonRecord.FirstName = request.FirstName;
                isTherePersonRecord.LastName = request.LastName;
                isTherePersonRecord.DateOfBirth = request.DateOfBirth;
                isTherePersonRecord.Phone = request.Phone;
                isTherePersonRecord.Email = request.Email;
                isTherePersonRecord.PhotoUrl = request.PhotoUrl;
                isTherePersonRecord.IsActive = request.IsActive;


                _personRepository.Update(isTherePersonRecord);
                await _personRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

