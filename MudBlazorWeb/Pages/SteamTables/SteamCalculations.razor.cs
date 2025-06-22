using Caldera;

namespace MudBlazorWeb.Pages.SteamTables;
public partial class SteamCalculations
{
    MassEnergyBalance balance = new();
    void Calcular()
    {
        balance.Calculate();
    }
}
