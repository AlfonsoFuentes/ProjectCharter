using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.Database.Entities.PurchaseOrders;

namespace Server.Database.Entities
{
    public class Project : AuditableEntity<Guid>, ITenantEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;


        public DateTime? StartDate { get; set; }

        public static Project Create(int Order)
        {
            return new Project()
            {
                Id = Guid.NewGuid(),
                Order = Order,
                Status = ProjectStatusEnum.Created.Id,
            };
        }
        public int ProjectNeedType { get; set; }
        public int CostCenter { get; set; }
        public int Focus { get; set; }
        public int Status { get; set; }
        public string ProjectNumber { get; set; } = string.Empty;

        public double PercentageEngineering { get; set; }
        public double PercentageContingency { get; set; }
        public double PercentageTaxProductive { get; set; }
        public bool IsProductiveAsset { get; set; } = true;

        #region Scope Management
        public List<BackGround> BackGrounds { get; set; } = new();
        public List<Objective> Objectives { get; set; } = new();
        public List<Requirement> Requirements { get; set; } = new();
        public List<Scope> Scopes { get; set; } = new();
        public List<AcceptanceCriteria> AcceptanceCriterias { get; set; } = new();
        public List<Bennefit> Bennefits { get; set; } = new();
        public List<Constrainst> Constrainsts { get; set; } = new();
        public List<Assumption> Assumptions { get; set; } = new();
        public List<LearnedLesson> LearnedLessons { get; set; } = new();
        public List<ExpertJudgement> ExpertJudgements { get; set; } = new();
        #endregion
        #region Timeline
        public List<Deliverable> Deliverables { get; set; } = new();


        #endregion
        #region Budget
        public List<BudgetItem> BudgetItems { get; set; } = new();
        public List<BasicEngineeringItem> BasicEngineeringItems { get; set; } = new();
        #endregion
        #region Quality
        public List<Quality> Qualitys { get; set; } = new();
        #endregion
        #region Risks
        public List<KnownRisk> KnownRisks { get; set; } = new();
        #endregion
        #region Communication
        public List<Communication> Communications { get; set; } = new();
        #endregion
        #region Resources
        public List<Resource> Resources { get; set; } = new();
        #endregion
        public List<Meeting> Meetings { get; set; } = new();
        public List<PurchaseOrder> PurchaseOrders { get; set; } = new();
        public List<Acquisition> Acquisitions { get; set; } = new();
        public List<StakeHolder> StakeHolders { get; } = [];
        public List<MonitoringLog> MonitoringLogs { get; set; } = new();

        [NotMapped]
        public List<Alteration> Alterations => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Alteration>().ToList();
        [NotMapped]
        public List<Structural> Structurals => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Structural>().ToList();
        [NotMapped]
        public List<Foundation> Foundations => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Foundation>().ToList();
        [NotMapped]
        public List<Equipment> Equipments => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Equipment>().ToList();
        [NotMapped]
        public List<Valve> Valves => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Valve>().ToList();
        [NotMapped]
        public List<Electrical> Electricals => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Electrical>().ToList();
        [NotMapped]
        public List<Pipe> Pipings => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Pipe>().ToList();
        [NotMapped]
        public List<Instrument> Instruments => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Instrument>().ToList();
        [NotMapped]
        public List<EHS> EHSs => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<EHS>().ToList();
        [NotMapped]
        public List<Painting> Paintings => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Painting>().ToList();
        [NotMapped]
        public List<Tax> Taxes => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Tax>().ToList();
        [NotMapped]
        public List<Testing> Testings => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Testing>().ToList();
        [NotMapped]
        public List<Contingency> Contingencys => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Contingency>().ToList();
        [NotMapped]
        public List<Engineering> EngineeringSalaries => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<Engineering>().ToList();
        [NotMapped]
        public List<EngineeringDesign> EngineeringDesigns => BudgetItems == null || BudgetItems.Count == 0 ? new() : BudgetItems.OfType<EngineeringDesign>().ToList();
        [NotMapped]
        public List<BudgetItem> Expenses => [.. Alterations];
        [NotMapped]
        public List<BudgetItem> Capital => [..Foundations,..Structurals,..Equipments,..Valves,..Electricals,
            ..Pipings,..Instruments,..EHSs,..Paintings,..Taxes,..Testings,..EngineeringDesigns];
        [NotMapped]
        public List<BudgetItem> CapitalPlusContingencies => [..Foundations,..Structurals,..Equipments,..Valves,..Electricals,
            ..Pipings,..Instruments,..EHSs,..Paintings,..Taxes,..Testings,..EngineeringDesigns, .. EngineeringContingencys];
        [NotMapped]
        public List<BudgetItem> EngineeringContingencys => [.. EngineeringSalaries, .. Contingencys];
        [NotMapped]
        public List<BudgetItem> Appropiation => [.. Expenses, .. Capital, .. EngineeringContingencys];
        [NotMapped]
        public double CapitalBudgetUSD => Capital == null || Capital.Count == 0 ? 0 : Capital.Sum(x => x.BudgetUSD);
        [NotMapped]
        public double ExpensesBudgetUSD => Expenses == null || Expenses.Count == 0 ? 0 : Expenses.Sum(x => x.BudgetUSD);
        [NotMapped]
        public double CompleteCapitalBudgetUSD => CapitalBudgetUSD + EngineeringContingencysBudgetUSD;
        [NotMapped]
        public double EngineeringContingencysBudgetUSD => EngineeringContingencys == null || EngineeringContingencys.Count == 0 ? 0 : EngineeringContingencys.Sum(x => x.BudgetUSD);
        [NotMapped]
        public double AppropiationBudgetUSD => CompleteCapitalBudgetUSD + ExpensesBudgetUSD;

        [NotMapped]
        public List<BasicEquipmentItem> BasicEquipmentItems => BasicEngineeringItems == null || BasicEngineeringItems.Count == 0 ? new() : BasicEngineeringItems.OfType<BasicEquipmentItem>().ToList();
        [NotMapped]
        public List<BasicInstrumentItem> BasicInstrumentItems => BasicEngineeringItems == null || BasicEngineeringItems.Count == 0 ? new() : BasicEngineeringItems.OfType<BasicInstrumentItem>().ToList();
        [NotMapped]
        public List<BasicValveItem> BasicValveItems => BasicEngineeringItems == null || BasicEngineeringItems.Count == 0 ? new() : BasicEngineeringItems.OfType<BasicValveItem>().ToList();
        [NotMapped]
        public List<BasicPipeItem> BasicPipeItem => BasicEngineeringItems == null || BasicEngineeringItems.Count == 0 ? new() : BasicEngineeringItems.OfType<BasicPipeItem>().ToList();
        [NotMapped]
        public List<BasicEngineeringItem> ProcessDiagramComponents => [.. BasicEquipmentItems, .. BasicInstrumentItems, .. BasicValveItems, .. BasicPipeItem];

    }


}
