using Blazored.FluentValidation;
using MudBlazor;
using MudBlazorWeb.FinishingLines.BackBones;
using MudBlazorWeb.Pages.Suppliers;
using Shared.Models.FinishingLines.BackBones;
using Shared.Models.FinishingLines.InitialLevelWips;
using static MudBlazor.CategoryTypes;
using static Shared.StaticClasses.StaticClass;

namespace MudBlazorWeb.FinishingLines.InitialLevelWips;
public partial class InitialLevelWipDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }


    FluentValidationValidator _fluentValidationValidator = null!;
    BackBoneResponseList BackBoneResponseList { get; set; } = new();
    
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
    public InitialLevelWipResponse Model { get; set; } = new();

   
   
}
