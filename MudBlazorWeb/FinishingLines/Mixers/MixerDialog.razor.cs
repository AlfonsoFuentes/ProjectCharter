using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.FinishingLines.Mixers;

namespace MudBlazorWeb.FinishingLines.Mixers;
public partial class MixerDialog
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
    public MixerResponse Model { get; set; } = new();

    async Task GetById()
    {
        var result = await GenericService.GetById<MixerResponse, GetMixerByIdRequest>(new GetMixerByIdRequest { Id = Model.Id });
        if (result.Succeeded)
        {
            Model = result.Data;
        }
        else
        {
            _snackBar.ShowError(result.Messages);
        }
    }
}
