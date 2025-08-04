using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.FinishingLines.ProductionLines;

namespace MudBlazorWeb.FinishingLines.ProductionLines;
public partial class ProductionLineDialog
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
    public ProductionLineResponse Model { get; set; } = new();

    async Task GetById()
    {
        if (Model.Id != Guid.Empty)
        {
            var response = await GenericService.GetById<ProductionLineResponse, GetProductionLineByIdRequest>(new GetProductionLineByIdRequest()
            {
                Id = Model.Id
            });
            if (response.Succeeded)
            {
                Model = response.Data;
            }
            else
            {
                _snackBar.ShowError("Production Line not found.");
            }
        }
    }
}
