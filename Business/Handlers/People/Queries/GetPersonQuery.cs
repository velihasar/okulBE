
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Concrete.Project;


namespace Business.Handlers.People.Queries
{
    public class GetPersonQuery : IRequest<IDataResult<Person>>
    {
        public int Id { get; set; }

        public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, IDataResult<Person>>
        {
            private readonly IPersonRepository _personRepository;
            private readonly IMediator _mediator;

            public GetPersonQueryHandler(IPersonRepository personRepository, IMediator mediator)
            {
                _personRepository = personRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Person>> Handle(GetPersonQuery request, CancellationToken cancellationToken)
            {
                var person = await _personRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Person>(person);
            }
        }
    }
}
