using Server.Database.Entities.BudgetItems;
using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.Database.Entities.ProjectManagements;
using Server.Database.Entities.PurchaseOrders;
using static Shared.StaticClasses.StaticClass;

namespace Server.Interfaces.Database
{
    public interface IAppDbContext
    {
        string _tenantId { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<Project> Projects { get; set; }
  
        DbSet<BackGround> BackGrounds { get; set; }
        DbSet<StakeHolder> StakeHolders { get; set; }
        DbSet<Scope> Scopes { get; set; }
        DbSet<Objective> Objectives { get; set; }
        DbSet<Deliverable> Deliverables { get; set; }
        DbSet<MainTaskDependency> MainTaskDependencys { get; set; }
        DbSet<NewGanttTask> NewGanttTasks { get; set; }
        DbSet<BudgetItemNewGanttTask> BudgetItemNewGantTasks { get; set; }

        DbSet<Requirement> Requirements { get; set; }
        DbSet<Assumption> Assumptions { get; set; }
        DbSet<Constrainst> Constrainsts { get; set; }
        DbSet<Bennefit> Bennefits { get; set; }
        DbSet<AcceptanceCriteria> AcceptanceCriterias { get; set; }
        DbSet<KnownRisk> KnownRisks { get; set; }
        DbSet<ExpertJudgement> ExpertJudgements { get; set; } 

        DbSet<RoleInsideProject> RoleInsideProjects { get; set; }
        DbSet<Meeting> Meetings { get; set; }
        DbSet<MeetingAttendant> MeetingAttendants { get; set; }
        DbSet<MeetingAgreement> MeetingAgreements { get; set; }
        DbSet<LearnedLesson> LearnedLessons { get; set; }

        DbSet<Alteration> Alterations { get; set; }
        DbSet<EHS> EHSs { get; set; }
        DbSet<Electrical> Electricals { get; set; }
        DbSet<Foundation> Foundations { get; set; }
        DbSet<Painting> Paintings { get; set; }
        DbSet<Structural> Structurals { get; set; }
        DbSet<Testing> Testings { get; set; }
       
        DbSet<EngineeringDesign> Engineerings { get; set; }
        DbSet<Tax> Taxes { get; set; }
        DbSet<TaxesItem> TaxesItems { get; set; }
    
        DbSet<Equipment> Equipments { get; set; }
        DbSet<Instrument> Instruments { get; set; }
        DbSet<Valve> Valves { get; set; }

        DbSet<EquipmentTemplate> EquipmentTemplates { get; set; }
        DbSet<InstrumentTemplate> InstrumentTemplates { get; set; }
        DbSet<ValveTemplate> ValveTemplates { get; set; }
        DbSet<Nozzle> Nozzles { get; set; }
        DbSet<NozzleTemplate> NozzleTemplates { get; set; }
        DbSet<Brand> Brands { get; set; }

        DbSet<Pipe> Isometrics { get; set; }
        DbSet<IsometricItem> IsometricItems { get; set; }
        DbSet<PipingAccesory> PipingAccesorys { get; set; }
        DbSet<PipingCategory> PipingCategorys { get; set; }
        DbSet<EngineeringFluidCode> EngineeringFluidCodes { get; set; }
        DbSet<PipeTemplate> PipeTemplates { get; set; }
        DbSet<PipingAccesoryImage> PipingAccesoryImages { get; set; }
        DbSet<PipingConnectionType> PipingConnectionTypes { get; set; }
        DbSet<PipingAccesoryCodeBrand> PipingAccesoryCodeBrands { get; set; }
      
        DbSet<App> Apps { get; set; }
        DbSet<Quality> Qualitys { get; set; }
        DbSet<Communication> Communications { get; set; }
        DbSet<Resource> Resources { get; set; }
        DbSet<Acquisition> Acquisitions { get; set; }
        DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<Engineering> EngineeringSalarys { get; set; }
        DbSet<Contingency> Contingencys { get; set; }
        DbSet<BasicEquipmentItem> BasicEquipmentItems { get; set; }
        DbSet<BasicInstrumentItem> BasicInstrumentItems { get; set; }
        DbSet<BasicValveItem> BasicValveItems { get; set; }
        DbSet<BasicPipeItem> BasicPipeItems { get; set; }
        DbSet<MonitoringLog> MonitoringLogs { get; set; }



        Task<int> SaveChangesAndRemoveCacheAsync(params string[] cacheKeys);
        Task<T> GetOrAddCacheAsync<T>(string key, Func<Task<T>> addItemFactory);
    }
}
