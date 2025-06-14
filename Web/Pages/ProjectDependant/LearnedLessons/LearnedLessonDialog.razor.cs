using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.LearnedLessons.Responses;

namespace Web.Pages.ProjectDependant.LearnedLessons;
public partial class LearnedLessonDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    // Método asincrónico para realizar la validación
    private bool Validated { get; set; }
    // Método asincrónico para realizar la validación
    public async Task ValidateAsync()
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
    public LearnedLessonResponse Model { get; set; } = new();

}
