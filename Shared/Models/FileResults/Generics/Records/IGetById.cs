using Shared.Models.FileResults.Generics.Request;

namespace Shared.Models.FileResults.Generics.Records
{
    public interface IGetById : IRequest
    {
        Guid Id { get; }

    }
    public interface IGetAll : IRequest
    {
       
    }
}
