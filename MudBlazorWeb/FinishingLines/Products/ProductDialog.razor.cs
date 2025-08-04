using Blazored.FluentValidation;
using MudBlazor;
using MudBlazorWeb.FinishingLines.BackBones;
using Shared.Models.FinishingLines.BackBones;
using Shared.Models.FinishingLines.Products;
using Shared.Models.FinishingLines.SKUs;

namespace MudBlazorWeb.FinishingLines.Products;
public partial class ProductDialog
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
    public ProductResponse Model { get; set; } = new();

    BackBoneResponseList BackBoneResponseList { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        await GetAllBackBones();

    }
    async Task GetAllBackBones()
    {
        var result = await GenericService.GetAll<BackBoneResponseList, BackBoneGetAll>(new BackBoneGetAll());
        if (result.Succeeded)
        {
            BackBoneResponseList = result.Data;
        }
    }
    private Task<IEnumerable<BackBoneResponse>> SearchBackBone(string value, CancellationToken token)
    {
        Func<BackBoneResponse, bool> Criteria = x =>
        x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase)
        ;
        IEnumerable<BackBoneResponse> FilteredItems = string.IsNullOrEmpty(value) ? BackBoneResponseList.Items.AsEnumerable() :
             BackBoneResponseList.Items.Where(Criteria);
        return Task.FromResult(FilteredItems);
    }
    public async Task AddBackBone()
    {

        var parameters = new DialogParameters<BackBoneDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<BackBoneDialog>("BackBone", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAllBackBones();

        }
    }
    async Task Normalize()
    {
        double sum = Model.Components.Sum(x => x.Percentage);
        foreach (var component in Model.Components)
        {
            component.Percentage = Math.Round((component.Percentage / sum) * 100, 2);
        }
        await ValidateAsync();
    }
    async Task getById()
    {
        if (Model.Id == Guid.Empty)
        {
            return;
        }
        var result = await GenericService.GetById<ProductResponse, GetProductByIdRequest>(new()
        {
            Id = Model.Id
        });
        if (result.Succeeded)
        {
            Model = result.Data;
        }
    }
}
