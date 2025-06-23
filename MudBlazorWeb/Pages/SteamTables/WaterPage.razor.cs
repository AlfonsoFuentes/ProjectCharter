using Caldera;

namespace MudBlazorWeb.Pages.SteamTables;
public partial class WaterPage
{
    [CascadingParameter]
    public MassEnergyBalance balance { get; set; } = new();
}
