
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

namespace Business.Handlers.Students.Queries
{

    public class GetStudentsQuery : IRequest<IDataResult<IEnumerable<Student>>>
    {
        public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, IDataResult<IEnumerable<Student>>>
        {
            private readonly IStudentRepository _studentRepository;
            private readonly IMediator _mediator;

            public GetStudentsQueryHandler(IStudentRepository studentRepository, IMediator mediator)
            {
                _studentRepository = studentRepository;
                _mediator = mediator;
            }

            [PerformanceAspect(5)]
            [CacheAspect(10)]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<IEnumerable<Student>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
            {
                return new SuccessDataResult<IEnumerable<Student>>(await _studentRepository.GetListAsync());
            }
        }
    }
}