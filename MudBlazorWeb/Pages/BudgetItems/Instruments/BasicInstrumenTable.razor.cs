using MudBlazor;
using MudBlazorWeb.Pages.BudgetItems.Equipments;
using MudBlazorWeb.Templates;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Requests;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Responses;

namespace MudBlazorWeb.Pages.BudgetItems.Instruments;
public partial class BasicInstrumenTable
{
    [Parameter]
    public InstrumentResponse Response { get; set; } = new();

    List<BasicInstrumentResponse> OrderedItems => Response.Items.Count == 0 ? new() : Response.Items.OrderBy(x => x.TagNumber).ToList();



    string nameFilter = string.Empty;
    public Func<BasicInstrumentResponse, bool> Criteria => x =>
    x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
    x.Brand.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<BasicInstrumentResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? OrderedItems :
        OrderedItems.Where(Criteria).ToList();

    HashSet<BasicInstrumentResponse> SelecteItems = null!;
    [Parameter]
    public EventCallback GetAll { get; set; }

    public async Task AddNew()
    {
        BasicInstrumentResponse model = new()
        {
            InstrumentId = Response.Id,
            ProjectId = Response.ProjectId,
        };

        var parameters = new DialogParameters<BasicInstrumentDialog>
        {
             { x => x.Model, model },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<BasicInstrumentDialog>("Add", parameters, options);
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
            DeleteGroupBasicInstrumentRequest request = new()
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
    async Task Edit(BasicInstrumentResponse response)
    {


        var parameters = new DialogParameters<BasicInstrumentDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<BasicInstrumentDialog>("Edit", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll.InvokeAsync();
        }
    }
    public async Task Delete(BasicInstrumentResponse response)
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
            DeleteBasicInstrumentRequest request = new()
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
                BasicInstrumentResponse model = new()
                {
                    InstrumentId = item.InstrumentId,
                    Name = item.Name,
                    Brand = item.Brand,
                    BudgetUSD = item.BudgetUSD,
                    ProjectId = item.ProjectId,
                    IsExisting = item.IsExisting,
                    Nozzles = item.Nozzles.Select(x => new Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses.NozzleResponse()
                    {
                        Name = x.Name,
                        ConnectionType = x.ConnectionType,
                        NominalDiameter = x.NominalDiameter,
                        NozzleType = x.NozzleType,
                        Order = x.Order
                    }).ToList(),
                    Order = item.Order + 1,
                    Template = item.Template,
                    ProvisionalTag = item.ProvisionalTag,
                    ShowDetails = item.ShowDetails,
                    ShowProvisionalTag = item.ShowProvisionalTag,
                    TemplateId = item.TemplateId


                };

                var parametersCopy = new DialogParameters<BasicInstrumentDialog>
                {
                     { x => x.Model, model },
                };

                var optionsCopy = new DialogOptions() { MaxWidth = model.ShowDetails ? MaxWidth.ExtraLarge : MaxWidth.Medium };

                var dialogsCopy = await DialogService.ShowAsync<BasicInstrumentDialog>("Add", parametersCopy, optionsCopy);
                var resulsCopyt = await dialog.Result;
                if (resulsCopyt!.Canceled)
                {
                    break;
                }

            }
            await GetAll.InvokeAsync();
            StateHasChanged();
        }
    }
}
