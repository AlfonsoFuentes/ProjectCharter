using Shared.Models.Templates.Instruments.Exports;
using Shared.Models.Templates.Instruments.Responses;

namespace Server.EndPoint.Templates.Instruments.Exports
{
    //public static class GetAllInstrumentTemplateExportEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.InstrumentTemplates.EndPoint.Export, async (InstrumentTemplateGetAllExport request, IMediator Mediator) =>
    //            {
    //                return await Mediator.Send(new Query(request));
    //            });
    //        }
    //    }
    //    record Query(InstrumentTemplateGetAllExport Data) : IRequest<IResult<FileResult>>
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
    //                ExportPDFExtension.ExportPDF(maps, "InstrumentTemplates", "InstrumentTemplates List") :
    //                ExportExcelExtension.ToExcel(maps, "InstrumentTemplates", "List");


    //            return Result<FileResult>.Success(response);
    //        }
    //    }
    //    static InstrumentTemplateExportResponse MapToResponseExcel(this InstrumentTemplateResponse row)
    //    {
    //        return new()
    //        {
    //            Name = row.Name,

    //        };
    //    }


    //}
}
