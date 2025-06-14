namespace Shared.Models.DeliverableGanttTasks.Helpers
{
    public static class TextHelper
    {
        // Dividir el texto en líneas basadas en un ancho máximo
        public static List<string> SplitTextIntoLines(string text, int maxWidth, int averageCharWidth = 7)
        {
            var lines = new List<string>();
            var words = text.Split(' ');
            var currentLine = "";

            foreach (var word in words)
            {
                // Estimar el ancho de la línea actual más la nueva palabra
                var testLine = string.IsNullOrEmpty(currentLine) ? word : $"{currentLine} {word}";
                var testWidth = testLine.Length * averageCharWidth;

                if (testWidth > maxWidth)
                {
                    // Si excede el ancho máximo, agregar la línea actual y comenzar una nueva
                    lines.Add(currentLine);
                    currentLine = word;
                }
                else
                {
                    // Si cabe, agregar la palabra a la línea actual
                    currentLine = testLine;
                }
            }

            // Agregar la última línea si no está vacía
            if (!string.IsNullOrEmpty(currentLine))
            {
                lines.Add(currentLine);
            }

            return lines;
        }
    }
}
