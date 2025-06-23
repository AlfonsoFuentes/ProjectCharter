using Caldera;

namespace MudBlazorWeb.Pages.SteamTables;
public partial class TableGases
{
    [Parameter]
    public CompoundList CompoundList { get; set; } = null!;

    public List<Compound> FilteredItems => CompoundList == null ? new List<Compound>() : CompoundList.List;

}