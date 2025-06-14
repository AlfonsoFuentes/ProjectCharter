using MudBlazor;
using MudBlazorWeb.Pages.BudgetItems.Equipments;
using MudBlazorWeb.Templates;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Responses;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;

namespace MudBlazorWeb.Pages.BudgetItems.Pipings;
public partial class BasicPipeTable
{
    [Parameter]
    public PipeResponse Response { get; set; } = new();

    List<BasicPipeResponse> OrderedItems => Response.Items.Count == 0 ? new() : Response.Items.OrderBy(x => x.TagNumber).ToList();



    string nameFilter = string.Empty;
    public Func<BasicPipeResponse, bool> Criteria => x =>
    x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
    x.TagNumber.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<BasicPipeResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? OrderedItems :
        OrderedItems.Where(Criteria).ToList();

    HashSet<BasicPipeResponse> SelecteItems = null!;
    [Parameter]
    public EventCallback GetAll { get; set; }

    public async Task AddNew()
    {
        BasicPipeResponse model = new()
        {
            PipeId = Response.Id,
            ProjectId = Response.ProjectId,
        };

        var parameters = new DialogParameters<BasicPipeDialog>
        {
             { x => x.Model, model },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<BasicPipeDialog>("Add", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
            StateHasChanged();
        }
    }
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
            DeleteGroupBasicPipeRequest request = new()
            {
                SelecteItems = SelecteItems,
                ProjectId = Response.ProjectId,

            };
            var resultDelete = await GenericService.Post(request);
            if (resultDelete.Succeeded)
            {
                await GetAll.InvokeAsync();
                _snackBar.ShowSuccess(resultDelete.Messages);
                SelecteItems = null!;

            }
            else
            {
                _snackBar.ShowError(resultDelete.Messages);
            }
        }

    }
    async Task Edit(BasicPipeResponse response)
    {


        var parameters = new DialogParameters<BasicPipeDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<BasicPipeDialog>("Edit", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
        }
    }
    public async Task Delete(BasicPipeResponse response)
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
            DeleteBasicPipeRequest request = new()
            {
                Id = response.Id,
                Name = response.Name,
                ProjectId = Response.ProjectId,

            };
            var resultDelete = await GenericService.Post(request);
            if (resultDelete.Succeeded)
            {
                await GetAll.InvokeAsync();
                _snackBar.ShowSuccess(resultDelete.Messages);


            }
            else
            {
                _snackBar.ShowError(resultDelete.Messages);
            }
        }

    }
}
