using Blazored.FluentValidation;
using MudBlazor;
using Shared.Enums.Instruments;
using Shared.Enums.NozzleTypes;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Records;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.Templates.Equipments.Responses;
using Shared.Models.Templates.Instruments.Records;
using Shared.Models.Templates.Instruments.Responses;
using Shared.Models.Templates.NozzleTemplates;
using MudBlazorWeb.Pages.Brands;

namespace MudBlazorWeb.Pages.BudgetItems.Instruments;
public partial class InstrumentDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }
   
    async Task GetInstrumentResponse()
    {
        if (Model.Id != Guid.Empty)
        {
            var result = await GenericService.GetById<InstrumentResponse, GetInstrumentByIdRequest>(
                  new GetInstrumentByIdRequest() { Id = Model.Id });

            if (result.Succeeded)
            {
                Model = result.Data;
               
            }

        }


    }
 
    protected override async Task OnInitializedAsync()
    {
      
        await GetInstrumentResponse();
        StateHasChanged();
     
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
    public InstrumentResponse Model { get; set; } = new();
    [Parameter]
    public bool IsEdit { get; set; } = true;
   
}
