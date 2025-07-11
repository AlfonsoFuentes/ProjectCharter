namespace MudBlazorWeb.Services.CurrencyServices
{
    //public interface ICurrencyRate
    //{
    //    Task<ConversionRate> GetRates(DateTime date);
    //}

    //public class CurrencyRate : ICurrencyRate
    //{
    //    //private readonly IHttpClientFactory _httpClientFactory;
    //    //private readonly ILocalStorageService _localStorage;
    //    private const string CurrencyAPI_KEY = "fxr_live_0717d3622fa4420ae78842bc55544a8b97b9";
    //    private readonly IGenericService http;
    //    public CurrencyRate(IGenericService _http)
    //    {
    //        http = _http;

    //    }
    //    //public async Task<ConversionRate> GetRates2(DateTime date)
    //    //{
    //    //    var today = date.Date;


    //    //    // 1. Leer desde localStorage
    //    //    if (await _localStorage.ContainKeyAsync("cached_rate"))
    //    //    {
    //    //        var cachedData = await _localStorage.GetItemAsync<CachedRate>("cached_rate");

    //    //        if (cachedData != null)
    //    //        {
    //    //            // Si la fecha no coincide → ignorar y borrar
    //    //            if (cachedData.Date.Date != today.Date)
    //    //            {
    //    //                await _localStorage.RemoveItemAsync("cached_rate");
    //    //            }
    //    //            else
    //    //            {
    //    //                // Verificar si es un valor por defecto
    //    //                if (IsDefaultValue(cachedData.Rates))
    //    //                {
    //    //                    // No usar valor por defecto → borrar y recargar
    //    //                    await _localStorage.RemoveItemAsync("cached_rate");
    //    //                }
    //    //                else
    //    //                {
    //    //                    return cachedData.Rates; // Usar dato válido
    //    //                }
    //    //            }
    //    //        }
    //    //    }

    //    //    // 2. Llamar a la API
    //    //    var freshRates = await FetchFromApi(today);

    //    //    // 3. Solo guardar si NO es un valor por defecto
    //    //    if (!IsDefaultValue(freshRates))
    //    //    {
    //    //        var cacheEntry = new CachedRate
    //    //        {
    //    //            Date = today,
    //    //            Rates = freshRates
    //    //        };

    //    //        await _localStorage.SetItemAsync("cached_rate", cacheEntry);
    //    //    }

    //    //    return freshRates;
    //    //}
    //    private bool IsDefaultValue(ConversionRate rate)
    //    {
    //        // Ajusta estos valores según lo que uses como fallback
    //        return rate.COP == 4000 && rate.EUR == 1;
    //    }
    //    public async Task<ConversionRate> GetRates(DateTime date)
    //    {

    //        // 3. Llamar a la API y obtener nuevos datos
    //        var freshRates = await FetchFromApi(date);

    //        // 4. Guardar los nuevos datos con la fecha actual


    //        return freshRates;
    //    }

    //    private async Task<ConversionRate> FetchFromApi(DateTime date)
    //    {


    //        // Construir URL dinámica
    //        var year = date.Year;
    //        var month = date.Month < 10 ? $"0{date.Month}" : $"{date.Month}";
    //        var day = date.Day < 10 ? $"0{date.Day}" : $"{date.Day}";


    //        var dateParam = $"&date={year}-{month}-{day}";
    //        var baseParams = "&base=USD&currencies=EUR,COP&resolution=1d&amount=1&places=3&format=json";

    //        var url = $"https://api.fxratesapi.com/historical?api_key={CurrencyAPI_KEY}{dateParam}{baseParams}";

    //        try
    //        {
    //            API_Obj postmodel = new API_Obj
    //            {
    //                EndPointName = url
    //            };
    //            var response = await http.PostResult(postmodel);

    //            if (response?.Succeeded == true)
    //            {
    //                return new ConversionRate
    //                {
    //                    COP = response.Data.rates.COP,
    //                    EUR = response.Data.rates.EUR
    //                };
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"Error al obtener tasas: {ex.Message}");
    //        }

    //        // Fallback
    //        return new ConversionRate { COP = 4000, EUR = 1 };
    //    }
    //}

    //// Modelos
    //public class API_Obj : IRequest
    //{
    //    public bool success { get; set; }
    //    public ConversionRate rates { get; set; } = new();

    //    public string EndPointName { get; set; } = string.Empty;


    //}

    //public class ConversionRate
    //{
    //    public double COP { get; set; }
    //    public double EUR { get; set; }
    //}

    //public class CachedRate
    //{
    //    public DateTime Date { get; set; }
    //    public ConversionRate Rates { get; set; } = new();
    //}
    //public static class DependencyContainer
    //{
    //    public static IServiceCollection CurrencyRateService(
    //        this IServiceCollection services)
    //    {
    //        services.AddScoped<IRate, Rates>();



    //        return services;
    //    }

    //}
}
