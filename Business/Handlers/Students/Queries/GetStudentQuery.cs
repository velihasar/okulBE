
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


namespace Business.Handlers.Students.Queries
{
    public class GetStudentQuery : IRequest<IDataResult<Student>>
    {
        public int Id { get; set; }

        public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, IDataResult<Student>>
        {
            private readonly IStudentRepository _studentRepository;
            private readonly IMediator _mediator;

            public GetStudentQueryHandler(IStudentRepository studentRepository, IMediator mediator)
            {
                _studentRepository = studentRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<Student>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
            {
                var student = await _studentRepository.GetAsync(p => p.Id == request.Id);
                return new SuccessDataResult<Student>(student);
            }
        }
    }
}
