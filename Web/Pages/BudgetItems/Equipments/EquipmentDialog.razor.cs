using Blazored.FluentValidation;
using MudBlazor;
using Shared.Models.Brands.Records;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Records;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.Templates.Equipments.Records;
using Shared.Models.Templates.Equipments.Responses;
using Shared.Models.Templates.NozzleTemplates;
using System.Threading.Tasks;
using Web.Pages.Brands;

namespace Web.Pages.BudgetItems.Equipments;
public partial class EquipmentDialog
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;
    DialogOptions _dialogOptions => MudDialog.Options;
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
    EquipmentTemplateResponseList EquipmentTemplateResponseList = new();
    async Task GetAllEquipmentTemplate()
    {
        var result = await GenericService.GetAll<EquipmentTemplateResponseList, EquipmentTemplateGetAll>(new EquipmentTemplateGetAll());
        if (result.Succeeded)
        {
            EquipmentTemplateResponseList = result.Data;
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
    async Task GetEquipmentResponse()
    {
        if (Model.Id != Guid.Empty)
        {
            var result = await GenericService.GetById<EquipmentResponse, GetEquipmentByIdRequest>(
                  new GetEquipmentByIdRequest() { Id = Model.Id });

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
        await GetAllEquipmentTemplate();
        await GetBrands();
        await GetEquipmentResponse();
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
    public EquipmentResponse Model { get; set; } = new();
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
    async Task GetFromTamplateList(EquipmentTemplateResponse response)
    {
        Model.Template = new()
        {
            Brand = response.Brand,
            ExternalMaterial = response.ExternalMaterial,
            Id = response.Id,
            InternalMaterial = response.InternalMaterial,
            Model = response.Model,
            Reference = response.Reference,
            SubType = response.SubType,
            TagLetter = response.TagLetter,
            Type = response.Type,
            Value = response.Value,

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

        if (Model.Template == null || EquipmentTemplateResponseList.Items.Count == 0)
        {
            return;
        }

        // Filtrar las plantillas que coinciden con las propiedades básicas del modelo
        var matchingTemplates = EquipmentTemplateResponseList.Items.Where(template =>
            string.Equals(template.BrandName, Model.Template.BrandName, StringComparison.InvariantCultureIgnoreCase) &&
        template.InternalMaterial.Id == Model.Template.InternalMaterial.Id &&
        template.ExternalMaterial.Id == Model.Template.ExternalMaterial.Id &&
        string.Equals(template.Model, Model.Template.Model, StringComparison.InvariantCultureIgnoreCase) &&
        string.Equals(template.Reference, Model.Template.Reference, StringComparison.InvariantCultureIgnoreCase) &&
        string.Equals(template.Type, Model.Template.Type, StringComparison.InvariantCultureIgnoreCase) &&
        string.Equals(template.SubType, Model.Template.SubType, StringComparison.InvariantCultureIgnoreCase) &&
        string.Equals(template.TagLetter, Model.Template.TagLetter, StringComparison.InvariantCultureIgnoreCase)
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
