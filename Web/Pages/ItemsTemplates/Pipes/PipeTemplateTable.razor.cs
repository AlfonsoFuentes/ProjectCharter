using MudBlazor;
using Shared.Models.Templates.Pipings.Records;
using Shared.Models.Templates.Pipings.Requests;
using Shared.Models.Templates.Pipings.Responses;
using Web.Templates;

namespace Web.Pages.ItemsTemplates.Pipes;
public partial class PipeTemplateTable
{
    [Parameter]
    public List<PipeTemplateResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    Func<PipeTemplateResponse, bool> Criteria => x =>
    x.Diameter.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||
    x.Class.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase) ||

    x.Material.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase);
    public List<PipeTemplateResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    protected override async Task OnInitializedAsync()
    {

        if (Items.Count == 0)
            await GetAll();
        else ByParameter = true;
    }
    async Task GetAll()
    {
        var result = await GenericService.GetAll<PipeTemplateResponseList, PipeTemplateGetAll>(new PipeTemplateGetAll());
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
    bool ByParameter { get; set; } = false;
    public async Task AddNew()
    {

        var parameters = new DialogParameters<PipeTemplateDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<PipeTemplateDialog>("Pipe Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }
    }



    async Task Edit(PipeTemplateResponse response)
    {


        var parameters = new DialogParameters<PipeTemplateDialog>
        {

            { x => x.Model, response},
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<PipeTemplateDialog>("Pipe Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    public async Task Delete(PipeTemplateResponse response)
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
            DeletePipeTemplateRequest request = new()
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
    HashSet<PipeTemplateResponse> SelecteItems = null!;
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
            DeleteGroupPipeTemplatesRequest request = new()
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
    public EventCallback<PipeTemplateResponse> SendToForm { get; set; }
    async Task Copy(PipeTemplateResponse response)
    {
        PipeTemplateResponse request = new()
        {
     
            Material = response.Material,
            Insulation = response.Insulation,
            Class = response.Class,
            Diameter = response.Diameter,
            EquivalentLenghPrice = response.EquivalentLenghPrice,
            LaborDayPrice = response.LaborDayPrice,




        };
        var parameters = new DialogParameters<PipeTemplateDialog>
        {

            { x => x.Model, request},
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<PipeTemplateDialog>("Pipe Template", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }

    }


}
