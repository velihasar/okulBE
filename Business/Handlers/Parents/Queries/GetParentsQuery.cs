
using Business.BusinessAspects;
using Business.Handlers.Parents.FilterParent;
using Core.Aspects.Autofac.Performance;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Dtos.ParentDto;

namespace Business.Handlers.Parents.Queries
{
    public class GetParentsQuery : IRequest<IDataResult<IEnumerable<ParentGetAllDto>>>
    {
        public class GetParentsQueryHandler : IRequestHandler<GetParentsQuery, IDataResult<IEnumerable<ParentGetAllDto>>>
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
            public async Task<IDataResult<IEnumerable<ParentGetAllDto>>> Handle(GetParentsQuery request, CancellationToken cancellationToken)
            {
                var list = await _parentRepository.GetListAsync(ParentFiltersHelper.GetParentsQueryFilter(request));
                var dtos = list.Select(x => new ParentGetAllDto
                {
                    Id = x.Id,
                    PersonId = x.PersonId
                });
                return new SuccessDataResult<IEnumerable<ParentGetAllDto>>(dtos);
            }
        }
    }
}