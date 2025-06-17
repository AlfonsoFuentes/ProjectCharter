using Shared.Models.Warnings.Records;
using Shared.Models.Warnings.Responses;

namespace MudBlazorWeb.Pages.ProjectDependant.Warnings;
public partial class WarningsGetAll
{
    public List<WarningResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    public Func<WarningResponse, bool> Criteria => x => x.WarningText.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<WarningResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();

    async Task GetAll()
    {

        var result = await GenericService.GetAll<WarningResponseList, WarningGetAll>(new WarningGetAll());
        if (result.Succeeded)
        {
            Items = result.Data.Items;
        }
    }
    protected override async Task OnInitializedAsync()
    {
        await GetAll();

    }
}
