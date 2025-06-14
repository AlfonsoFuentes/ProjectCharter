using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.MainTaskDependencys;

namespace MudBlazorWeb.Pages.ProjectDependant.DeliverableGantTasks;
public partial class GanttDependecyTable
{
    [CascadingParameter]
    public DeliverableGanttTaskResponse Model { get; set; } = new();

    [Parameter]
    public List<DeliverableGanttTaskResponse> Items { get; set; } = new();
    [Parameter,EditorRequired]
    public EventCallback ValidateAsync { get; set; }
    [Parameter]
    public EventCallback RecreateItems { get; set; }
    async Task SelectedItemChanged(MainTaskDependencyResponse selected)
    {
        if (selected.DependencyTask != null)
        {
            selected.DependencyTaskId = selected.DependencyTask!.Id;
            selected.DependencyType = Shared.Enums.TasksRelationTypes.TasksRelationTypeEnum.FinishStart;
            selected.MainTask = Model;
            if (RecreateItems.HasDelegate)
            {
                await RecreateItems.InvokeAsync();
            }

        }


    }
    public async Task Delete(MainTaskDependencyResponse response)
    {
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete dependency from {response.Name}?" },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {
            Model.RemoveDependency(response);
            await ValidateAsync.InvokeAsync();
        }

    }
    

}
