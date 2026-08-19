
using Business.BusinessAspects;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete.Project;

namespace Business.Handlers.People.Queries
{

    public class GetPeopleQuery : IRequest<IDataResult<IEnumerable<Person>>>
    {
        public class GetPeopleQueryHandler : IRequestHandler<GetPeopleQuery, IDataResult<IEnumerable<Person>>>
        {
            private readonly IPersonRepository _personRepository;
            private readonly IMediator _mediator;

            public GetPeopleQueryHandler(IPersonRepository personRepository, IMediator mediator)
            {
                _personRepository = personRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Person>>> Handle(GetPeopleQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Person>>(await _personRepository.GetListAsync());
            }
        }
    }
}