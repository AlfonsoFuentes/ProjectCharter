using MudBlazor;
using MudBlazorWeb.Templates;
using Shared.Models.FinishingLines.InitialLevelWips;
using Shared.Models.FinishingLines.ProductionLineAssignments;

namespace MudBlazorWeb.FinishingLines.InitialLevelWips;
public partial class InitialLevelWipTable
{
    [Parameter]
    public List<InitialLevelWipResponse> Items { get; set; } = new();
    [Parameter]
    public EventCallback<List<InitialLevelWipResponse>> ItemsChanged { get; set; }
    [Parameter]
    public EventCallback GetAll { get; set; }
    [Parameter]
    public EventCallback ValidateAsync { get; set; }
    [Parameter]
    public Guid ProductionLineAssigmentId { get; set; } = Guid.Empty;
    string nameFilter = string.Empty;
    public Func<InitialLevelWipResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<InitialLevelWipResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    //protected override async Task OnInitializedAsync()
    //{
    //    await GetAll();
    //}
    //async Task GetAll()
    //{
    //    var result = await GenericService.GetAll<InitialLevelWipResponseList, InitialLevelWipGetAll>(new InitialLevelWipGetAll());
    //    if (result.Succeeded)
    //    {
    //        Items = result.Data.Items;
    //    }
    //}
    //public async Task AddNew()
    //{

    //    var parameters = new DialogParameters<InitialLevelWipDialog>
    //    {

    //    };

    //    var options = new DialogOptions() { MaxWidth = MaxWidth.Small };

    //    var dialog = await DialogService.ShowAsync<InitialLevelWipDialog>("InitialLevelWip", parameters, options);
    //    var result = await dialog.Result;
    //    if (result != null && !result.Canceled)
    //    {
    //        await GetAll.InvokeAsync();
    //        StateHasChanged();
    //    }
    //}
    async Task Edit(InitialLevelWipResponse response)
    {


        var parameters = new DialogParameters<InitialLevelWipDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Small };


        var dialog = await DialogService.ShowAsync<InitialLevelWipDialog>("InitialLevelWip", parameters, options);
        var result = await dialog.Result;
        if (result != null && !result.Canceled)
        {
            if(ProductionLineAssigmentId != Guid.Empty)
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
    //public async Task Delete(InitialLevelWipResponse response)
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
    //        DeleteInitialLevelWipRequest request = new()
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
    HashSet<InitialLevelWipResponse> SelecteItems = null!;
   
}
