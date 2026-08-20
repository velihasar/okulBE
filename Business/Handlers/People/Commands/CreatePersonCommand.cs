
using Business.BusinessAspects;
using Business.Constants;
using Business.Handlers.People.FilterPerson;
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
using Core.Entities.Dtos.PersonDto;
using Core.Extensions;

namespace Business.Handlers.People.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CreatePersonCommand : IRequest<IDataResult<PersonCreateResponseDto>>
    {

        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }


        public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, IDataResult<PersonCreateResponseDto>>
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
            public async Task<IDataResult<PersonCreateResponseDto>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
            {
                var tenantId = UserInfoExtensions.GetTenantIdOrZero();
                var userId = UserInfoExtensions.GetUserIdOrZero();

                var isTherePersonRecord = _personRepository.Query().Any(PersonFiltersHelper.CreatePersonCommandFilter(request));

                if (isTherePersonRecord)
                    return new ErrorDataResult<PersonCreateResponseDto>(Messages.NameAlreadyExist);

                var addedPerson = new Person
                {
                    TenantId = tenantId,
                    UserId = request.UserId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    Phone = request.Phone,
                    Email = request.Email,
                    PhotoUrl = request.PhotoUrl,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = userId > 0 ? userId : null,
                    CreatedDate = System.DateTime.Now
                };

                _personRepository.Add(addedPerson);
                await _personRepository.SaveChangesAsync();

                var dto = new PersonCreateResponseDto
                {
                    Id = addedPerson.Id,
                    UserId = addedPerson.UserId,
                    FirstName = addedPerson.FirstName,
                    LastName = addedPerson.LastName,
                    DateOfBirth = addedPerson.DateOfBirth,
                    Phone = addedPerson.Phone,
                    Email = addedPerson.Email,
                    PhotoUrl = addedPerson.PhotoUrl
                };

                return new SuccessDataResult<PersonCreateResponseDto>(dto, Messages.Added);
            }
        }
    }
}