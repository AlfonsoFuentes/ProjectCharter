using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FinishingLines.ProductionLineAssignments;
using static Shared.StaticClasses.StaticClass;

namespace MudBlazorWeb.FinishingLines.ProductionLineAssignments;
public partial class ProductionLineAssignmentTable
{
    [Parameter]
    public List<ProductionLineAssignmentResponse> Items { get; set; } = new();
    [Parameter]
    public EventCallback<List<ProductionLineAssignmentResponse>> ItemsChanged { get; set; }
    [Parameter]
    public EventCallback GetAll { get; set; }
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    [Parameter]
    public Guid PlanId { get; set; } = Guid.Empty;

    string nameFilter = string.Empty;
    public Func<ProductionLineAssignmentResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<ProductionLineAssignmentResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    //protected override async Task OnInitializedAsync()
    //{
    //    await GetAll();
    //}
    //async Task GetAll()
    //{
    //    var result = await GenericService.GetAll<ProductionLineAssignmentResponseList, ProductionLineAssignmentGetAll>(new ProductionLineAssignmentGetAll());
    //    if (result.Succeeded)
    //    {
    //        Items = result.Data.Items;
    //    }
    //}
    public async Task AddNew()
    {
        ProductionLineAssignmentResponse response = new()
        {
            PlanId = PlanId,
        };

        var parameters = new DialogParameters<ProductionLineAssignmentDialog>
        {
             { x => x.Model, response },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<ProductionLineAssignmentDialog>("ProductionLineAssignment", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (PlanId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
            else
            {
                Items.Add(response);
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
            }

        }
    }
    async Task Edit(ProductionLineAssignmentResponse response)
    {


        var parameters = new DialogParameters<ProductionLineAssignmentDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<ProductionLineAssignmentDialog>("ProductionLineAssignment", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (PlanId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
            else
            {
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
            }
        }
    }
    public async Task Delete(ProductionLineAssignmentResponse response)
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
            if (PlanId != Guid.Empty)
            {
                DeleteProductionLineAssignmentRequest request = new()
                {
                    Id = response.Id,
                    Name = response.Name,
                    PlanId = PlanId,

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
            else
            {
                Items.Remove(response);
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
            }

        }

    }
    HashSet<ProductionLineAssignmentResponse> SelecteItems = null!;
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
            if (PlanId != Guid.Empty)
            {
                DeleteGroupProductionLineAssignmentRequest request = new()
                {
                    SelecteItems = SelecteItems,

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
            else
            {
                Items.RemoveAll(x => SelecteItems.Contains(x));
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
                _snackBar.ShowSuccess($"Group of {SelecteItems.Count} items deleted successfully.");
                SelecteItems = null!;
            }
        }

    }
    //async Task GetPlanById()
    //{
    //    if (PlanId == Guid.Empty) return;
    //    var result = await GenericService.GetById<ProductionLineAssignmentResponse, 
    //        GetProductionLineAssignmentByIdRequest>(new GetProductionLineAssignmentByIdRequest { Id = PlanId });
    //    if (result.Succeeded)
    //    {
    //        Items = result.Data.Items;
    //        await ItemsChanged.InvokeAsync(Items);
    //    }
    //    else
    //    {
    //        _snackBar.ShowError(result.Messages);
    //    }
    //}
}
