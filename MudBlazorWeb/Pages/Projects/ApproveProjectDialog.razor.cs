using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.Projects.Mappers;
using Shared.Models.Projects.Reponses;
using Shared.Models.Projects.Request;

namespace MudBlazorWeb.Pages.Projects;
public partial class ApproveProjectDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    [Parameter]
    public ApproveProjectRequest Model { get; set; } = new();
    public BudgetItemResponseList Response { get; set; } = new();
    async Task GetBudgetItems()
    {
    
        var result = await GenericService.GetAll<BudgetItemResponseList, BudgetItemGetAll>(new BudgetItemGetAll()
        {
            ProjectId = Model.Id,
        });
        if (result.Succeeded)
        {
            Response = result.Data;
            Model.CapitalUSD = Response.Capital.Sum(x => x.BudgetUSD);
            Model.ExpensesUSD = Response.Expenses.Sum(x => x.BudgetUSD);
        }

    }
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }
   

    FluentValidationValidator _fluentValidationValidator = null!;
    protected override async Task OnInitializedAsync()
    {
        await GetBudgetItems();
       
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

   
}
