
using Business.BusinessAspects;
using Business.Handlers.Teachers.FilterTeacher;
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
using Core.Entities.Dtos.TeacherDto;

namespace Business.Handlers.Teachers.Queries
{
    public class GetTeachersQuery : IRequest<IDataResult<IEnumerable<TeacherGetAllDto>>>
    {
        public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, IDataResult<IEnumerable<TeacherGetAllDto>>>
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
            public async Task<IDataResult<IEnumerable<TeacherGetAllDto>>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
            {
                var list = await _teacherRepository.GetListAsync(TeacherFiltersHelper.GetTeachersQueryFilter(request));
                var dtos = list.Select(x => new TeacherGetAllDto
                {
                    Id = x.Id,
                    PersonId = x.PersonId,
                    StartDate = x.StartDate
                });
                return new SuccessDataResult<IEnumerable<TeacherGetAllDto>>(dtos);
            }
        }
    }
}