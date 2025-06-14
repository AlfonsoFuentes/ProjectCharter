using Shared.Enums.ProjectNeedTypes;

namespace Shared.Models.Projects.Reponses
{
    public class ProjectExportResponse
    {
        public string Name { get; set; } = string.Empty;
        public ProjectNeedTypeEnum ProjectNeedType { get; set; } = ProjectNeedTypeEnum.None;
    }
}
