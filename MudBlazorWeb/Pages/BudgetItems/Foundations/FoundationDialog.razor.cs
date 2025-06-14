using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.BudgetItems.IndividualItems.Foundations.Responses;

namespace MudBlazorWeb.Pages.BudgetItems.Foundations;
public partial class FoundationDialog
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
    public FoundationResponse Model { get; set; } = new();
    [Parameter]
    public bool IsEdit { get; set; } = true;
}
