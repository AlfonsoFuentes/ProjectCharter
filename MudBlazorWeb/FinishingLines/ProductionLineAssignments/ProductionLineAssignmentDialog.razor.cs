using Blazored.FluentValidation;
using MudBlazor;
using Shared.Commons;
using Shared.Models.FinishingLines.InitialLevelWips;
using Shared.Models.FinishingLines.ProductionLineAssignments;
using Shared.Models.FinishingLines.ProductionLines;
using Shared.Models.FinishingLines.WIPTankLines;

namespace MudBlazorWeb.FinishingLines.ProductionLineAssignments;
public partial class ProductionLineAssignmentDialog
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
        if (Model.PlanId == Guid.Empty)
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
    public ProductionLineAssignmentResponse Model { get; set; } = new();

    ProductionLineResponseList ProductionLineResponseList = new();
    bool loaded = false;
    protected override async Task OnInitializedAsync()
    {
        await GetAllProductionLines();
        await GetById();
        loaded = true;

    }
    async Task GetAllProductionLines()
    {
        var result = await GenericService.GetAll<ProductionLineResponseList, ProductionLineGetAll>(new ProductionLineGetAll());
        if (result.Succeeded)
        {
            ProductionLineResponseList = result.Data;
        }
    }
    async Task GetById()
    {
        if (Model.Id != Guid.Empty)
        {
            var result = await GenericService.GetById<ProductionLineAssignmentResponse,
                GetProductionLineAssignmentByIdRequest>(new GetProductionLineAssignmentByIdRequest { Id = Model.Id });
            if (result.Succeeded)
            {
                Model = result.Data;
            }
        }
    }
    void LineChange()
    {
        if (Model.ProductionLine == null) return;

        Model.InitialLevelWips = Model.ProductionLine.WIPTanks.Select(x => new InitialLevelWipResponse
        {
            WipTankLine = x,

        }).ToList();
    }

}
