using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.MonitoringLogs.Records;
using Shared.Models.MonitoringLogs.Requests;
using Shared.Models.MonitoringLogs.Responses;

namespace MudBlazorWeb.Pages.ProjectDependant.MonitoringLogs;
public partial class TotalProjectsMonitoringLogs
{
    public List<MonitoringLogResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    public Func<MonitoringLogResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<MonitoringLogResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    protected override async Task OnParametersSetAsync()
    {
        await GetAll();
    }
    async Task GetAll()
    {
       
        var result = await GenericService.GetAll<MonitoringLogResponseList, TotalMonitoringLogGetAll>(new TotalMonitoringLogGetAll()
        {
            
        });
        if (result.Succeeded)
        {
            Items = result.Data.Items;
        }
    }
    



    async Task Edit(MonitoringLogResponse response)
    {


        var parameters = new DialogParameters<MonitoringLogDialog>
        {

            { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<MonitoringLogDialog>("MonitoringLog", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    public async Task Delete(MonitoringLogResponse response)
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
            DeleteMonitoringLogRequest request = new()
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
   
   

}
