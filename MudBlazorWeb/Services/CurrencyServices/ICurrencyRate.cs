namespace MudBlazorWeb.Services.CurrencyServices
{
    public interface ICurrencyRate
    {
        Task<ConversionRate> GetRates(DateTime date);
    }

    public class CurrencyRate : ICurrencyRate
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;

        public CurrencyRate(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }

        public async Task<ConversionRate> GetRates(DateTime date)
        {
            var today = date.Date;

            // 1. Leer desde localStorage
            if (await _localStorage.ContainKeyAsync("cached_rate"))
            {
                var cachedData = await _localStorage.GetItemAsync<CachedRate>("cached_rate");

                if (cachedData != null && cachedData.Date.Date == today.Date)
                {
                    return cachedData.Rates; // Devolver datos del mismo día
                }
                else
                {
                    // 2. Si es de otro día → eliminar el dato antiguo
                    await _localStorage.RemoveItemAsync("cached_rate");
                }
            }

            // 3. Llamar a la API y obtener nuevos datos
            var freshRates = await FetchFromApi(today);

            // 4. Guardar los nuevos datos con la fecha actual
            var cacheEntry = new CachedRate
            {
                Date = today,
                Rates = freshRates
            };

            await _localStorage.SetItemAsync("cached_rate", cacheEntry);

            return freshRates;
        }

        private async Task<ConversionRate> FetchFromApi(DateTime date)
        {
            var client = _httpClientFactory.CreateClient();

            // Construir URL dinámica
            var year = date.Year;
            var month = date.Month < 10 ? $"0{date.Month}" : $"{date.Month}";
            var day = date.Day < 10 ? $"0{date.Day}" : $"{date.Day}";

            var apiKey = "TU_API_KEY_AQUI"; // Mejor mover a IConfiguration
            var dateParam = $"&date={year}-{month}-{day}";
            var baseParams = "&base=USD&currencies=EUR,COP&resolution=1d&amount=1&places=3&format=json";

            var url = $"https://api.fxratesapi.com/historical?api_key={apiKey}{dateParam}{baseParams}";

            try
            {
                var response = await client.GetFromJsonAsync<API_Obj>(url);

                if (response?.success == true)
                {
                    return new ConversionRate
                    {
                        COP = response.rates.COP,
                        EUR = response.rates.EUR
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener tasas: {ex.Message}");
            }

            // Fallback
            return new ConversionRate { COP = 4000, EUR = 1 };
        }
    }

    // Modelos
    public class API_Obj
    {
        public bool success { get; set; }
        public ConversionRate rates { get; set; } = new();
    }

    public class ConversionRate
    {
        public double COP { get; set; }
        public double EUR { get; set; }
    }

    public class CachedRate
    {
        public DateTime Date { get; set; }
        public ConversionRate Rates { get; set; } = new();
    }
    public static class DependencyContainer
    {
        public static IServiceCollection CurrencyRateService(
            this IServiceCollection services)
        {
            services.AddScoped<ICurrencyRate, CurrencyRate>();



            return services;
        }

    }
}
