using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.FinishingLines.BIGWIPTanks;
using Shared.Models.FinishingLines.InitialLevelBigWips;
using Shared.Models.FinishingLines.ProductionPlans;
using static MudBlazor.CategoryTypes;

namespace MudBlazorWeb.FinishingLines.ProductionPlans;
public partial class ProductionPlanDialog
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
        await GetAllBigwiptansk();
        if (Model.InitialLevelBigWips.Count == 0)
        {
            Model.InitialLevelBigWips = BIGWIPTankResponseList.Items.Select(x => new InitialLevelBigWipResponse()
            {
                BigWipTank = x,
                ProductionPlanId = Model.Id,
            }).ToList();
        }
    }
    BIGWIPTankResponseList BIGWIPTankResponseList = new();
    async Task GetAllBigwiptansk()
    {
        var result = await GenericService.GetAll<BIGWIPTankResponseList, BIGWIPTankGetAll>(new BIGWIPTankGetAll());
        if (result.Succeeded)
        {
            BIGWIPTankResponseList = result.Data;
        }
    }
    private void Cancel() => MudDialog.Cancel();

    [Parameter]
    public ProductionPlanResponse Model { get; set; } = new();

    async Task GetById()
    {
        var result = await GenericService.GetById<ProductionPlanResponse, GetProductionPlanByIdRequest>(new()
        {
            Id = Model.Id
        });
        if (result.Succeeded)
        {
            Model = result.Data;
        }
        else
        {
            _snackBar.ShowError(result.Messages);
            MudDialog.Cancel();
        }
    }
}
