
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


namespace Business.Handlers.Teachers.Queries
{
    public class GetTeacherQuery : IRequest<IDataResult<Teacher>>
    {
        public int Id { get; set; }

        public class GetTeacherQueryHandler : IRequestHandler<GetTeacherQuery, IDataResult<Teacher>>
        {
            private readonly ITeacherRepository _teacherRepository;
            private readonly IMediator _mediator;

            public GetTeacherQueryHandler(ITeacherRepository teacherRepository, IMediator mediator)
            {
                _teacherRepository = teacherRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Teacher>> Handle(GetTeacherQuery request, CancellationToken cancellationToken)
            {
                var teacher = await _teacherRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Teacher>(teacher);
            }
        }
    }
}
