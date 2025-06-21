using Caldera;
using Caldera.Nueva_clases;

namespace MudBlazorWeb.Pages;
public partial class SteamCalculations
{
    MassEnergyBalance balance = new();
    void Calcular()
    {
        balance.Calculate();
    }
}
