using MudBlazor;
using MudBlazorWeb.Pages.BudgetItems.Equipments;
using MudBlazorWeb.Templates;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Responses;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;

namespace MudBlazorWeb.Pages.BudgetItems.Valves;
public partial class BasicValveTable
{
    [Parameter]
    public ValveResponse Response { get; set; } = new();

    List<BasicValveResponse> OrderedItems => Response.Items.Count == 0 ? new() : Response.Items.OrderBy(x => x.TagNumber).ToList();



    string nameFilter = string.Empty;
    public Func<BasicValveResponse, bool> Criteria => x =>
    x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
    x.Brand.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<BasicValveResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? OrderedItems :
        OrderedItems.Where(Criteria).ToList();

    HashSet<BasicValveResponse> SelecteItems = null!;
    [Parameter]
    public EventCallback GetAll { get; set; }

    public async Task AddNew()
    {
        BasicValveResponse model = new()
        {
            ValveId = Response.Id,
            ProjectId = Response.ProjectId,
        };

        var parameters = new DialogParameters<BasicValveDialog>
        {
             { x => x.Model, model },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<BasicValveDialog>("Add", parameters, options);
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
            DeleteGroupBasicValveRequest request = new()
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
    async Task Edit(BasicValveResponse response)
    {


        var parameters = new DialogParameters<BasicValveDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<BasicValveDialog>("Edit", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
        }
    }
    public async Task Delete(BasicValveResponse response)
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
            DeleteBasicValveRequest request = new()
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
