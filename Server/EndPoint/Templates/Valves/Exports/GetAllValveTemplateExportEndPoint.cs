using Shared.Models.Templates.Valves.Exports;
using Shared.Models.Templates.Valves.Responses;

namespace Server.EndPoint.Templates.Valves.Exports
{
    //public static class GetAllValveTemplateExportEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.ValveTemplates.EndPoint.Export, async (ValveTemplateGetAllExport request, IMediator Mediator) =>
    //            {
    //                return await Mediator.Send(new Query(request));
    //            });
    //        }
    //    }
    //    record Query(ValveTemplateGetAllExport Data) : IRequest<IResult<FileResult>>
    //    {

    //    }
    //    class QueryHandler : IRequestHandler<Query, IResult<FileResult>>
    //    {
    //        private IQueryRepository Repository;
    //        private IAppDbContext _cache;

    //        public QueryHandler(IAppDbContext cache, IQueryRepository repository)
    //        {
    //            _cache = cache;
    //            Repository = repository;
    //        }

    //        public async Task<IResult<FileResult>> Handle(Query request, CancellationToken cancellationToken)
    //        {
    //            await Task.Delay(1);
    //            var maps = request.Data.query.Select(x => x.MapToResponseExcel()).AsQueryable();
    //            var response = request.Data.FileType == ExportFileType.pdf ?
    //                ExportPDFExtension.ExportPDF(maps, "ValveTemplates", "ValveTemplates List") :
    //                ExportExcelExtension.ToExcel(maps, "ValveTemplates", "List");


    //            return Result<FileResult>.Success(response);
    //        }
    //    }
    //    static ValveTemplateExportResponse MapToResponseExcel(this ValveTemplateResponse row)
    //    {
    //        return new()
    //        {
    //            Name = row.Name,

    //        };
    //    }


    //}
}
