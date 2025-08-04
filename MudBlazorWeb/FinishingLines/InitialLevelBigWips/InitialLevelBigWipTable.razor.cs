using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.FinishingLines.InitialLevelBigWips;
using Shared.Models.FinishingLines.ProductionLineAssignments;

namespace MudBlazorWeb.FinishingLines.InitialLevelBigWips;
public partial class InitialLevelBigWipTable
{
    [Parameter]
    public List<InitialLevelBigWipResponse> Items { get; set; } = new();
    [Parameter]
    public EventCallback<List<InitialLevelBigWipResponse>> ItemsChanged { get; set; }
    [Parameter]
    public EventCallback GetAll { get; set; }
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    [Parameter]
    public Guid PlanId { get; set; } = Guid.Empty;
    string nameFilter = string.Empty;
    public Func<InitialLevelBigWipResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<InitialLevelBigWipResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    //protected override async Task OnInitializedAsync()
    //{
    //    await GetAll();
    //}
    //async Task GetAll()
    //{
    //    var result = await GenericService.GetAll<InitialLevelBigWipResponseList, InitialLevelBigWipGetAll>(new InitialLevelBigWipGetAll());
    //    if (result.Succeeded)
    //    {
    //        Items = result.Data.Items;
    //    }
    //}
    //public async Task AddNew()
    //{

    //    var parameters = new DialogParameters<InitialLevelBigWipDialog>
    //    {

    //    };

    //    var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

    //    var dialog = await DialogService.ShowAsync<InitialLevelBigWipDialog>("InitialLevelBigWip", parameters, options);
    //    var result = await dialog.Result;
    //    if (result != null && !result.Canceled)
    //    {
    //        await GetAll.InvokeAsync();
    //        StateHasChanged();
    //    }
    //}
    async Task Edit(InitialLevelBigWipResponse response)
    {


        var parameters = new DialogParameters<InitialLevelBigWipDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };


        var dialog = await DialogService.ShowAsync<InitialLevelBigWipDialog>("InitialLevelBigWip", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if (PlanId != Guid.Empty)
            {
                await GetAll.InvokeAsync();
            }
            else
            {
                Items.Add(response);
                await ItemsChanged.InvokeAsync(Items);
                await ValidateAsync.InvokeAsync();
            }
            
        }
    }
    //public async Task Delete(InitialLevelBigWipResponse response)
    //{
    //    var parameters = new DialogParameters<DialogTemplate>
    //    {
    //        { x => x.ContentText, $"Do you really want to delete {response.Name}? This process cannot be undone." },
    //        { x => x.ButtonText, "Delete" },
    //        { x => x.Color, Color.Error }
    //    };

    //    var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

    //    var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
    //    var result = await dialog.Result;


    //    if (!result!.Canceled)
    //    {
    //        DeleteInitialLevelBigWipRequest request = new()
    //        {
    //            Id = response.Id,
    //            Name = response.Name,

    //        };
    //        var resultDelete = await GenericService.Post(request);
    //        if (resultDelete.Succeeded)
    //        {
    //            await GetAll();
    //            _snackBar.ShowSuccess(resultDelete.Messages);


    //        }
    //        else
    //        {
    //            _snackBar.ShowError(resultDelete.Messages);
    //        }
    //    }

    //}
    HashSet<InitialLevelBigWipResponse> SelecteItems = null!;
   
}
