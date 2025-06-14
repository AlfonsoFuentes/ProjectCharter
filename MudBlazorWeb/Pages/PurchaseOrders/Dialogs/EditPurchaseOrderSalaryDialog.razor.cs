using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.PurchaseOrders.Mappers;
using Shared.Models.PurchaseOrders.Records;
using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;

namespace MudBlazorWeb.Pages.PurchaseOrders.Dialogs;
public partial class EditPurchaseOrderSalaryDialog
{
  
    FluentValidationValidator _fluentValidationValidator = null!;
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }

    [Parameter]
    public PurchaseOrderResponse PurchaseOrder { get; set; } = new();
    public EditPurchaseOrderSalaryRequest Model { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        await GetPurchaseOrder();
      
        await GetBudgetItems();
        Model = PurchaseOrder.ToEditSalaryClosed();
        Model.PurchaseOrderItems.ForEach(x =>
        {
            x.BudgetItem = OriginalBudgetItems.Single(y => y.Id == x.BudgetItemId);


        });
        Model.ProjectPurchaseOrders = ResponseList.ProjectPurchaseOrders;
        StateHasChanged();

    }

    async Task GetPurchaseOrder()
    {
        var result = await GenericService.GetById<PurchaseOrderResponse,
            GetPurchaseOrderByIdRequest>(new GetPurchaseOrderByIdRequest()
            {
                Id = PurchaseOrder.Id,
            });
        if (result.Succeeded)
        {
            PurchaseOrder = result.Data;
        }
    }
    public BudgetItemWithPurchaseOrderResponseList ResponseList { get; set; } = new();
    async Task GetBudgetItems()
    {
        var resultProjectt = await GenericService.GetAll<BudgetItemWithPurchaseOrderResponseList, BudgetItemWithPurchaseOrderGetAll>(new BudgetItemWithPurchaseOrderGetAll()
        {
            ProjectId = PurchaseOrder.ProjectId,
        });
        if (resultProjectt.Succeeded)
        {
            ResponseList = resultProjectt.Data;
            OriginalBudgetItems = resultProjectt.Data.SalariesWithPurchaseOrderss;

        }
    }
    List<BudgetItemWithPurchaseOrdersResponse> OriginalBudgetItems = new();
  


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
   
   

}
