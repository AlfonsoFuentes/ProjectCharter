using Blazored.FluentValidation;
using MudBlazor;
using Shared.Enums.Instruments;
using Shared.Enums.NozzleTypes;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Records;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.Templates.Equipments.Responses;
using Shared.Models.Templates.Instruments.Records;
using Shared.Models.Templates.Instruments.Responses;
using Shared.Models.Templates.NozzleTemplates;
using Web.Pages.Brands;

namespace Web.Pages.BudgetItems.Instruments;
public partial class InstrumentDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    private bool Validated { get; set; } = false;
    async Task ValidateAsync()
    {
        Validated = _fluentValidationValidator == null ? false : await _fluentValidationValidator.ValidateAsync(options => { options.IncludeAllRuleSets(); });
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
    InstrumentTemplateResponseList InstrumentTemplateResponseList = new();
    async Task GetAllInstrumentTemplate()
    {
        var result = await GenericService.GetAll<InstrumentTemplateResponseList, InstrumentTemplateGetAll>(new InstrumentTemplateGetAll());
        if (result.Succeeded)
        {
            InstrumentTemplateResponseList = result.Data;
        }
    }
    async Task GetInstrumentResponse()
    {
        if (Model.Id != Guid.Empty)
        {
            var result = await GenericService.GetById<InstrumentResponse, GetInstrumentByIdRequest>(
                  new GetInstrumentByIdRequest() { Id = Model.Id });

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
        await GetAllInstrumentTemplate();
        await GetBrands();
        await GetInstrumentResponse();
        StateHasChanged();
        loaded = true;
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
    public InstrumentResponse Model { get; set; } = new();
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
    async Task GetFromTamplateList(InstrumentTemplateResponse response)
    {
        Model.Template = new()
        {
            Id = response.Id,
            Name = response.Name,
            Brand = response.Brand,
            VariableInstrument = response.VariableInstrument,
            ModifierVariable = response.ModifierVariable,
            Reference = response.Reference,
            Material = response.Material,
            SignalType = response.SignalType,
            ConnectionType = response.ConnectionType,
            Model = response.Model,
            Value = response.Value,
            Diameter = response.Diameter,

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
    void OnChangeTemplate()
    {

        if (Model.Template == null || InstrumentTemplateResponseList.Items.Count == 0)
        {
            return;
        }

        // Filtrar las plantillas que coinciden con las propiedades básicas del modelo
        var matchingTemplates = InstrumentTemplateResponseList.Items.Where(template =>
            string.Equals(template.BrandName, Model.Template.BrandName, StringComparison.InvariantCultureIgnoreCase) &&
        template.Material.Id == Model.Template.Material.Id &&
        string.Equals(template.Model, Model.Template.Model, StringComparison.InvariantCultureIgnoreCase) &&
        string.Equals(template.Reference, Model.Template.Reference, StringComparison.InvariantCultureIgnoreCase)&&
        template.VariableInstrument.Id == Model.Template.VariableInstrument.Id &&
        template.ModifierVariable.Id == Model.Template.ModifierVariable.Id &&
        template.SignalType.Id == Model.Template.SignalType.Id &&
        template.ConnectionType.Id == Model.Template.ConnectionType.Id 
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
    void ChangeConnectionType()
    {
        foreach (var nozzle in Model.Nozzles)
        {
            nozzle.ConnectionType = Model.Template.ConnectionType;
        }
        OnChangeTemplate();
    }
    void ChangeDiameter()
    {
        foreach (var nozzle in Model.Nozzles)
        {
            nozzle.NominalDiameter = Model.Template.Diameter;
        }
        OnChangeTemplate();
    }
    void ChangeVariableInstrument()
    {
        // Actualizar las boquillas según el tipo de válvula
        UpdateNozzlesBasedOnInstrumentType();

        OnChangeTemplate();
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
        Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Inlet });

        if (Model.Template.VariableInstrument == VariableInstrumentEnum.MassFlowMeter || Model.Template.VariableInstrument == VariableInstrumentEnum.VolumeFlowMeter)
        {
            Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet });
        }

    }
    void AdjustNozzlesForInstrumentType()
    {
        if (Model.Template.VariableInstrument == VariableInstrumentEnum.MassFlowMeter || Model.Template.VariableInstrument == VariableInstrumentEnum.VolumeFlowMeter)
        {
            if (Model.Nozzles.Count == 1)
            {
                Model.Nozzles.Add(new NozzleResponse() { Id = Guid.NewGuid(), NozzleType = NozzleTypeEnum.Outlet });
            }

        }
        else if (Model.Nozzles.Count == 2)
        {
            Model.Nozzles.Remove(Model.Nozzles.Last());
        }
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
}
