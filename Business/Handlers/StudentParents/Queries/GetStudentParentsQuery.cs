
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

namespace Business.Handlers.StudentParents.Queries
{

    public class GetStudentParentsQuery : IRequest<IDataResult<IEnumerable<StudentParent>>>
    {
        public class GetStudentParentsQueryHandler : IRequestHandler<GetStudentParentsQuery, IDataResult<IEnumerable<StudentParent>>>
        {
            private readonly IStudentParentRepository _studentParentRepository;
            private readonly IMediator _mediator;

            public GetStudentParentsQueryHandler(IStudentParentRepository studentParentRepository, IMediator mediator)
            {
                _studentParentRepository = studentParentRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<StudentParent>>> Handle(GetStudentParentsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<StudentParent>>(await _studentParentRepository.GetListAsync());
            }
        }
    }
}