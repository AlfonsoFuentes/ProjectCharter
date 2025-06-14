using Shared.Enums.CostCenter;
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
using Shared.Models.PurchaseOrders.Responses;
using static Shared.StaticClasses.StaticClass;

namespace Shared.Models.BudgetItems.Responses
{
    public class BudgetItemWithPurchaseOrderResponseList : IResponseAll
    {
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
        public List<EngineeringResponse> Salaries { get; set; } = new();
        public List<ContingencyResponse> Contingencys { get; set; } = new();
        public List<BudgetItemWithPurchaseOrdersResponse> Expenses => [.. Alterations];
        public List<BudgetItemWithPurchaseOrdersResponse> Capital => [..Foundations,..Structurals,..Equipments,..Valves,..Electricals,
            ..Pipings,..Instruments,..EHSs,..Paintings,..Taxes,..Testings,..EngineeringDesigns];
        public List<BudgetItemWithPurchaseOrdersResponse> CapitalPlusEngineeringContingency => [.. Capital, .. Salaries, .. Contingencys];
        public List<BudgetItemWithPurchaseOrdersResponse> SalariesWithPurchaseOrderss => [.. Salaries];
        public List<BudgetItemWithPurchaseOrdersResponse> CapitalPlusContingency => [.. Capital,.. Contingencys];
        public List<BudgetItemWithPurchaseOrdersResponse> Items => BudgetItems.OrderBy(x => x.OrderList).ThenBy(x => x.Nomenclatore).ToList();
        public List<BudgetItemWithPurchaseOrdersResponse> BudgetItems => [.. Expenses, .. Capital,.. Salaries, ..Contingencys];
        public List<BudgetItemWithPurchaseOrdersResponse> OrderedItems => BudgetItems.OrderBy(x => x.OrderList).ThenBy(x => x.Nomenclatore).ToList();
        public double TotalCapital => TotalCapitalWithOutVAT + TaxesBudget;
        public double TotalCapitalPlusEngineeringContingency => CapitalPlusEngineeringContingency.Sum(x => x.BudgetUSD);
        public double TotalCapitalWithOutVAT => Capital.Sum(x => x.BudgetUSD);
        public double TotalExpenses => Expenses.Sum(x => x.BudgetUSD);
        public double PercentageEngineering { get; set; }
        public double PercentageContingency { get; set; }
        public double PercentageTaxes { get; set; }
        public bool IsProductiveAsset { get; set; } = true;

        public CostCenterEnum CostCenter {  get; set; }= CostCenterEnum.None;
        public Guid ProjectId {  get; set; }
        public string ProjectNumber { get; set; }=string.Empty;

        public double EngineeringBudget => TotalCapital * PercentageEngineering / 100;
        public double ContingenyBudget => TotalCapital * PercentageContingency / 100;
        public double TaxesBudget => IsProductiveAsset ? 0 : TotalCapitalWithOutVAT * PercentageTaxes / 100;
        public double TotalBudget => TotalCapital + TotalExpenses + EngineeringBudget + ContingenyBudget;


        public double CapitalActualUSD=> CapitalPlusEngineeringContingency.Sum(x=>x.ActualUSD);
        public double CapitalCommitemntUSD => CapitalPlusEngineeringContingency.Sum(x => x.CommitmentUSD);
        public double CapitalPotentialUSD => CapitalPlusEngineeringContingency.Sum(x => x.PotentialUSD);
        public double CapitalAssignedUSD=>CapitalActualUSD+CapitalCommitemntUSD+CapitalPotentialUSD;
        public double CapitalToCommitUSD => TotalCapitalPlusEngineeringContingency - CapitalAssignedUSD;

        public double ExpensesActualUSD => Expenses.Sum(x => x.ActualUSD);
        public double ExpensesCommitemntUSD => Expenses.Sum(x => x.CommitmentUSD);
        public double ExpensesPotentialUSD => Expenses.Sum(x => x.PotentialUSD);
        public double ExpensesAssignedUSD => ExpensesActualUSD + ExpensesCommitemntUSD + ExpensesPotentialUSD;
        public double ExpensesToCommitUSD => TotalExpenses - ExpensesAssignedUSD;

        public double AppropiationActualUSD => CapitalActualUSD+ ExpensesActualUSD;
        public double AppropiationCommitemntUSD => CapitalCommitemntUSD+ ExpensesCommitemntUSD;
        public double AppropiationPotentialUSD => CapitalPotentialUSD+ ExpensesPotentialUSD;
        public double AppropiationAssignedUSD => CapitalAssignedUSD+ ExpensesAssignedUSD;
        public double AppropiationToCommitUSD => CapitalToCommitUSD+ ExpensesToCommitUSD;
        public List<PurchaseOrderResponse> ProjectPurchaseOrders=> BudgetItems.SelectMany(x=>x.PurchaseOrders).ToList();
    }
}
