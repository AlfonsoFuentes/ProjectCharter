using Shared.Enums.ExportFiles;
using Shared.Models.LearnedLessons.Responses;

namespace Shared.Models.LearnedLessons.Exports
{
    public record LearnedLessonGetAllExport(ExportFileType FileType, List<LearnedLessonResponse> query);
}
