using Blazored.FluentValidation;
using MudBlazor;
using Shared.Enums.NozzleTypes;
using Shared.Enums.ValvesEnum;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.BudgetItems.IndividualItems.Valves.Records;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;
using Shared.Models.Templates.Equipments.Responses;
using Shared.Models.Templates.Valves.Records;
using Shared.Models.Templates.Valves.Responses;
using MudBlazorWeb.Pages.Brands;

namespace MudBlazorWeb.Pages.BudgetItems.Valves;
public partial class ValveDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }


    FluentValidationValidator _fluentValidationValidator = null!;
   
    async Task GetValveResponse()
    {
        if (Model.Id != Guid.Empty)
        {
            var result = await GenericService.GetById<ValveResponse, GetValveByIdRequest>(
                  new GetValveByIdRequest() { Id = Model.Id });

            if (result.Succeeded)
            {
                Model = result.Data;
                //await OnChageDetails();
            }

        }


    }
 
    protected override async Task OnInitializedAsync()
    {
 
        await GetValveResponse();
        StateHasChanged();

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
    public ValveResponse Model { get; set; } = new();
    [Parameter]
    public bool IsEdit { get; set; } = true;
   
}
