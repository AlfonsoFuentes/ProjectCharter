using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;
using Shared.Models.EngineeringFluidCodes.Records;
using Shared.Models.EngineeringFluidCodes.Responses;
using MudBlazorWeb.Pages.Brands;
using static MudBlazor.CategoryTypes;
using MudBlazorWeb.Pages.ItemsTemplates.FluidCodes;
using Shared.Models.Templates.Pipings.Records;
using Shared.Models.Templates.Pipings.Responses;
using Shared.Models.BudgetItems.IndividualItems.Valves.Records;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Records;

namespace MudBlazorWeb.Pages.BudgetItems.Pipings;
public partial class PipeDialog
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

    protected override async Task OnInitializedAsync()
    {
     
        await GetPipeResponse();
        StateHasChanged();

    }

    private void Cancel() => MudDialog.Cancel();

    [Parameter]
    public PipeResponse Model { get; set; } = new();
    [Parameter]
    public bool IsEdit { get; set; } = true;
    async Task GetPipeResponse()
    {
        if (Model.Id != Guid.Empty)
        {
            var result = await GenericService.GetById<PipeResponse, GetPipeByIdRequest>(
                  new GetPipeByIdRequest() { Id = Model.Id });

            if (result.Succeeded)
            {
                Model = result.Data;
          
            }

        }


    }
   
  
}
