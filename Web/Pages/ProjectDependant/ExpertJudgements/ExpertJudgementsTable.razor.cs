using MudBlazor;
using Newtonsoft.Json.Linq;
using Shared.Models.ExpertJudgements.Records;
using Shared.Models.ExpertJudgements.Requests;
using Shared.Models.ExpertJudgements.Responses;
using Shared.Models.Projects.Reponses;
using Shared.StaticClasses;
using Web.Templates;

namespace Web.Pages.ProjectDependant.ExpertJudgements;
public partial class ExpertJudgementsTable
{

    [Parameter]
    public ProjectResponse Project { get; set; } = null!;
    public List<ExpertJudgementResponse> Items { get; set; } = new();
    string nameFilter = string.Empty;
    public Func<ExpertJudgementResponse, bool> Criteria => x => x.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase);
    public List<ExpertJudgementResponse> FilteredItems => string.IsNullOrEmpty(nameFilter) ? Items :
        Items.Where(Criteria).ToList();
    protected override async Task OnParametersSetAsync()
    {
        await GetAll();
    }
    async Task GetAll()
    {
        if (Project == null) return;
        var result = await GenericService.GetAll<ExpertJudgementResponseList, ExpertJudgementGetAll>(new ExpertJudgementGetAll()
        {
            ProjectId = Project.Id, 
        });
        if (result.Succeeded)
        {
            Items = result.Data.Items;
        }
    }
    public async Task AddNew()
    {

        var parameters = new DialogParameters<ExpertJudgementDialog>
        {
            { x => x.Model, new(){ProjectId=Project.Id } },
        };

        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };

        var dialog = await DialogService.ShowAsync<ExpertJudgementDialog>("ExpertJudgement", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
            StateHasChanged();
        }
    }



    async Task Edit(ExpertJudgementResponse response)
    {


        var parameters = new DialogParameters<ExpertJudgementDialog>
        {

             { x => x.Model, response },
        };
        var options = new DialogOptions() { MaxWidth = MaxWidth.Medium };


        var dialog = await DialogService.ShowAsync<ExpertJudgementDialog>("Expert Judgement", parameters, options);
        var result = await dialog.Result;
        if (result != null)
        {
            await GetAll();
        }
    }
    public async Task Delete(ExpertJudgementResponse response)
    {
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete {response.Name}? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {
            DeleteExpertJudgementRequest request = new()
            {
                Id = response.Id,
                Name = response.Name,

            };
            var resultDelete = await GenericService.Post(request);
            if (resultDelete.Succeeded)
            {
                await GetAll();
                _snackBar.ShowSuccess(resultDelete.Messages);


            }
            else
            {
                _snackBar.ShowError(resultDelete.Messages);
            }
        }

    }
    HashSet<ExpertJudgementResponse> SelecteItems = null!;
    public async Task DeleteGroup()
    {
        if (SelecteItems == null) return;
        var parameters = new DialogParameters<DialogTemplate>
        {
            { x => x.ContentText, $"Do you really want to delete this {SelecteItems.Count} Items? This process cannot be undone." },
            { x => x.ButtonText, "Delete" },
            { x => x.Color, Color.Error }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };

        var dialog = await DialogService.ShowAsync<DialogTemplate>("Delete", parameters, options);
        var result = await dialog.Result;


        if (!result!.Canceled)
        {
            DeleteGroupExpertJudgementRequest request = new()
            {
                SelecteItems = SelecteItems,
                ProjectId = Project.Id,

            };
            var resultDelete = await GenericService.Post(request);
            if (resultDelete.Succeeded)
            {
                await GetAll();
                _snackBar.ShowSuccess(resultDelete.Messages);
                SelecteItems = null!;

            }
            else
            {
                _snackBar.ShowError(resultDelete.Messages);
            }
        }

    }


}
