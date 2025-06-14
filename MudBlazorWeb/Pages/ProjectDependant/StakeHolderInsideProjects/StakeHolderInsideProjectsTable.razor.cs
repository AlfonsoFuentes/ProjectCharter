using MudBlazor;
using Newtonsoft.Json.Linq;
using Shared.Models.StakeHolderInsideProjects.Records;
using Shared.Models.StakeHolderInsideProjects.Requests;
using Shared.Models.StakeHolderInsideProjects.Responses;
using Shared.Models.Projects.Reponses;
using Shared.StaticClasses;
using MudBlazorWeb.Templates;
using Shared.Models.FileResults.Generics.Reponses;

namespace MudBlazorWeb.Pages.ProjectDependant.StakeHolderInsideProjects;
public partial class StakeHolderInsideProjectsTable
{

    [Parameter]
    public ProjectResponse Project { get; set; } = null!;
    public List<StakeHolderInsideProjectResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    public Func<StakeHolderInsideProjectResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<StakeHolderInsideProjectResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    protected override async Task OnParametersSetAsync()
    {
        await GetAll();
    }
    async Task GetAll()
    {
        if (Project == null) return;
        var result = await GenericService.GetAll<StakeHolderInsideProjectResponseList, StakeHolderInsideProjectGetAll>(new StakeHolderInsideProjectGetAll()
        {
            ProjectId = Project.Id,
        });
        if (result.Succeeded)
        {
            Items = result.Data.Items;
        }
    }
    public async Task AddNew()
    {

        var parameters = new DialogParameters<StakeHolderInsideProjectDialog>
        {
            { x => x.Model, new StakeHolderInsideProjectResponse(){ProjectId=Project.Id,Create=true } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<StakeHolderInsideProjectDialog>("Stake Holder Inside Project", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }
    }



    async Task Edit(StakeHolderInsideProjectResponse response)
    {


        var parameters = new DialogParameters<StakeHolderInsideProjectDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<StakeHolderInsideProjectDialog>("Stake Holder Inside Project", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    public async Task Delete(StakeHolderInsideProjectResponse response)
    {
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete {response.Name}? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {
            DeleteStakeHolderInsideProjectRequest request = new()
            {
                Id = response.StakeHolder!.Id,
                Name = response.Name,
                ProjectId = Project.Id,

            };
            var resultDelete = await GenericService.Post(request);
            if (resultDelete.Succeeded)
            {
                await GetAll();
                _snackBar.ShowSuccess(resultDelete.Messages);


            }
            else
            {
                _snackBar.ShowError(resultDelete.Messages);
            }
        }

    }
    HashSet<StakeHolderInsideProjectResponse> SelecteItems = null!;
    public async Task DeleteGroup()
    {
        if (SelecteItems == null) return;
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete this {SelecteItems.Count} Items? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {
            DeleteGroupStakeHolderInsideProjectRequest request = new()
            {
                SelecteItems = SelecteItems,
                ProjectId = Project.Id,

            };
            var resultDelete = await GenericService.Post(request);
            if (resultDelete.Succeeded)
            {
                await GetAll();
                _snackBar.ShowSuccess(resultDelete.Messages);
                SelecteItems = null!;

            }
            else
            {
                _snackBar.ShowError(resultDelete.Messages);
            }
        }

    }


}
