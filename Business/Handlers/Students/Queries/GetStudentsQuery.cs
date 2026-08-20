
using Business.BusinessAspects;
using Business.Handlers.Students.FilterStudent;
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
using Core.Entities.Dtos.StudentDto;

namespace Business.Handlers.Students.Queries
{
    public class GetStudentsQuery : IRequest<IDataResult<IEnumerable<StudentGetAllDto>>>
    {
        public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, IDataResult<IEnumerable<StudentGetAllDto>>>
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
            public async Task<IDataResult<IEnumerable<StudentGetAllDto>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
            {
                var list = await _studentRepository.GetListAsync(StudentFiltersHelper.GetStudentsQueryFilter(request));
                var dtos = list.Select(x => new StudentGetAllDto
                {
                    Id = x.Id,
                    PersonId = x.PersonId,
                    StudentNumber = x.StudentNumber,
                    EnrollmentDate = x.EnrollmentDate
                });
                return new SuccessDataResult<IEnumerable<StudentGetAllDto>>(dtos);
            }
        }
    }
}