using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.FinishingLines.WIPTankLines;

namespace MudBlazorWeb.FinishingLines.WIPTankLines;
public partial class WIPTankLineDialog
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
    public WIPTankLineResponse Model { get; set; } = new();
}
