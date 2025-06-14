namespace Web;
public partial class App
{
    [Inject]
    public IRate _CurrencyService { get; set; } = null!;
    public ConversionRate RateList { get; set; } = null!;
    public string UserId { get; set; }  =string.Empty;  
    protected override async Task OnInitializedAsync()
    {

        RateList = await _CurrencyService.GetRates(DateTime.UtcNow);

    }
}
