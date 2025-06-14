using Shared.Models.DeliverableGanttTasks.Responses;

namespace MudBlazorWeb.Pages.ProjectDependant.WorkBreakownStructures;
public partial class DeliverableNode
{
    [CascadingParameter]
    public WbsChartSvg MainPage { get; set; } = null!;

    [Parameter]
    public DeliverableGanttTaskResponse Node { get; set; } = null!;

    [Parameter]
    public int X { get; set; }

    [Parameter]
    public int Y { get; set; }

    private int BoxWidth { get; set; } = 200;
    private int lineHeight = 20;
    private int boxHeight;

    protected override void OnParametersSet()
    {
        // Calcular la altura del nodo en función del número de líneas de texto
        boxHeight = Math.Max(60, Node.TextLines(BoxWidth).Count * lineHeight + 10);
    }

    private string GenerateSvgText()
    {
        var svgText = new System.Text.StringBuilder();
        int paddingTop = 20; // Padding superior para el texto

        for (int i = 0; i < Node.TextLines(BoxWidth).Count; i++)
        {
            svgText.Append($"<text x=\"{X + 10}\" y=\"{Y + paddingTop + i * lineHeight}\" font-family=\"Arial\" font-size=\"14\" fill=\"#333\">");
            svgText.Append(Node.TextLines(BoxWidth)[i]);
            svgText.Append("</text>");
        }

        return svgText.ToString();
    }

    private int GetSubtaskHeight(DeliverableGanttTaskResponse subtask)
    {
        int maxHeight = Math.Max(60, subtask.TextLines(BoxWidth).Count * lineHeight + 10);

        if (subtask.OrderedSubTasks != null && subtask.OrderedSubTasks.Count > 0)
        {
            foreach (var child in subtask.OrderedSubTasks)
            {
                maxHeight += GetSubtaskHeight(child) + 10;
            }
        }

        return maxHeight;
    }
}
