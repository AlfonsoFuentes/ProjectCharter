using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.StakeHolderInsideProjects.Responses;
using Shared.Models.StakeHolders.Records;
using Shared.Models.StakeHolders.Responses;
using MudBlazorWeb.Pages.StakeHolders;

namespace MudBlazorWeb.Pages.ProjectDependant.StakeHolderInsideProjects;
public partial class StakeHolderInsideProjectDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; }
    // Método asincrónico para realizar la validación
    public async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }



    StakeHolderResponseList StakeHolderResponseList = new();


    FluentValidationValidator _fluentValidationValidator = null!;
    protected override async Task OnInitializedAsync()
    {
        await GetAllStakeHolders();
        StateHasChanged();

    }
    async Task GetAllStakeHolders()
    {
        var result = await GenericService.GetAll<StakeHolderResponseList, StakeHolderGetAll>(new StakeHolderGetAll());
        if (result.Succeeded)
        {
            StakeHolderResponseList = result.Data;
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
    public StakeHolderInsideProjectResponse Model { get; set; } = new();
    async Task AddSatkeHolder()
    {
        var parameters = new DialogParameters<StakeHolderDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<StakeHolderDialog>("StakeHolder", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAllStakeHolders();
            StateHasChanged();
        }
    }

}
