
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

namespace Business.Handlers.Parents.Queries
{

    public class GetParentsQuery : IRequest<IDataResult<IEnumerable<Parent>>>
    {
        public class GetParentsQueryHandler : IRequestHandler<GetParentsQuery, IDataResult<IEnumerable<Parent>>>
        {
            private readonly IParentRepository _parentRepository;
            private readonly IMediator _mediator;

            public GetParentsQueryHandler(IParentRepository parentRepository, IMediator mediator)
            {
                _parentRepository = parentRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Parent>>> Handle(GetParentsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Parent>>(await _parentRepository.GetListAsync());
            }
        }
    }
}