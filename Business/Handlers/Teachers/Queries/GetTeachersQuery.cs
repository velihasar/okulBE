
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

namespace Business.Handlers.Teachers.Queries
{

    public class GetTeachersQuery : IRequest<IDataResult<IEnumerable<Teacher>>>
    {
        public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, IDataResult<IEnumerable<Teacher>>>
        {
            private readonly ITeacherRepository _teacherRepository;
            private readonly IMediator _mediator;

            public GetTeachersQueryHandler(ITeacherRepository teacherRepository, IMediator mediator)
            {
                _teacherRepository = teacherRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Teacher>>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Teacher>>(await _teacherRepository.GetListAsync());
            }
        }
    }
}