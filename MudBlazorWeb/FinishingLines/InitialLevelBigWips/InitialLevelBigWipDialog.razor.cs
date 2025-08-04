using Blazored.FluentValidation;
using MudBlazor;
using MudBlazorWeb.FinishingLines.BackBones;
using MudBlazorWeb.Pages.Suppliers;
using Shared.Models.FinishingLines.BackBones;
using Shared.Models.FinishingLines.InitialLevelBigWips;
using static MudBlazor.CategoryTypes;
using static Shared.StaticClasses.StaticClass;

namespace MudBlazorWeb.FinishingLines.InitialLevelBigWips;
public partial class InitialLevelBigWipDialog
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
        if (Model.Id == Guid.Empty)
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
    public InitialLevelBigWipResponse Model { get; set; } = new();

    private Task<IEnumerable<BackBoneResponse>> SearchBackBone(string value, CancellationToken token)
    {
        Func<BackBoneResponse, bool> Criteria = x =>
        x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)
        ;
        IEnumerable<BackBoneResponse> FilteredItems = string.IsNullOrEmpty(value) ? BackBoneResponseList.Items.AsEnumerable() :
             BackBoneResponseList.Items.Where(Criteria);
        return Task.FromResult(FilteredItems);
    }

}
