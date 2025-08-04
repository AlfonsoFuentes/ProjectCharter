using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.FinshingLines;
using Shared.Models.FinishingLines.ProductionPlans;
using Shared.Models.FinishingLines.ProductionPlanSimulations;

namespace MudBlazorWeb.FinishingLines.ProductionPlans;
public partial class ProductionPlanTable
{
    public List<ProductionPlanResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    public Func<ProductionPlanResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<ProductionPlanResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    protected override async Task OnInitializedAsync()
    {
        await GetAll();
    }
    async Task GetAll()
    {
        var result = await GenericService.GetAll<ProductionPlanResponseList, ProductionPlanGetAll>(new ProductionPlanGetAll());
        if (result.Succeeded)
        {
            Items = result.Data.Items;
        }
    }
    public async Task AddNew()
    {

        var parameters = new DialogParameters<ProductionPlanDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<ProductionPlanDialog>("ProductionPlan", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            await GetAll();
            StateHasChanged();
        }
    }
    ReadSimulationFromDatabase SimulationData = null!;
    async Task Simulate(ProductionPlanResponse response)
    {
        SimulationData = null!;
        var result = await GenericService.GetById<ReadSimulationFromDatabase, GetProductionPlanSimulationByIdRequest>(new GetProductionPlanSimulationByIdRequest()
        {
            Id = response.Id,
        });
        if (result.Succeeded)
        {
            SimulationData = result.Data;
        }
    }
    async Task Edit(ProductionPlanResponse response)
    {


        var parameters = new DialogParameters<ProductionPlanDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<ProductionPlanDialog>("ProductionPlan", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            await GetAll();
        }
    }
    public async Task Delete(ProductionPlanResponse response)
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
            DeleteProductionPlanRequest request = new()
            {
                Id = response.Id,
                Name = response.Name,

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
    HashSet<ProductionPlanResponse> SelecteItems = null!;
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
            DeleteGroupProductionPlanRequest request = new()
            {
                SelecteItems = SelecteItems,

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
