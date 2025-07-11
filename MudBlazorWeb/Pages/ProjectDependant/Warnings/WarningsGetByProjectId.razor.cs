using Shared.Models.Projects.Reponses;
using Shared.Models.Warnings.Records;
using Shared.Models.Warnings.Responses;

namespace MudBlazorWeb.Pages.ProjectDependant.Warnings;
public partial class WarningsGetByProjectId
{
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;

    WarningResponseList Response = new();
   
   
    protected override async Task OnParametersSetAsync()
    {
        await GetAll();
    }
    async Task GetAll()
    {
        if (Project == null) return;
        var result = await GenericService.GetAll<WarningResponseList, WarningGetByProjectId>(new WarningGetByProjectId()
        {
            ProjectId = Project.Id,
        });
        if (result.Succeeded)
        {
            Response = result.Data;
        }
    }
    
}
