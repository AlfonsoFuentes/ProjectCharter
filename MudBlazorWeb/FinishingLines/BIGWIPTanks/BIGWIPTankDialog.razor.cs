using Blazored.FluentValidation;
using MudBlazor;
using MudBlazorWeb.FinishingLines.BackBones;
using MudBlazorWeb.Pages.Suppliers;
using Shared.Models.FinishingLines.BackBones;
using Shared.Models.FinishingLines.BIGWIPTanks;
using static MudBlazor.CategoryTypes;
using static Shared.StaticClasses.StaticClass;

namespace MudBlazorWeb.FinishingLines.BIGWIPTanks;
public partial class BIGWIPTankDialog
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
    protected override async Task OnInitializedAsync()
    {
        await GetAllBackBones();

    }
    async Task GetAllBackBones()
    {
        var result = await GenericService.GetAll<BackBoneResponseList, BackBoneGetAll>(new BackBoneGetAll());
        if (result.Succeeded)
        {
            BackBoneResponseList = result.Data;
        }
    }
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
    public BIGWIPTankResponse Model { get; set; } = new();

    private Task<IEnumerable<BackBoneResponse>> SearchBackBone(string value, CancellationToken token)
    {
        Func<BackBoneResponse, bool> Criteria = x =>
        x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)
        ;
        IEnumerable<BackBoneResponse> FilteredItems = string.IsNullOrEmpty(value) ? BackBoneResponseList.Items.AsEnumerable() :
             BackBoneResponseList.Items.Where(Criteria);
        return Task.FromResult(FilteredItems);
    }
    public async Task AddBackBone()
    {

        var parameters = new DialogParameters<BackBoneDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<BackBoneDialog>("BackBone", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAllBackBones();

        }
    }
}
