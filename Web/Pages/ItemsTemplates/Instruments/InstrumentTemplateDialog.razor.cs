using Blazored.FluentValidation;
using MudBlazor;
using Shared.Enums.Instruments;
using Shared.Enums.NozzleTypes;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.Templates.Instruments.Responses;
using Shared.Models.Templates.NozzleTemplates;
using Web.Pages.Brands;

namespace Web.Pages.ItemsTemplates.Instruments;
public partial class InstrumentTemplateDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    BrandResponseList BrandsResponseList { get; set; } = new();
    async Task GetBrands()
    {
        var result = await GenericService.GetAll<BrandResponseList, BrandGetAll>(new BrandGetAll());
        if (result.Succeeded)
        {
            BrandsResponseList = result.Data;
        }
    }
    protected override async Task OnInitializedAsync()
    {
        await GetBrands();
        StateHasChanged();
    }
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
    public InstrumentTemplateResponse Model { get; set; } = new();

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
    void ChangeVariableInstrument()
    {
        // Actualizar las boquillas según el tipo de válvula
        UpdateNozzlesBasedOnInstrumentType();

        
    }
    

    void UpdateNozzlesBasedOnInstrumentType()
    {
        if (Model.Nozzles.Count == 0)
        {
            AddInitialNozzles();
        }
        else
        {
            AdjustNozzlesForInstrumentType();
        }
    }

    void AddInitialNozzles()
    {
        Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Inlet });

        if (Model.VariableInstrument == VariableInstrumentEnum.MassFlowMeter || Model.VariableInstrument == VariableInstrumentEnum.VolumeFlowMeter)
        {
            Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet });
        }

    }
    void AdjustNozzlesForInstrumentType()
    {
        if (Model.VariableInstrument == VariableInstrumentEnum.MassFlowMeter || Model.VariableInstrument == VariableInstrumentEnum.VolumeFlowMeter)
        {
            if (Model.Nozzles.Count == 1)
            {
                Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet });
            }

        }
        else if (Model.Nozzles.Count == 2)
        {
            Model.Nozzles.Remove(Model.Nozzles.Last());
        }
    }
  
    void ChangeConnectionType()
    {

        if (Model.Nozzles.Count > 0)
        {
            foreach (var nozzle in Model.Nozzles)
            {
                nozzle.ConnectionType = Model.ConnectionType;
            }
        }

    }
    void ChangeDiameter()
    {

        if (Model.Nozzles.Count > 0)
        {
            foreach (var nozzle in Model.Nozzles)
            {
                nozzle.NominalDiameter = Model.Diameter;
            }
        }

    }
}
