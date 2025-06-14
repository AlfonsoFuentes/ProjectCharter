using MudBlazor;
using Shared.ExtensionsMetods;
using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.FileResults.Generics.Reponses;

namespace MudBlazorWeb.Pages.ProjectDependant.DeliverableGantTasks;
public partial class BudgetSubItemDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public List<BasicResponse> Items { get; set; } = new();


    public HashSet<BasicResponse> SelectedItems { get; set; } = new();

    public bool Disabled => SelectedItems.Count == 0;

    protected override void OnInitialized()
    {
        SelectedItems=Items.Where(x=>x.Selected).ToHashSet();
    }
    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(SelectedItems.ToList()));
    }
    void Cancel()
    {
        MudDialog.Close(DialogResult.Ok(false));
    }
    double BudgetSelected => SelectedItems.Count == 0 ? 0 : SelectedItems.Sum(x => x.BudgetUSD);
    string sBudgetSelected => BudgetSelected.ToCurrencyCulture();
}


