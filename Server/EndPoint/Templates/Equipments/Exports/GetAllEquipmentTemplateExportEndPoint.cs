using Shared.Models.Templates.Equipments.Exports;
using Shared.Models.Templates.Equipments.Responses;

namespace Server.EndPoint.Templates.Equipments.Exports
{
    //public static class GetAllEquipmentTemplateExportEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.EquipmentTemplates.EndPoint.Export, async (EquipmentTemplateGetAllExport request, IMediator Mediator) =>
    //            {
    //                return await Mediator.Send(new Query(request));
    //            });
    //        }
    //    }
    //    record Query(EquipmentTemplateGetAllExport Data) : IRequest<IResult<FileResult>>
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
    //                ExportPDFExtension.ExportPDF(maps, "EquipmentTemplates", "EquipmentTemplates List") :
    //                ExportExcelExtension.ToExcel(maps, "EquipmentTemplates", "List");


    //            return Result<FileResult>.Success(response);
    //        }
    //    }
    //    static EquipmentTemplateExportResponse MapToResponseExcel(this EquipmentTemplateResponse row)
    //    {
    //        return new()
    //        {
    //            Name = row.Name,

    //        };
    //    }


    //}
}
