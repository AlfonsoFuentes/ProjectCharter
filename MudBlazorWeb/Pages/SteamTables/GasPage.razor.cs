using Caldera;

namespace MudBlazorWeb.Pages.SteamTables;
public partial class GasPage
{
    [CascadingParameter]
    public MassEnergyBalance balance { get; set; } = new();

}
