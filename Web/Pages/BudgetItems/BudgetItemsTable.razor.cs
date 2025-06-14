using Microsoft.VisualBasic.FileIO;
using MudBlazor;
using Shared.Models.Brands.Responses;
using Shared.Models.BudgetItems.Exports;
using Shared.Models.BudgetItems.IndividualItems.Alterations.Responses;
using Shared.Models.BudgetItems.IndividualItems.EHSs.Responses;
using Shared.Models.BudgetItems.IndividualItems.Electricals.Responses;
using Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Responses;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Foundations.Responses;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Paintings.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;
using Shared.Models.BudgetItems.IndividualItems.Structurals.Responses;
using Shared.Models.BudgetItems.IndividualItems.Taxs.Responses;
using Shared.Models.BudgetItems.IndividualItems.Testings.Responses;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;
using Shared.Models.BudgetItems.Mappers;
using Shared.Models.BudgetItems.Records;
using Shared.Models.BudgetItems.Requests;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.FileResults;
using Shared.Models.Projects.Reponses;
using System.Linq;
using Web.Pages.Brands;
using Web.Pages.BudgetItems.Alterations;
using Web.Pages.BudgetItems.EHSs;
using Web.Pages.BudgetItems.Electricals;
using Web.Pages.BudgetItems.EngineeringDesigns;
using Web.Pages.BudgetItems.Equipments;
using Web.Pages.BudgetItems.Foundations;
using Web.Pages.BudgetItems.Instruments;
using Web.Pages.BudgetItems.Paintings;
using Web.Pages.BudgetItems.Pipings;
using Web.Pages.BudgetItems.Structurals;
using Web.Pages.BudgetItems.Taxs;
using Web.Pages.BudgetItems.Testings;
using Web.Pages.BudgetItems.Valves;
using Web.Templates;
using static Shared.StaticClasses.StaticClass;

namespace Web.Pages.BudgetItems;
public partial class BudgetItemsTable
{
    [Parameter]
    public ProjectResponse Project { get; set; } = null!;
    public BudgetItemResponseList Response { get; set; } = new();
    public List<IBudgetItemResponse> Items => Response.Items;

    async Task GetAll()
    {
        if (Project == null) return;
        var result = await GenericService.GetAll<BudgetItemResponseList, BudgetItemGetAll>(new BudgetItemGetAll()
        {
            ProjectId = Project.Id,
        });
        if (result.Succeeded)
        {
            Response = result.Data;
        }

    }
    override protected async Task OnParametersSetAsync()
    {
        await GetAll();
    }

