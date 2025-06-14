using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.Templates.NozzleTemplates;

namespace MudBlazorWeb.Pages.BudgetItems.Nozzles;
public partial class NozzleBudgetaryDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter]
    public NozzleResponse Model { get; set; } = new();

    FluentValidationValidator _fluentValidationValidator = null!;
    private void Cancel() => MudDialog.Cancel();
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }

    void OnOk()
    {
        MudDialog.Close(DialogResult.Ok(Model));
    }
    [Parameter]
    public bool EditDiameter { get; set; } = true;
    [Parameter]
    public bool EditConnection { get; set; } = true;
}
