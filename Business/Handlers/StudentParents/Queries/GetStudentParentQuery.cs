
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


namespace Business.Handlers.StudentParents.Queries
{
    public class GetStudentParentQuery : IRequest<IDataResult<StudentParent>>
    {
        public int StudentId { get; set; }

        public class GetStudentParentQueryHandler : IRequestHandler<GetStudentParentQuery, IDataResult<StudentParent>>
        {
            private readonly IStudentParentRepository _studentParentRepository;
            private readonly IMediator _mediator;

            public GetStudentParentQueryHandler(IStudentParentRepository studentParentRepository, IMediator mediator)
            {
                _studentParentRepository = studentParentRepository;
                _mediator = mediator;
            }
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IDataResult<StudentParent>> Handle(GetStudentParentQuery request, CancellationToken cancellationToken)
            {
                var studentParent = await _studentParentRepository.GetAsync(p => p.StudentId == request.StudentId);
                return new SuccessDataResult<StudentParent>(studentParent);
            }
        }
    }
}
