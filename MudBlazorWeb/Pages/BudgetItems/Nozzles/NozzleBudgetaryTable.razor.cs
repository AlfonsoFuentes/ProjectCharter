using MudBlazor;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.Templates.NozzleTemplates;
using MudBlazorWeb.Pages.ItemsTemplates.NozzleTemplates;
using static MudBlazor.CategoryTypes;

namespace MudBlazorWeb.Pages.BudgetItems.Nozzles;
public partial class NozzleBudgetaryTable
{
    [Parameter]
    public List<NozzleResponse> Items { get; set; } = new();

    [Parameter]
    public EventCallback<List<NozzleResponse>> ItemsChanged { get; set; }

    async Task OnItemsChanged()
    {
        await ItemsChanged.InvokeAsync(Items);
        if (Change.HasDelegate) await Change.InvokeAsync();
        if (Validate.HasDelegate) await Validate.InvokeAsync();
    }
    [Parameter]
    public EventCallback Change { get; set; }
    [Parameter]
    public bool EditDiameter { get; set; } = true;
    [Parameter]
    public bool EditConnection { get; set; } = true;
    async Task Add()
    {
        var parameters = new DialogParameters<NozzleBudgetaryDialog>
        {
             { x => x.EditDiameter, EditDiameter},
             { x => x.EditConnection, EditConnection},
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<NozzleBudgetaryDialog>("Nozzle Template", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled && result.Data != null)
        {
            var model = result.Data as NozzleResponse;
            Items.Add(model!);
            await OnItemsChanged();

        }

    }
    [Parameter]
    public EventCallback Validate { get; set; }


    async Task Edit(NozzleResponse item)
    {
        var parameters = new DialogParameters<NozzleBudgetaryDialog>
        {
             { x => x.Model, item},
             { x => x.EditDiameter, EditDiameter},
             { x => x.EditConnection, EditConnection},
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<NozzleBudgetaryDialog>("Nozzle Template", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled && result.Data != null)
        {
            var model = result.Data as NozzleResponse;
            item = model!;
            await OnItemsChanged();

        }
    }

    async Task Delete(NozzleResponse item)
    {
        Items.Remove(item);
        await OnItemsChanged();

    }
}
