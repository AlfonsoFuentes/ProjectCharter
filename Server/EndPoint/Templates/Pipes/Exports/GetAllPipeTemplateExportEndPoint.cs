using Shared.Models.Templates.Pipings.Exports;
using Shared.Models.Templates.Pipings.Responses;

namespace Server.EndPoint.Templates.Pipes.Exports
{
//    public static class GetAllPipeTemplateExportEndPoint
//    {
//        public class EndPoint : IEndPoint
//        {
//            public void MapEndPoint(IEndpointRouteBuilder app)
//            {
//                app.MapPost(StaticClass.PipeTemplates.EndPoint.Export, async (PipeTemplateGetAllExport request, IMediator Mediator) =>
//                {
//                    return await Mediator.Send(new Query(request));
//                });
//            }
//        }
//        record Query(PipeTemplateGetAllExport Data) : IRequest<IResult<FileResult>>
//        {

//        }
//        class QueryHandler : IRequestHandler<Query, IResult<FileResult>>
//        {
//            private IQueryRepository Repository;
//            private IAppDbContext _cache;

//            public QueryHandler(IAppDbContext cache, IQueryRepository repository)
//            {
//                _cache = cache;
//                Repository = repository;
//            }

//            public async Task<IResult<FileResult>> Handle(Query request, CancellationToken cancellationToken)
//            {
//                await Task.Delay(1);
//                var maps = request.Data.query.Select(x => x.MapToResponseExcel()).AsQueryable();
//                var response = request.Data.FileType == ExportFileType.pdf ?
//                    ExportPDFExtension.ExportPDF(maps, "PipeTemplates", "PipeTemplates List") :
//                    ExportExcelExtension.ToExcel(maps, "PipeTemplates", "List");


//                return Result<FileResult>.Success(response);
//            }
//        }
//        static PipeTemplateExportResponse MapToResponseExcel(this PipeTemplateResponse row)
//        {
//            return new()
//            {
//                Name = row.Name,

//            };
//        }


//    }
}
