using Blazored.FluentValidation;
using MudBlazor;
using Shared.Enums.DiameterEnum;
using Shared.Enums.NozzleTypes;
using Shared.Enums.ValvesEnum;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.StakeHolders.Responses;
using Shared.Models.Templates.NozzleTemplates;
using Shared.Models.Templates.Valves.Responses;
using Web.Pages.Brands;

namespace Web.Pages.ItemsTemplates.Valves;
public partial class ValveTemplateDialog
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
    [Parameter]
    public bool EditDiamater { get; set; } = true;
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
    public ValveTemplateResponse Model { get; set; } = new();

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
    void ChangValveType()
    {

        // Actualizar las boquillas según el tipo de válvula
        UpdateNozzlesBasedOnValveType();


    }

    void UpdateNozzlesBasedOnValveType()
    {
        if (Model.Nozzles.Count == 0)
        {
            AddInitialNozzles();
        }
        else
        {
            AdjustNozzlesForValveType();
        }
    }

    void AddInitialNozzles()
    {
        if (Model.Type == ValveTypesEnum.Sample_port)
        {
            Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Inlet, NominalDiameter = Model.Diameter, });
            return;
        }
        Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Inlet, NominalDiameter = Model.Diameter });
        Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Diameter });

        if (Model.Type == ValveTypesEnum.Ball_Three_Way_L || Model.Type == ValveTypesEnum.Ball_Three_Way_T)
        {
            Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Diameter });
        }
        else if(Model.Type == ValveTypesEnum.Diaphragm_Zero_deadLeg || Model.Type == ValveTypesEnum.Ball_Zero_deadLeg)
        {
            Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet });
        }
        else if (Model.Type == ValveTypesEnum.Ball_Four_Way)
        {
            Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Diameter });
            Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Diameter });
        }
    }

    void AdjustNozzlesForValveType()
    {
        if (Model.Type == ValveTypesEnum.Ball_Three_Way_L || Model.Type == ValveTypesEnum.Ball_Three_Way_T)
        {

            if (Model.Nozzles.Count == 2)
            {
                Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Diameter });
            }
            else if (Model.Nozzles.Count == 4)
            {
                Model.Nozzles.Remove(Model.Nozzles.Last());
            }
        }
        else if (Model.Type == ValveTypesEnum.Diaphragm_Zero_deadLeg || Model.Type == ValveTypesEnum.Ball_Zero_deadLeg)
        {
            if (Model.Nozzles.Count == 2)
            {
                Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet });
            }
            else if (Model.Nozzles.Count == 4)
            {
                Model.Nozzles.Remove(Model.Nozzles.Last());
            }
        }
        else if (Model.Type == ValveTypesEnum.Ball_Four_Way)
        {
            if (Model.Nozzles.Count == 2)
            {
                Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Diameter });
                Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Diameter });
            }
            else if (Model.Nozzles.Count == 3)
            {
                Model.Nozzles.Add(new NozzleTemplateResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Diameter });
            }

        }
        else if (Model.Nozzles.Count == 3)
        {
            Model.Nozzles.Remove(Model.Nozzles.Last());
        }
        else if (Model.Nozzles.Count == 4)
        {
            Model.Nozzles.Remove(Model.Nozzles.Last());
            Model.Nozzles.Remove(Model.Nozzles.Last());
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
   
    void ChangeActuator()
    {

        if (Model.ActuatorType.Id == ActuatorTypeEnum.Hand.Id)
        {
            Model.SignalType = SignalTypeEnum.NotApplicable;
            Model.FailType = FailTypeEnum.Not_Applicable;
        }
        if (Model.ActuatorType.Id == ActuatorTypeEnum.Double_effect.Id)
        {

            Model.FailType = FailTypeEnum.Not_Applicable;
        }
    }
    void ChangePositioner()
    {

        if (Model.PositionerType == PositionerTypeEnum.Proportional)
        {
            Model.SignalType = SignalTypeEnum.mA_4_20;
        }
        else
        {
            Model.SignalType = SignalTypeEnum.None;
        }

    }
    void ChangeConnectionType()
    {
        foreach (var nozzle in Model.Nozzles)
        {
            nozzle.ConnectionType = Model.ConnectionType;
        }
      
    }
}
