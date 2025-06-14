using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;
using Shared.Models.EngineeringFluidCodes.Records;
using Shared.Models.EngineeringFluidCodes.Responses;
using Web.Pages.Brands;
using static MudBlazor.CategoryTypes;
using Web.Pages.ItemsTemplates.FluidCodes;
using Shared.Models.Templates.Pipings.Records;
using Shared.Models.Templates.Pipings.Responses;
using Shared.Models.BudgetItems.IndividualItems.Valves.Records;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Records;

namespace Web.Pages.BudgetItems.Pipings;
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
    bool loaded = false;
    protected override async Task OnInitializedAsync()
    {
        await GetTemplates();
        await GetFluids();
        await GetBrands();
        await GetPipeResponse();
        StateHasChanged();
        loaded = true;
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
                await OnChageDetails();
            }

        }


    }
    async Task OnChageDetails()
    {
        if (Model.ShowDetails)
        {
            await this.MudDialog.SetOptionsAsync(new DialogOptions() { MaxWidth = MaxWidth.ExtraLarge });
        }
        else
        {
            await this.MudDialog.SetOptionsAsync(new DialogOptions() { MaxWidth = MaxWidth.Medium });
        }
    }
    async Task AddBrand()
    {
        var parameters = new DialogParameters<BrandDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<BrandDialog>("Brand", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetBrands();
            StateHasChanged();
        }
    }
    BrandResponseList BrandsResponseList { get; set; } = new();
    async Task GetBrands()
    {
        var result = await GenericService.GetAll<BrandResponseList, BrandGetAll>(new BrandGetAll());
        if (result.Succeeded)
        {
            BrandsResponseList = result.Data;
        }
    }
    EngineeringFluidCodeResponseList EngineeringFluidCodeResponseList = new();
    async Task GetFluids()
    {

        var result = await GenericService.GetAll<EngineeringFluidCodeResponseList, EngineeringFluidCodeGetAll>(new EngineeringFluidCodeGetAll());
        if (result.Succeeded)
        {
            EngineeringFluidCodeResponseList = result.Data;
        }
    }
    public async Task AddFluid()
    {

        var parameters = new DialogParameters<FluidCodeDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<FluidCodeDialog>("Fluid Code", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetFluids();
            StateHasChanged();
        }
    }
    PipeTemplateResponseList PipeTemplateResponseList = new();
    async Task GetTemplates()
    {
        var result = await GenericService.GetAll<PipeTemplateResponseList, PipeTemplateGetAll>(new PipeTemplateGetAll());
        if (result.Succeeded)
        {
            PipeTemplateResponseList = result.Data;
        }
    }
    async Task GetFromTamplateList(PipeTemplateResponse response)
    {
        Model.Template = new()
        {
            Id = response.Id,
            Name = response.Name,
            Diameter = response.Diameter,
            Class = response.Class,
            Material = response.Material,
            Insulation = response.Insulation,
            EquivalentLenghPrice = response.EquivalentLenghPrice,
            LaborDayPrice = response.LaborDayPrice, 

        };

       

        await ValidateAsync();
    }
}
