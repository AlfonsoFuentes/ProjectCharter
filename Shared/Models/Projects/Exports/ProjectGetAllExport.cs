using Shared.Enums.ExportFiles;
using Shared.Models.Projects.Reponses;

namespace Shared.Models.Projects.Exports
{
    public record ProjectGetAllExport(ExportFileType FileType, Guid ProjectId);
}
