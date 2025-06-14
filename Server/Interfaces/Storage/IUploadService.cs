using Shared.Models.IdentityModels.Requests;

namespace Server.Interfaces.Storage
{
    public interface IUploadService
    {
        string UploadAsync(UploadRequest request);
    }
}