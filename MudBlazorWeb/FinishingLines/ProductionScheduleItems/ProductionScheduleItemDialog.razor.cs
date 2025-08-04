using Blazored.FluentValidation;
using MudBlazor;
using MudBlazorWeb.FinishingLines.SKUs;
using Shared.Models.FinishingLines.LineSpeeds;
using Shared.Models.FinishingLines.ProductionScheduleItems;
using Shared.Models.FinishingLines.SKUs;

namespace MudBlazorWeb.FinishingLines.ProductionScheduleItems;
public partial class ProductionScheduleItemDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }


    FluentValidationValidator _fluentValidationValidator = null!;

    private async Task Submit()
    {
        if(Model.ProductionLineAssigmentId == Guid.Empty)
        {
            MudDialog.Close(DialogResult.Ok(true));
            return;
        }
        var result = await GenericService.Post(Model);


        if (result.Succeeded)
        {
            _snackBar.ShowSuccess(result.Messages);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            _snackBar.ShowError(result.Messages);
        }

    }


    private void Cancel() => MudDialog.Cancel();

    [Parameter]
    public ProductionScheduleItemResponse Model { get; set; } = new();
    private Task<IEnumerable<SKUResponse>> SearchSKU(string value, CancellationToken token)
    {
        Func<SKUResponse, bool> Criteria = x =>
        x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)
        ;
        IEnumerable<SKUResponse> FilteredItems = string.IsNullOrEmpty(value) ? SKUList.AsEnumerable() :
             SKUList.Where(Criteria);
        return Task.FromResult(FilteredItems);
    }
    public async Task AddSku()
    {

        var parameters = new DialogParameters<SKUDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<SKUDialog>("SKU", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAllSkus();

        }
    }
    LineSpeedResponseList SKUResponseList { get; set; } = new();
    List<SKUResponse> SKUList => SKUResponseList.Items.Select(x=>x.Sku!).ToList();
    protected override async Task OnInitializedAsync()
    {
        await GetAllSkus();

    }
    async Task GetAllSkus()
    {
        var result = await GenericService.GetAll<LineSpeedResponseList, LineSpeedGetAll>(new LineSpeedGetAll() { LineId = Model.ProductionLineId });
        if (result.Succeeded)
        {
            SKUResponseList = result.Data;
        }
    }
}
