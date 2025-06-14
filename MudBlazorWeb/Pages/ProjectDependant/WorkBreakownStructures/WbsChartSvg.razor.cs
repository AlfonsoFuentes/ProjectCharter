using Microsoft.JSInterop;
using Shared.Models.BudgetItems.Responses;
using Shared.Models.DeliverableGanttTasks.Helpers;
using Shared.Models.DeliverableGanttTasks.Records;
using Shared.Models.DeliverableGanttTasks.Responses;
using Shared.Models.Projects.Reponses;

namespace MudBlazorWeb.Pages.ProjectDependant.WorkBreakownStructures;
public partial class WbsChartSvg
{


    [Parameter]
    public ProjectResponse Project { get; set; } = null!;

    DeliverableGanttTaskResponseList Response { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
       
        await GetAll();
        loaded = true;
    }
    bool loaded = false;
    async Task GetAll()
    {
        if (Project == null) return;

        var result = await GenericService.GetAll<DeliverableGanttTaskResponseList, GetAllDeliverableGanttTask>(new GetAllDeliverableGanttTask
        {
            ProjectId = Project.Id,
        });
        if (result.Succeeded)
        {
            Response = result.Data;
            Response.UpdateSubTaskAndDependencies();
        
        }


    }
    private int paddingX = 80; // Espaciado horizontal entre deliverables
    private int boxWidth = 200; // Ancho fijo del rectángulo
    private int lineHeight = 20; // Altura de cada línea de texto
    private int screenWidth = 0; // Ancho de la pantalla
    private DotNetObjectReference<WbsChartSvg> dotNetHelper = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Crear una referencia .NET para manejar eventos desde JavaScript
            dotNetHelper = DotNetObjectReference.Create(this);

            // Obtener el ancho inicial de la pantalla
            screenWidth = await JSRuntime.InvokeAsync<int>("getScreenWidth") - 200;

            // Configurar el listener de cambio de tamaño
            await JSRuntime.InvokeVoidAsync("setupResizeListener", dotNetHelper);

            StateHasChanged(); // Forzar actualización del componente
        }
    }

    [JSInvokable]
    public void UpdateScreenWidth(int newWidth)
    {
        screenWidth = newWidth - 100; // Restar margen lateral
        StateHasChanged(); // Forzar actualización del componente
    }

    private List<int> GetXPositions()
    {
        var positions = new List<int>();
        int currentX = 10; // Posición inicial cercana al borde izquierdo

        foreach (var deliverable in Response.Deliverables)
        {
            positions.Add(currentX);
            currentX += boxWidth + paddingX; // Incrementar la posición horizontal

            // Si el ancho total supera el ancho de la pantalla, ajustar el espaciado
            if (currentX > screenWidth)
            {
                paddingX = Math.Max(paddingX - 10, 20); // Reducir el espaciado mínimo a 20px
                currentX = positions[^1] + boxWidth + paddingX; // Recalcular la posición
            }
        }

        return positions;
    }

    private int GetSvgHeight()
    {
        int totalHeight = 50; // Margen superior inicial

        foreach (var deliverable in Response.Deliverables)
        {
            totalHeight = Math.Max(totalHeight, GetNodeHeight(deliverable) + 10); // Tomar la altura máxima
        }

        return totalHeight + 100;
    }

    private int GetSvgWidtht()
    {
        int totalWidth = 0; // Ancho total inicial

        foreach (var deliverable in Response.Deliverables)
        {
            totalWidth += boxWidth + paddingX; // Sumar el ancho del elemento y el espaciado
        }

        // Restar el último paddingX ya que no es necesario después del último elemento
        totalWidth = Math.Max(totalWidth - paddingX, 0);

        return totalWidth + 250; // Añadir un margen adicional para evitar recortes
    }

    private int GetNodeHeight(DeliverableGanttTaskResponse node)
    {
        int maxHeight = Math.Max(60, node.TextLines(boxWidth).Count * lineHeight + 10);

        if (node.OrderedSubTasks != null && node.OrderedSubTasks.Count > 0)
        {
            foreach (var item in node.OrderedSubTasks)
            {
                maxHeight += GetSubtaskHeight(item) + 10;
            }
        }

        return maxHeight;
    }

    private int GetSubtaskHeight(DeliverableGanttTaskResponse subtask)
    {
        int maxHeight = Math.Max(60, subtask.TextLines(boxWidth).Count * lineHeight + 10);

        if (subtask.OrderedSubTasks != null && subtask.OrderedSubTasks.Count > 0)
        {
            foreach (var child in subtask.OrderedSubTasks)
            {
                maxHeight += GetSubtaskHeight(child) + 10;
            }
        }

        return maxHeight;
    }

    public void Dispose()
    {
        // Limpiar el listener de cambio de tamaño cuando el componente se destruye
        dotNetHelper?.Dispose();
    }

}
