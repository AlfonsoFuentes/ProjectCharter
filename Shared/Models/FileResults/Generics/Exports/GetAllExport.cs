using Shared.Enums.ExportFiles;
using Shared.Models.Projects.Reponses;

namespace Shared.Models.FileResults.Generics.Exports
{
    public record GetAllExport<T>(ExportFileType FileType, List<T> query) where T : class;
}
