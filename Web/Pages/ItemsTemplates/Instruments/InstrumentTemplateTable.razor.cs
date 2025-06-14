using MudBlazor;
using Shared.Models.Templates.Instruments.Records;
using Shared.Models.Templates.Instruments.Requests;
using Shared.Models.Templates.Instruments.Responses;
using Web.Pages.ItemsTemplates.Instruments;
using Web.Templates;

namespace Web.Pages.ItemsTemplates.Instruments;
public partial class InstrumentTemplateTable
{
    [Parameter]
    public List<InstrumentTemplateResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    Func<InstrumentTemplateResponse, bool> Criteria => x =>
    x.VariableInstrument.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.BrandName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.Model.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.Material.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)
    ;
    public List<InstrumentTemplateResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    protected override async Task OnInitializedAsync()
    {

        if (!ByParameter)
            await GetAll();

    }
    async Task GetAll()
    {
        var result = await GenericService.GetAll<InstrumentTemplateResponseList, InstrumentTemplateGetAll>(new InstrumentTemplateGetAll());
        if (result.Succeeded)
        {
            Items = result.Data.Items;
           


        }
        if (ByParameter && UpdateForm.HasDelegate)
        {
            await UpdateForm.InvokeAsync();
        }
    }
    [Parameter]
    public EventCallback UpdateForm { get; set; }
    [Parameter]
    public bool ByParameter { get; set; } = false;
    public async Task AddNew()
    {

        var parameters = new DialogParameters<InstrumentTemplateDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<InstrumentTemplateDialog>("Instrument Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }
    }



    async Task Edit(InstrumentTemplateResponse response)
    {


        var parameters = new DialogParameters<InstrumentTemplateDialog>
        {

            { x => x.Model, response},
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<InstrumentTemplateDialog>("Instrument Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    public async Task Delete(InstrumentTemplateResponse response)
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
            DeleteInstrumentTemplateRequest request = new()
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
    HashSet<InstrumentTemplateResponse> SelecteItems = null!;
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
            DeleteGroupInstrumentTemplatesRequest request = new()
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
    [Parameter]
    public EventCallback<InstrumentTemplateResponse> SendToForm { get; set; }
    async Task Copy(InstrumentTemplateResponse response)
    {
        InstrumentTemplateResponse request = new()
        {
            Brand = response.Brand,
            Material = response.Material,
            Model = response.Model,
            Name = response.Name,
            Nozzles = response.Nozzles,

            SignalType = response.SignalType,
            Reference = response.Reference,
            ModifierVariable = response.ModifierVariable,
            VariableInstrument = response.VariableInstrument,
            Value = response.Value,
            ConnectionType = response.ConnectionType,




        };
        var parameters = new DialogParameters<InstrumentTemplateDialog>
        {

            { x => x.Model, request},
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<InstrumentTemplateDialog>("Instrument Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }

    }

}
