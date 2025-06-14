using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;

namespace MudBlazorWeb.Pages.BudgetItems.Equipments;
public partial class BasicEquipmentTable
{
    [Parameter]
    public EquipmentResponse Response { get; set; } = new();

    List<BasicEquipmentResponse> OrderedItems => Response.Items.Count == 0 ? new() : Response.Items.OrderBy(x => x.TagNumber).ToList();



    string nameFilter = string.Empty;
    public Func<BasicEquipmentResponse, bool> Criteria => x =>
    x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
    x.Brand.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<BasicEquipmentResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? OrderedItems :
        OrderedItems.Where(Criteria).ToList();

    HashSet<BasicEquipmentResponse> SelecteItems = null!;
    [Parameter]
    public EventCallback GetAll { get; set; }

    public async Task AddNew()
    {
        BasicEquipmentResponse model = new()
        {
            EquipmentId = Response.Id,
            ProjectId = Response.ProjectId,
        };

        var parameters = new DialogParameters<BasicEquipmentDialog>
        {
             { x => x.Model, model },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<BasicEquipmentDialog>("Add", parameters, options);
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
            DeleteGroupBasicEquipmentRequest request = new()
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
    async Task Edit(BasicEquipmentResponse response)
    {


        var parameters = new DialogParameters<BasicEquipmentDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<BasicEquipmentDialog>("Edit", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
        }
    }
    public async Task Delete(BasicEquipmentResponse response)
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
            DeleteBasicEquipmentRequest request = new()
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
    async Task CopySelectedGroup()
    {
        if (SelecteItems == null) return;
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to copy this {SelecteItems.Count} Items? This process cannot be undone." },
            { x => x.ButtonText, "Copy" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Copy", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            foreach (var item in SelecteItems)
            {
                BasicEquipmentResponse model = new()
                {
                    EquipmentId = item.EquipmentId,
                    ProjectId = item.ProjectId,
                    Name = item.Name,
                    Brand = item.Brand,
                    BudgetUSD = item.BudgetUSD,
                    Nozzles = item.Nozzles.Select(x => new Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses.NozzleResponse()
                    {
                        Name = x.Name,
                        ConnectionType = x.ConnectionType,
                        NominalDiameter = x.NominalDiameter,
                        NozzleType = x.NozzleType,
                        Order = x.Order
                    }).ToList(),
                    ShowDetails = item.ShowDetails,
                    Template = item.Template,
                    TagNumber = item.TagNumber,
                    IsExisting = item.IsExisting,
                    ProvisionalTag = item.ProvisionalTag,
                    ShowProvisionalTag = item.ShowProvisionalTag,
                     
                     Order = item.Order + 1,
                };

                var parametersCopy = new DialogParameters<BasicEquipmentDialog>
                {
                     { x => x.Model, model },
                };

                var optionsCopy = new DialogOptions() { MaxWidth = model.ShowDetails? MaxWidth.ExtraLarge : MaxWidth.Medium};

                var dialogsCopy = await DialogService.ShowAsync<BasicEquipmentDialog>("Add", parametersCopy, optionsCopy);
                var resulsCopyt = await dialog.Result;
                if(resulsCopyt!.Canceled)
                {
                    break;
                }

            }
            await GetAll.InvokeAsync();
            StateHasChanged();
        }
    }
}
