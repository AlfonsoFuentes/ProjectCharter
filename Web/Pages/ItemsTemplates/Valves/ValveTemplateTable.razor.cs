using MudBlazor;
using Shared.Models.StakeHolders.Records;
using Shared.Models.StakeHolders.Requests;
using Shared.Models.StakeHolders.Responses;
using Shared.Models.Templates.Equipments.Responses;
using Shared.Models.Templates.Valves.Records;
using Shared.Models.Templates.Valves.Requests;
using Shared.Models.Templates.Valves.Responses;
using Web.Templates;

namespace Web.Pages.ItemsTemplates.Valves;
public partial class ValveTemplateTable
{
    [Parameter]
    public List<ValveTemplateResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    Func<ValveTemplateResponse, bool> Criteria => x =>
    x.Type.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||

    x.BrandName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||

    x.Model.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.Material.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.Diameter.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)
    ;
    public List<ValveTemplateResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    protected override async Task OnInitializedAsync()
    {

        if (!ByParameter)
            await GetAll();

    }
    async Task GetAll()
    {
        var result = await GenericService.GetAll<ValveTemplateResponseList, ValveTemplateGetAll>(new ValveTemplateGetAll());
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

        var parameters = new DialogParameters<ValveTemplateDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<ValveTemplateDialog>("Valve Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }
    }



    async Task Edit(ValveTemplateResponse response)
    {


        var parameters = new DialogParameters<ValveTemplateDialog>
        {

            { x => x.Model, response},
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<ValveTemplateDialog>("Valve Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    public async Task Delete(ValveTemplateResponse response)
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
            DeleteValveTemplateRequest request = new()
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
    HashSet<ValveTemplateResponse> SelecteItems = null!;
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
            DeleteGroupValveTemplatesRequest request = new()
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
    public EventCallback<ValveTemplateResponse> SendToForm { get; set; }
    async Task Copy(ValveTemplateResponse response)
    {
        ValveTemplateResponse request = new()
        {
            Brand = response.Brand,
            Material = response.Material,
            Model = response.Model,
            Name = response.Name,
            Nozzles = response.Nozzles,
            ActuatorType = response.ActuatorType,
            Diameter = response.Diameter,
            FailType = response.FailType,
            HasFeedBack = response.HasFeedBack,
            PositionerType = response.PositionerType,
            SignalType = response.SignalType,
            TagLetter = response.TagLetter,
            Type = response.Type,
            Value = response.Value,




        };
        var parameters = new DialogParameters<ValveTemplateDialog>
        {

            { x => x.Model, request},
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<ValveTemplateDialog>("Valve Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }

    }

}
