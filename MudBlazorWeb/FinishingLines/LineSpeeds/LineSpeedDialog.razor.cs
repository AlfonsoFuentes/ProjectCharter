using Blazored.FluentValidation;
using MudBlazor;
using MudBlazorWeb.FinishingLines.BackBones;
using MudBlazorWeb.FinishingLines.SKUs;
using Shared.Models.FinishingLines.BackBones;
using Shared.Models.FinishingLines.LineSpeeds;
using Shared.Models.FinishingLines.SKUs;

namespace MudBlazorWeb.FinishingLines.LineSpeeds;
public partial class LineSpeedDialog
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
        if (Model.LineId == Guid.Empty)
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
    public LineSpeedResponse Model { get; set; } = new();

    private Task<IEnumerable<SKUResponse>> SearchSKU(string value, CancellationToken token)
    {
        Func<SKUResponse, bool> Criteria = x =>
        x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)
        ;
        IEnumerable<SKUResponse> FilteredItems = string.IsNullOrEmpty(value) ? SKUResponseList.Items.AsEnumerable() :
             SKUResponseList.Items.Where(Criteria);
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
    SKUResponseList SKUResponseList { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        await GetAllSkus();

    }
    async Task GetAllSkus()
    {
        var result = await GenericService.GetAll<SKUResponseList, SKUGetAll>(new SKUGetAll());
        if (result.Succeeded)
        {
            SKUResponseList = result.Data;
        }
    }
}
