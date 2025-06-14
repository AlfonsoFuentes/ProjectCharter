using Shared.Enums.CostCenter;
using Shared.Enums.ProjectNeedTypes;
using Shared.Models.BudgetItems.IndividualItems.Alterations.Responses;
using Shared.Models.BudgetItems.IndividualItems.Contingencys.Responses;
using Shared.Models.BudgetItems.IndividualItems.EHSs.Responses;
using Shared.Models.BudgetItems.IndividualItems.Electricals.Responses;
using Shared.Models.BudgetItems.IndividualItems.EngineeringDesigns.Responses;
using Shared.Models.BudgetItems.IndividualItems.Engineerings.Responses;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Foundations.Responses;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Paintings.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;
using Shared.Models.BudgetItems.IndividualItems.Structurals.Responses;
using Shared.Models.BudgetItems.IndividualItems.Taxs.Responses;
using Shared.Models.BudgetItems.IndividualItems.Testings.Responses;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;
using Shared.Models.FileResults.Generics.Reponses;

namespace Shared.Models.BudgetItems.Responses
{
    public class BudgetItemResponseList : IResponseAll
    {
        public Guid ProjectId { get; set; }

        public string Name { get; set; } = string.Empty;
        public List<AlterationResponse> Alterations { get; set; } = new();
        public List<FoundationResponse> Foundations { get; set; } = new();
        public List<StructuralResponse> Structurals { get; set; } = new();
        public List<EquipmentResponse> Equipments { get; set; } = new();
        public List<ElectricalResponse> Electricals { get; set; } = new();
        public List<PipeResponse> Pipings { get; set; } = new();
        public List<InstrumentResponse> Instruments { get; set; } = new();
        public List<EHSResponse> EHSs { get; set; } = new();
        public List<PaintingResponse> Paintings { get; set; } = new();
        public List<TaxResponse> Taxes { get; set; } = new();
        public List<TestingResponse> Testings { get; set; } = new();
        public List<ValveResponse> Valves { get; set; } = new();
        public List<EngineeringDesignResponse> EngineeringDesigns { get; set; } = new();
        public List<EngineeringResponse> Engineerings { get; set; } = new();
        public List<ContingencyResponse> Contingencys { get; set; } = new();
        public List<BudgetItemResponse> Expenses => [.. Alterations];
        public List<BudgetItemResponse> Capital => [..Foundations,..Structurals,..Equipments,..Valves,..Electricals,
            ..Pipings,..Instruments,..EHSs,..Paintings,..Taxes,..Testings,..EngineeringDesigns];
        public List<BudgetItemResponse> CapitalPlusEngineeringContingency => [.. Capital, .. Engineerings, .. Contingencys];
        public List<BudgetItemResponse> OrderedItems => BudgetItems.OrderBy(x => x.OrderList).ThenBy(x => x.Nomenclatore).ToList();
        public List<BudgetItemResponse> ItemsPendingToAssignBudget => OrderedItems.Count == 0 ? new() : OrderedItems.Where(x => x.IsAvailableToAssignedToTask).ToList();
        public List<BudgetItemResponse> BudgetItems => [.. Expenses, .. CapitalPlusEngineeringContingency];
        public double TotalCapital => Capital.Sum(x => x.BudgetUSD) + TaxesBudget;

        public double TotalCapitalWithOutVAT => Capital.Sum(x => x.BudgetUSD);
        public double TotalExpenses => Expenses.Sum(x => x.BudgetUSD);
        public double PercentageEngineering { get; set; }
        public double PercentageContingency { get; set; }
        public double PercentageTaxes { get; set; }
        public bool IsProductive { get; set; } = true;
        public CostCenterEnum CostCenter { get; set; } = CostCenterEnum.None;
        public ProjectStatusEnum Status { get; set; } = ProjectStatusEnum.None;
        public string ProjectNumber { get; set; } = string.Empty;
        public double EngineeringBudget => TotalCapital * PercentageEngineering / 100;
        public double ContingenyBudget => TotalCapital * PercentageContingency / 100;
        public double TaxesBudget => IsProductive ? 0 : TotalCapitalWithOutVAT * PercentageTaxes / 100;
        public double TotalBudget => BudgetItems.Sum(x=>x.BudgetUSD);

        public double TotalCapitalPlusEngineeringContingency => CapitalPlusEngineeringContingency.Sum(x => x.BudgetUSD);


    }
}