    string nameFilter = string.Empty;
    public Func<IBudgetItemResponse, bool> Criteria => x =>
    x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
    x.Tag.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase) ||
    x.Nomenclatore.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<IBudgetItemResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();

    public async Task EditAlterations(AlterationResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<AlterationDialog>
        {
             { x => x.Model, model },
            { x => x.IsEdit, IsEdit   }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<AlterationDialog>("Alteration", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditFoundations(FoundationResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<FoundationDialog>
        {
             { x => x.Model, model },
             { x => x.IsEdit, IsEdit   }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<FoundationDialog>("Foundation", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditStructurals(StructuralResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<StructuralDialog>
        {
           { x => x.Model, model },
           { x => x.IsEdit, IsEdit }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<StructuralDialog>("Structural", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditEquipments(EquipmentResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<EquipmentDialog>
        {
           { x => x.Model, model },
           { x => x.IsEdit, IsEdit }
        };
        MaxWidth width = model.ShowDetails ? MaxWidth.Large : MaxWidth.Medium;

        var options = new DialogOptions() { MaxWidth = width };

        var dialog = await DialogService.ShowAsync<EquipmentDialog>("Equipment", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditValves(ValveResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<ValveDialog>
        {
             { x => x.Model, model },
                { x => x.IsEdit, IsEdit }
        };

        MaxWidth width = model.ShowDetails ? MaxWidth.Large : MaxWidth.Medium;

        var options = new DialogOptions() { MaxWidth = width };

        var dialog = await DialogService.ShowAsync<ValveDialog>("Valve", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditElectricals(ElectricalResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<ElectricalDialog>
        {
            { x => x.Model, model },
            { x => x.IsEdit, IsEdit }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<ElectricalDialog>("Electrical", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditPipes(PipeResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<PipeDialog>
        {
          { x => x.Model, model },
          { x => x.IsEdit, IsEdit }
        };

        MaxWidth width = model.ShowDetails ? MaxWidth.Large : MaxWidth.Medium;

        var options = new DialogOptions() { MaxWidth = width };

        var dialog = await DialogService.ShowAsync<PipeDialog>("Pipe", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditInstruments(InstrumentResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<InstrumentDialog>
        {
            { x => x.Model, model },
            { x => x.IsEdit, IsEdit }
        };

        MaxWidth width = model.ShowDetails ? MaxWidth.Large : MaxWidth.Medium;

        var options = new DialogOptions() { MaxWidth = width };

        var dialog = await DialogService.ShowAsync<InstrumentDialog>("Instrument", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditEHSs(EHSResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<EHSDialog>
        {
           { x => x.Model, model },
            { x => x.IsEdit, IsEdit }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<EHSDialog>("EHS", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditPaintings(PaintingResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<PaintingDialog>
        {
           { x => x.Model, model },
           { x => x.IsEdit, IsEdit }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<PaintingDialog>("Painting", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditTaxs(TaxResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<TaxDialog>
        {
          { x => x.Model, model },
          { x => x.IsEdit, IsEdit }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<TaxDialog>("Tax", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditTestings(TestingResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<TestingDialog>
        {
          { x => x.Model, model },
            { x => x.IsEdit, IsEdit   }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<TestingDialog>("Testing", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task EditEngineeringDesigns(EngineeringDesignResponse model, bool IsEdit = true)
    {

        var parameters = new DialogParameters<EngineeringDesignDialog>
        {
             { x => x.Model, model },
                { x => x.IsEdit, IsEdit }
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<EngineeringDesignDialog>("Engineering Design", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    async Task Edit(IBudgetItemResponse response, bool IsEdit = true)
    {
        switch (response)
        {
            case AlterationResponse alteration:
                await EditAlterations(alteration, IsEdit);
                break;
            case FoundationResponse foundation:
                await EditFoundations(foundation, IsEdit);
                break;
            case StructuralResponse structural:
                await EditStructurals(structural, IsEdit);
                break;
            case EquipmentResponse equipment:
                await EditEquipments(equipment, IsEdit);
                break;
            case ValveResponse valve:
                await EditValves(valve, IsEdit);
                break;
            case ElectricalResponse electrical:
                await EditElectricals(electrical, IsEdit);
                break;
            case PipeResponse pipe:
                await EditPipes(pipe, IsEdit);
                break;
            case InstrumentResponse instrument:
                await EditInstruments(instrument, IsEdit);
                break;
            case EHSResponse ehs:
                await EditEHSs(ehs, IsEdit);
                break;
            case PaintingResponse painting:
                await EditPaintings(painting, IsEdit);
                break;
            case TaxResponse tax:
                await EditTaxs(tax, IsEdit);
                break;
            case TestingResponse testing:
                await EditTestings(testing, IsEdit);
                break;
            case EngineeringDesignResponse engineeringDesign:
                await EditEngineeringDesigns(engineeringDesign, IsEdit);
                break;
        }

    }

    public async Task Delete(IBudgetItemResponse response)
    {
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete {response.Name}? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {

            DeleteBudgetItemRequest request = new()
            {
                Id = response.Id,
                Name = response.Name,

            };
            var resultDelete = await GenericService.Delete(request);
            if (resultDelete.Succeeded)
            {
                await GetAll();
                _snackBar.ShowSuccess(resultDelete.Messages);


            }
            else
            {
                _snackBar.ShowError(resultDelete.Messages);
            }
        }

    }
    HashSet<IBudgetItemResponse> SelecteItems = null!;
    async Task DeleteGroup()
    {
        if (SelecteItems == null) return;
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete this {SelecteItems.Count} Items? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {

            DeleteBudgetItemGroupRequest request = new()
            {
                DeleteGroup = SelecteItems.Select(x => x.Id).ToList(),
                ProjectId = Response.ProjectId,
            };
            var resultDelete = await GenericService.Delete(request);
            if (resultDelete.Succeeded)
            {
                await GetAll();
                _snackBar.ShowSuccess(resultDelete.Messages);


            }
            else
            {
                _snackBar.ShowError(resultDelete.Messages);
            }
        }

    }
    public async Task AddAlterations()
    {
        var parameters = new DialogParameters<AlterationDialog>
        {
            { x => x.Model, new AlterationResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<AlterationDialog>("Alteration", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddFoundations()
    {
        var parameters = new DialogParameters<FoundationDialog>
        {
            { x => x.Model, new FoundationResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<FoundationDialog>("Foundation", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddStructurals()
    {
        var parameters = new DialogParameters<StructuralDialog>
        {
            { x => x.Model, new StructuralResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<StructuralDialog>("Structural", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddEquipments()
    {
        var parameters = new DialogParameters<EquipmentDialog>
        {
            { x => x.Model, new EquipmentResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<EquipmentDialog>("Equipment", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddValves()
    {
        var parameters = new DialogParameters<ValveDialog>
        {
            { x => x.Model, new ValveResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<ValveDialog>("Valve", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddElectricals()
    {
        var parameters = new DialogParameters<ElectricalDialog>
        {
            { x => x.Model, new ElectricalResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<ElectricalDialog>("Electrical", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddPipes()
    {
        var parameters = new DialogParameters<PipeDialog>
        {
            { x => x.Model, new PipeResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<PipeDialog>("Pipe", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddInstruments()
    {
        var parameters = new DialogParameters<InstrumentDialog>
        {
            { x => x.Model, new InstrumentResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<InstrumentDialog>("Instrument", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddEHSs()
    {
        var parameters = new DialogParameters<EHSDialog>
        {
            { x => x.Model, new EHSResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<EHSDialog>("EHS", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddPaintings()
    {
        var parameters = new DialogParameters<PaintingDialog>
        {
            { x => x.Model, new PaintingResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<PaintingDialog>("Painting", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddTaxs()
    {
        var parameters = new DialogParameters<TaxDialog>
        {
            { x => x.Model, new TaxResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<TaxDialog>("Tax", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddTestings()
    {
        var parameters = new DialogParameters<TestingDialog>
        {
            { x => x.Model, new TestingResponse(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<TestingDialog>("Testing", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    public async Task AddEngineeringDesigns()
    {
        var parameters = new DialogParameters<EngineeringDesignDialog>
        {
            { x => x.Model, new EngineeringDesignResponse(){ProjectId=Project.Id } },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<EngineeringDesignDialog>("Engineering Design", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }

    }
    async Task ExportExcel()
    {
        BudgetItemExportGetAll request = new()
        {

            Items = Response.Items.Select(x => x.MapToExport()).ToList(),
            Name = Project.Name,
        };
        var resultExport = await GenericService.GetAll<FileResult, BudgetItemExportGetAll>(request);
        if (resultExport.Succeeded)
        {
            var downloadresult = await blazorDownloadFileService.DownloadFile(resultExport.Data.ExportFileName,
              resultExport.Data.Data, contentType: resultExport.Data.ContentType);
            if (downloadresult.Succeeded)
            {

                _snackBar.ShowSuccess($"{resultExport.Data.ExportFileName} created succesfuly");


            }
        }
    }
    async Task ExportPdf()
    {
        BudgetItemExportGetAll request = new()
        {

            Items = Response.Items.Select(x => x.MapToExport()).ToList(),
            Name = Project.Name,
            ExportFile = Shared.Enums.ExportFiles.ExportFileType.pdf
        };
        var resultExport = await GenericService.GetAll<FileResult, BudgetItemExportGetAll>(request);
        if (resultExport.Succeeded)
        {
            var downloadresult = await blazorDownloadFileService.DownloadFile(resultExport.Data.ExportFileName,
              resultExport.Data.Data, contentType: resultExport.Data.ContentType);
            if (downloadresult.Succeeded)
            {

                _snackBar.ShowSuccess($"{resultExport.Data.ExportFileName} created succesfuly");


            }
        }
    }
}
