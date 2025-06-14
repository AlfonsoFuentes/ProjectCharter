using MudBlazor;
using Shared.Models.Apps.Records;
using Shared.Models.Apps.Requests;
using Shared.Models.Apps.Responses;
using Shared.Models.Projects.Mappers;
using Shared.Models.Projects.Records;
using Shared.Models.Projects.Reponses;
using Shared.Models.Projects.Request;
using Web.Infrastructure.Managers.Projects;
using Web.Templates;
namespace Web.Pages.Projects;
public partial class ProjectsTable
{
    Guid _selectedProjectId;

    public List<ProjectResponse> FilteredItems => ProjectList.Items.Count == 0 ? new() :
        _selectedProjectId == Guid.Empty ? ProjectList.Items : ProjectList.Items.Where(x => x.Id == _selectedProjectId).ToList();
    public ProjectResponseList ProjectList { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        await GetCurrentProject();
        await GetAll();
    }
    public async Task GetAll()
    {

        var result = await GenericService.GetAll<ProjectResponseList, ProjectGetAll>(new ProjectGetAll());
        if (result.Succeeded)
        {
            ProjectList = result.Data;

        }

    }
    async Task GetCurrentProject()
    {
        var result = await GenericService.GetById<GetCurrentProjectResponse, GetCurrentProjectId>(new GetCurrentProjectId());
        if (result.Succeeded)
        {
            _selectedProjectId = result.Data.CurrentProjectId;
        }
    }
    public async Task AddNew()
    {

        var parameters = new DialogParameters<ProjectDialog>
        {

        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<ProjectDialog>("Project", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }
    }
    async Task Edit(ProjectResponse response)
    {


        var parameters = new DialogParameters<ProjectDialog>
        {

            { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<ProjectDialog>("Project", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    async Task Approve(ProjectResponse response)
    {


        var parameters = new DialogParameters<ApproveProjectDialog>
        {

            { x => x.Model, response.ToApprove() },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<ApproveProjectDialog>("Approve Project", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    async Task UnApprove(ProjectResponse response)
    {
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to Un Approve {response.Name}? ." },
            { x => x.ButtonText, "Un Approve" },
            { x => x.Color, Color.Warning }
        };
        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Un Approve", parameters, options);
        var result = await dialog.Result;

        if (!result!.Canceled)
        {
            UnApproveProjectRequest request = new()
            {
                Id = response.Id,
                Name = response.Name,

            };
            var resultUnApprove = await GenericService.Post(request);
            if (resultUnApprove.Succeeded)
            {
                await GetAll();
                _snackBar.ShowSuccess(resultUnApprove.Messages);


            }
            else
            {
                _snackBar.ShowError(resultUnApprove.Messages);
            }
        }
    }
    public async Task Delete(ProjectResponse response)
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
            DeleteProjectRequest request = new()
            {
                Id = response.Id,
                Name = response.Name,

            };
            var resultDelete = await GenericService.Post(request);
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
    [Parameter]
    public EventCallback ExportExcel { get; set; }
    [Parameter]
    public EventCallback ExportPDF { get; set; }
    bool open;
    async Task OnExpandedChanged(ProjectResponse row)
    {

        if (_selectedProjectId == Guid.Empty)
        {
            _selectedProjectId = row.Id;
        }
        else if (_selectedProjectId == row.Id)
        {
            _selectedProjectId = Guid.Empty;
        }
        open = _selectedProjectId != Guid.Empty;
        CreateUpdateAppRequest model = new()
        {
            ProjectId = _selectedProjectId,
        };
        var result = await GenericService.Post(model);

    }
    [Inject]
    private IProjectService ProjectService { get; set; } = null!;

    async Task ExportProjectCharter(ProjectResponse response)
    {

        var resultExport = await ProjectService.ExportProjectCharter(response);
        if (resultExport.Succeeded)
        {

            Console.WriteLine(resultExport.Message);
            var downloadresult = await blazorDownloadFileService.DownloadFile(resultExport.Data.ExportFileName,
              resultExport.Data.Data, contentType: resultExport.Data.ContentType);
            if (downloadresult.Succeeded)
            {
                _snackBar.ShowSuccess($"Project Charter for {response.Name} exported succesfully");

            }
            else
            {
                _snackBar.ShowError($"Project Charter for {response.Name} not exported succesfully");
            }
        }
        else
        {
            _snackBar.ShowError($"Project Charter for {response.Name} not created succesfully");
        }
    }
    async Task ExportProjectExecution(ProjectResponse response)
    {

        var resultExport = await ProjectService.ExportProjectPlann(response);
        if (resultExport.Succeeded)
        {

            Console.WriteLine(resultExport.Message);
            var downloadresult = await blazorDownloadFileService.DownloadFile(resultExport.Data.ExportFileName,
              resultExport.Data.Data, contentType: resultExport.Data.ContentType);
            if (downloadresult.Succeeded)
            {
                _snackBar.ShowSuccess($"Project Charter for {response.Name} exported succesfully");

            }
            else
            {
                _snackBar.ShowError($"Project Charter for {response.Name} not exported succesfully");
            }
        }
        else
        {
            _snackBar.ShowError($"Project Charter for {response.Name} not created succesfully");
        }
    }
}
