using Shared.Models.EngineeringFluidCodes.Exports;
using Shared.Models.EngineeringFluidCodes.Responses;

namespace Server.EndPoint.EngineeringFluidCodes.Exports
{
    //public static class GetAllEngineeringFluidCodeExportEndPoint
    //{
    //    public class EndPoint : IEndPoint
    //    {
    //        public void MapEndPoint(IEndpointRouteBuilder app)
    //        {
    //            app.MapPost(StaticClass.EngineeringFluidCodes.EndPoint.Export, async (EngineeringFluidCodeGetAllExport request, IMediator Mediator) =>
    //            {
    //                return await Mediator.Send(new Query(request));
    //            });
    //        }
    //    }
    //    record Query(EngineeringFluidCodeGetAllExport Data) : IRequest<IResult<FileResult>>
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
    //                ExportPDFExtension.ExportPDF(maps, "EngineeringFluidCodes", "EngineeringFluidCodes List") :
    //                ExportExcelExtension.ToExcel(maps, "EngineeringFluidCodes", "List");


    //            return Result<FileResult>.Success(response);
    //        }
    //    }
    //    static EngineeringFluidCodeExportResponse MapToResponseExcel(this EngineeringFluidCodeResponse row)
    //    {
    //        return new()
    //        {
    //            Name = row.Name,

    //        };
    //    }


    //}
}
