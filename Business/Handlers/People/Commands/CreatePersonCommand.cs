
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
using Business.Handlers.People.ValidationRules;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.People.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreatePersonCommand : IRequest<IResult>
    {

        public int TenantId { get; set; }
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsActive { get; set; }


        public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, IResult>
        {
            private readonly IPersonRepository _personRepository;
            private readonly IMediator _mediator;
            public CreatePersonCommandHandler(IPersonRepository personRepository, IMediator mediator)
            {
                _personRepository = personRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(CreatePersonValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
            {
                var isTherePersonRecord = _personRepository.Query().Any(u => u.TenantId == request.TenantId);

                if (isTherePersonRecord == true)
                    return new ErrorResult(Messages.NameAlreadyExist);

                var addedPerson = new Person
                {
                    TenantId = request.TenantId,
                    UserId = request.UserId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    Phone = request.Phone,
                    Email = request.Email,
                    PhotoUrl = request.PhotoUrl,
                    IsActive = request.IsActive,

                };

                _personRepository.Add(addedPerson);
                await _personRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Added);
            }
        }
    }
}