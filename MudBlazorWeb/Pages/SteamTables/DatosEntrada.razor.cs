using Caldera;

namespace MudBlazorWeb.Pages.SteamTables;
public partial class DatosEntrada
{
    [CascadingParameter]
    public MassEnergyBalance balance { get; set; } = new();
}
