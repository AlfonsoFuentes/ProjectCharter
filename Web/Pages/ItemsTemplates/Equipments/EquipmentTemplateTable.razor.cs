using MudBlazor;
using Shared.Models.Templates.Equipments.Records;
using Shared.Models.Templates.Equipments.Requests;
using Shared.Models.Templates.Equipments.Responses;
using Web.Templates;

namespace Web.Pages.ItemsTemplates.Equipments;
public partial class EquipmentTemplateTable
{
    [Parameter]
    public List<EquipmentTemplateResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    Func<EquipmentTemplateResponse, bool> Criteria => x =>
    x.Type.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.SubType.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.BrandName.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.Reference.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.Model.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.InternalMaterial.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.ExternalMaterial.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase)
    ;
    public List<EquipmentTemplateResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
     
    protected override async Task OnInitializedAsync()
    {

        if (!ByParameter)
            await GetAll();
    }
    [Parameter]
    public bool ByParameter { get; set; } = false;
    async Task GetAll()
    {
        var result = await GenericService.GetAll<EquipmentTemplateResponseList, EquipmentTemplateGetAll>(new EquipmentTemplateGetAll());
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
    public EventCallback<EquipmentTemplateResponse> SendToForm { get; set; } 
    public async Task AddNew()
    {

        var parameters = new DialogParameters<EquipmentTemplateDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<EquipmentTemplateDialog>("Equipment Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }
    }

    async Task Copy(EquipmentTemplateResponse response)
    {
        EquipmentTemplateResponse request = new()
        {
            Brand = response.Brand,
            ExternalMaterial = response.ExternalMaterial,
            InternalMaterial = response.InternalMaterial,
            Model = response.Model,
            Name = response.Name,
            Nozzles = response.Nozzles,
            Reference = response.Reference,
            SubType = response.SubType,
            TagLetter = response.TagLetter,
            Type = response.Type,
            Value = response.Value,



        };
        var parameters = new DialogParameters<EquipmentTemplateDialog>
        {

            { x => x.Model, request},
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<EquipmentTemplateDialog>("Equipment Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
        
    }

    async Task Edit(EquipmentTemplateResponse response)
    {


        var parameters = new DialogParameters<EquipmentTemplateDialog>
        {

            { x => x.Model, response},
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<EquipmentTemplateDialog>("Equipment Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    public async Task Delete(EquipmentTemplateResponse response)
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
            DeleteEquipmentTemplateRequest request = new()
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
    HashSet<EquipmentTemplateResponse> SelecteItems = null!;
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
            DeleteGroupEquipmentTemplatesRequest request = new()
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
