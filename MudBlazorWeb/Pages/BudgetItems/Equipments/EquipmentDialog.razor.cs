using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Records;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.Templates.Equipments.Records;
using Shared.Models.Templates.Equipments.Responses;
using Shared.Models.Templates.NozzleTemplates;
using System.Threading.Tasks;
using MudBlazorWeb.Pages.Brands;

namespace MudBlazorWeb.Pages.BudgetItems.Equipments;
public partial class EquipmentDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }


    FluentValidationValidator _fluentValidationValidator = null!;
   
    async Task GetEquipmentResponse()
    {
        if (Model.Id != Guid.Empty)
        {
            var result = await GenericService.GetById<EquipmentResponse, GetEquipmentByIdRequest>(
                  new GetEquipmentByIdRequest() { Id = Model.Id });

            if (result.Succeeded)
            {
                Model = result.Data;
               



            }

        }


    }
  
    protected override async Task OnInitializedAsync()
    {
    
        await GetEquipmentResponse();
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
    public EquipmentResponse Model { get; set; } = new();
    [Parameter]
    public bool IsEdit { get; set; } = true;
    
}
