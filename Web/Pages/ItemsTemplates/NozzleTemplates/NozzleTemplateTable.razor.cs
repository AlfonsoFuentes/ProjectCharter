using MudBlazor;
using Shared.Models.Templates.NozzleTemplates;

namespace Web.Pages.ItemsTemplates.NozzleTemplates;
public partial class NozzleTemplateTable
{
    string nameFilter { get; set; } = string.Empty;

    [Parameter]
    public List<NozzleTemplateResponse> Items { get; set; } = new();

    [Parameter]
    public EventCallback<List<NozzleTemplateResponse>> ItemsChanged { get; set; }
    [Parameter]
    public bool DisableAdd { get; set; } = false;
    async Task OnItemsChanged()
    {
        await ItemsChanged.InvokeAsync(Items);
    }
    [Parameter]
    public bool EditDiameter { get; set; } = true;
    [Parameter]
    public bool EditConnection { get; set; } = true;
    async Task Add()
    {
        var parameters = new DialogParameters<NozzleTemplateDialog>
        {
             { x => x.EditDiameter, EditDiameter},
             { x => x.EditConnection, EditConnection},
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<NozzleTemplateDialog>("Nozzle Template", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled && result.Data != null)
        {
            var model = result.Data as NozzleTemplateResponse;
            Items.Add(model!);
            await OnItemsChanged();
            await Validate.InvokeAsync();
            StateHasChanged();
        }

    }
    [Parameter]

    public EventCallback Validate { get; set; }
    async Task Edit(NozzleTemplateResponse item)
    {
        var parameters = new DialogParameters<NozzleTemplateDialog>
        {
             { x => x.Model, item},
              { x => x.EditDiameter, EditDiameter},
              { x => x.EditConnection, EditConnection},
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<NozzleTemplateDialog>("Nozzle Template", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled && result.Data != null)
        {
            var model = result.Data as NozzleTemplateResponse;
            item = model!;
            await OnItemsChanged();
            await Validate.InvokeAsync();
            StateHasChanged();
        }
    }

    async Task Delete(NozzleTemplateResponse item)
    {
        Items.Remove(item);
        await OnItemsChanged();
        await Validate.InvokeAsync();
    }
}
