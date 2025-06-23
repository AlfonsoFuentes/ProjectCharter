using Caldera;

namespace MudBlazorWeb.Pages.SteamTables;
public partial class SummaryPage
{
    [CascadingParameter]
    public MassEnergyBalance balance { get; set; } = new();
}
