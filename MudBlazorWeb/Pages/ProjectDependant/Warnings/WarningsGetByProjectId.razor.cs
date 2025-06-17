using Shared.Models.Projects.Reponses;
using Shared.Models.Scopes.Records;
using Shared.Models.Scopes.Responses;
using Shared.Models.Warnings.Records;
using Shared.Models.Warnings.Responses;

namespace MudBlazorWeb.Pages.ProjectDependant.Warnings;
public partial class WarningsGetByProjectId
{
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;
    public List<WarningResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    public Func<WarningResponse, bool> Criteria => x => x.WarningText.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<WarningResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
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
            Items = result.Data.Items;
        }
    }
}
