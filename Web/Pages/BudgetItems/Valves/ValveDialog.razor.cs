using Blazored.FluentValidation;
using MudBlazor;
using Shared.Enums.NozzleTypes;
using Shared.Enums.ValvesEnum;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.BudgetItems.IndividualItems.Valves.Records;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;
using Shared.Models.Templates.Equipments.Responses;
using Shared.Models.Templates.Valves.Records;
using Shared.Models.Templates.Valves.Responses;
using Web.Pages.Brands;

namespace Web.Pages.BudgetItems.Valves;
public partial class ValveDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
    }


    FluentValidationValidator _fluentValidationValidator = null!;
    BrandResponseList BrandsResponseList { get; set; } = new();
    async Task GetBrands()
    {
        var result = await GenericService.GetAll<BrandResponseList, BrandGetAll>(new BrandGetAll());
        if (result.Succeeded)
        {
            BrandsResponseList = result.Data;
        }
    }
    ValveTemplateResponseList ValveTemplateResponseList = new();
    async Task GetAllValveTemplate()
    {
        var result = await GenericService.GetAll<ValveTemplateResponseList, ValveTemplateGetAll>(new ValveTemplateGetAll());
        if (result.Succeeded)
        {
            ValveTemplateResponseList = result.Data;
        }
    }
    async Task GetValveResponse()
    {
        if (Model.Id != Guid.Empty)
        {
            var result = await GenericService.GetById<ValveResponse, GetValveByIdRequest>(
                  new GetValveByIdRequest() { Id = Model.Id });

            if (result.Succeeded)
            {
                Model = result.Data;
                await OnChageDetails();
            }

        }


    }
    bool loaded = false;
    protected override async Task OnInitializedAsync()
    {
        await GetAllValveTemplate();
        await GetBrands();
        await GetValveResponse();
        StateHasChanged();
        loaded = true;
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

    [Parameter]
    public ValveResponse Model { get; set; } = new();
    [Parameter]
    public bool IsEdit { get; set; } = true;
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
    async Task GetFromTamplateList(ValveTemplateResponse response)
    {
        Model.Template = new()
        {
            Diameter = response.Diameter,
            Brand = response.Brand,
            ActuatorType = response.ActuatorType,
            FailType = response.FailType,
            HasFeedBack = response.HasFeedBack,
            Material = response.Material,
            Model = response.Model,
            PositionerType = response.PositionerType,
            SignalType = response.SignalType,
            Type = response.Type,
            Id = response.Id,
            Value = response.Value,
            ConnectionType = response.ConnectionType,
        };

       
        Model.Nozzles = response.Nozzles.Select((row, index) => new NozzleResponse
        {
            Order = index + 1,
            Id = Guid.NewGuid(),
            ConnectionType = row.ConnectionType,
            NominalDiameter = row.NominalDiameter,
            NozzleType = row.NozzleType
        }).ToList();
        Model.BudgetUSD = response.Value;

        await ValidateAsync();
    }
    void ChangValveType()
    {

        // Actualizar las boquillas según el tipo de válvula
        UpdateNozzlesBasedOnValveType();
        OnChangeTemplate();

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
        if(Model.Template.Type== ValveTypesEnum.Sample_port)
        {
            Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Inlet, NominalDiameter = Model.Template.Diameter, });
            return;
        }
        Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Inlet, NominalDiameter = Model.Template.Diameter,  });
        Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Template.Diameter, });

        if (Model.Template.Type == ValveTypesEnum.Ball_Three_Way_L || Model.Template.Type == ValveTypesEnum.Ball_Three_Way_T)
        {
            Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Template.Diameter,  });
        }
        else if (Model.Template.Type == ValveTypesEnum.Diaphragm_Zero_deadLeg || Model.Template.Type == ValveTypesEnum.Ball_Zero_deadLeg)
        {
            Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet,  });
        }
        else if (Model.Template.Type == ValveTypesEnum.Ball_Four_Way)
        {
            Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Template.Diameter, });
            Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Template.Diameter,  });
        }
    }

    void AdjustNozzlesForValveType()
    {
        if (Model.Template.Type == ValveTypesEnum.Ball_Three_Way_L || Model.Template.Type == ValveTypesEnum.Ball_Three_Way_T)
        {

            if (Model.Nozzles.Count == 2)
            {
                Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Template.Diameter });
            }
            else if (Model.Nozzles.Count == 4)
            {
                Model.Nozzles.Remove(Model.Nozzles.Last());
            }
        }
        else if (Model.Template.Type == ValveTypesEnum.Diaphragm_Zero_deadLeg || Model.Template.Type == ValveTypesEnum.Ball_Zero_deadLeg)
        {
            if (Model.Nozzles.Count == 2)
            {
                Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet });
            }
            else if (Model.Nozzles.Count == 4)
            {
                Model.Nozzles.Remove(Model.Nozzles.Last());
            }
        }
        else if (Model.Template.Type == ValveTypesEnum.Ball_Four_Way)
        {
            if (Model.Nozzles.Count == 2)
            {
                Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Template.Diameter });
                Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Template.Diameter });
            }
            else if (Model.Nozzles.Count == 3)
            {
                Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet, NominalDiameter = Model.Template.Diameter });
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
                nozzle.NominalDiameter = Model.Template.Diameter;
            }
        }
        OnChangeTemplate();
    }
   
    void ChangeActuator()
    {

        if (Model.Template.ActuatorType.Id == ActuatorTypeEnum.Hand.Id)
        {
            Model.Template.SignalType = SignalTypeEnum.NotApplicable;
            Model.Template.FailType = FailTypeEnum.Not_Applicable;
        }
        if (Model.Template.ActuatorType.Id == ActuatorTypeEnum.Double_effect.Id)
        {

            Model.Template.FailType = FailTypeEnum.Not_Applicable;
        }
        OnChangeTemplate(); 
    }
    void ChangePositioner()
    {

        if (Model.Template.PositionerType == PositionerTypeEnum.Proportional)
        {
            Model.Template.SignalType = SignalTypeEnum.mA_4_20;
        }
        else
        {
            Model.Template.SignalType = SignalTypeEnum.None;
        }
        OnChangeTemplate(); 
    }
    async Task OnChageDetails()
    {
        if (Model.ShowDetails)
        {
            await this.MudDialog.SetOptionsAsync(new DialogOptions() { MaxWidth = MaxWidth.ExtraExtraLarge });
        }
        else
        {
            await this.MudDialog.SetOptionsAsync(new DialogOptions() { MaxWidth = MaxWidth.Medium });
        }
    }
    void ChangeConnectionType()
    {
        foreach (var nozzle in Model.Nozzles)
        {
            nozzle.ConnectionType = Model.Template.ConnectionType;
        }
        OnChangeTemplate();
    }
    void OnChangeTemplate()
    {

        if (Model.Template == null || ValveTemplateResponseList.Items.Count == 0)
        {
            return;
        }

        // Filtrar las plantillas que coinciden con las propiedades básicas del modelo
        var matchingTemplates = ValveTemplateResponseList.Items.Where(template =>
            string.Equals(template.BrandName, Model.Template.BrandName, StringComparison.InvariantCultureIgnoreCase) &&
            template.Material.Id == Model.Template.Material.Id &&
            template.ActuatorType.Id == Model.Template.ActuatorType.Id &&
            template.FailType.Id == Model.Template.FailType.Id &&
            template.SignalType.Id == Model.Template.SignalType.Id &&
            template.PositionerType.Id == Model.Template.PositionerType.Id &&
            template.Type.Id == Model.Template.Type.Id &&
            template.Diameter.Id == Model.Template.Diameter.Id &&
             string.Equals(template.Model, Model.Template.Model, StringComparison.InvariantCultureIgnoreCase)
            
        ).ToList();

        // Si no hay coincidencias, establecer Id a Guid.Empty
        if (matchingTemplates.Count == 0)
        {
            Model.Template.Id = Guid.Empty;
            return;
        }

        // Buscar una coincidencia exacta en las boquillas (nozzles)
        foreach (var template in matchingTemplates)
        {
            // Verificar si todas las boquillas del template coinciden con las del modelo
            bool allNozzlesMatch = template.Nozzles.All(nozzle =>
                Model.Nozzles.Any(modelNozzle =>
                    modelNozzle.ConnectionType.Id == nozzle.ConnectionType.Id &&
                    modelNozzle.NominalDiameter.Id == nozzle.NominalDiameter.Id &&
                    modelNozzle.NozzleType.Id == nozzle.NozzleType.Id
                )
            );

            if (allNozzlesMatch)
            {
                // Asignar el Id del template coincidente y el valor del presupuesto
                Model.Template.Id = template.Id;
                Model.BudgetUSD = template.Value;
                return; // Salir del bucle una vez que se encuentra una coincidencia
            }
        }

        // Si no se encuentra ninguna coincidencia, establecer Id a Guid.Empty
        Model.Template.Id = Guid.Empty;


    }
}
