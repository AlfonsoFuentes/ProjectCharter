﻿using Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.EngineeringItems;
using Server.Database.Entities.ProjectManagements;
using Server.Database.Entities.PurchaseOrders;

namespace Server.DatabaseImplementations.Databases
{
    public class BlazorHeroContext : AuditableContext, IAppDbContext
    {
        private readonly ICurrentUserService _currentUserService;

        private readonly IAppCache _cache;
        public string _tenantId { get; set; }

        public BlazorHeroContext(DbContextOptions<BlazorHeroContext> options, ICurrentUserService currentUserService, IAppCache cache)
            : base(options)
        {
            _currentUserService = currentUserService;

            _tenantId = currentUserService.Email;
            _cache = cache;
        }
        public DbSet<OtherTask> OtherTasks { get; set; } = null!;
        public DbSet<App> Apps { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<BackGround> BackGrounds { get; set; } = null!;
        public DbSet<StakeHolder> StakeHolders { get; set; } = null!;
        public DbSet<Scope> Scopes { get; set; } = null!;
        public DbSet<Objective> Objectives { get; set; } = null!;
        public DbSet<Bennefit> Bennefits { get; set; }
        public DbSet<AcceptanceCriteria> AcceptanceCriterias { get; set; }
        public DbSet<KnownRisk> KnownRisks { get; set; }
        public DbSet<MainTaskDependency> MainTaskDependencys { get; set; }
        public DbSet<NewGanttTask> NewGanttTasks { get; set; } = null!;
        public DbSet<BudgetItemNewGanttTask> BudgetItemNewGantTasks { get; set; }
        public DbSet<MonitoringLog> MonitoringLogs { get; set; } = null!;
        public DbSet<Deliverable> Deliverables { get; set; } = null!;
        public DbSet<Requirement> Requirements { get; set; } = null!;
        public DbSet<Assumption> Assumptions { get; set; } = null!;
        public DbSet<Constrainst> Constrainsts { get; set; } = null!;
        public DbSet<ExpertJudgement> ExpertJudgements { get; set; } = null!;
        public DbSet<RoleInsideProject> RoleInsideProjects { get; set; } = null!;
        public DbSet<Meeting> Meetings { get; set; } = null!;
        public DbSet<MeetingAttendant> MeetingAttendants { get; set; } = null!;
        public DbSet<MeetingAgreement> MeetingAgreements { get; set; } = null!;
        public DbSet<LearnedLesson> LearnedLessons { get; set; } = null!;
        public DbSet<Alteration> Alterations { get; set; } = null!;
        public DbSet<EHS> EHSs { get; set; } = null!;
        public DbSet<Electrical> Electricals { get; set; } = null!;
        public DbSet<Foundation> Foundations { get; set; } = null!;
        public DbSet<Painting> Paintings { get; set; } = null!;
        public DbSet<Structural> Structurals { get; set; } = null!;
        public DbSet<Testing> Testings { get; set; } = null!;
        public DbSet<EngineeringDesign> Engineerings { get; set; } = null!;
        public DbSet<Engineering> EngineeringSalarys { get; set; } = null!;
        public DbSet<Contingency> Contingencys { get; set; } = null!;
        public DbSet<Tax> Taxes { get; set; } = null!;
        public DbSet<TaxesItem> TaxesItems { get; set; } = null!;
        public DbSet<Equipment> Equipments { get; set; } = null!;
        public DbSet<Instrument> Instruments { get; set; } = null!;
        public DbSet<Valve> Valves { get; set; } = null!;
        public DbSet<EquipmentTemplate> EquipmentTemplates { get; set; } = null!;
        public DbSet<InstrumentTemplate> InstrumentTemplates { get; set; } = null!;
        public DbSet<ValveTemplate> ValveTemplates { get; set; } = null!;
        public DbSet<Nozzle> Nozzles { get; set; } = null!;
        public DbSet<NozzleTemplate> NozzleTemplates { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Pipe> Isometrics { get; set; } = null!;
        public DbSet<IsometricItem> IsometricItems { get; set; } = null!;
        public DbSet<PipingAccesory> PipingAccesorys { get; set; } = null!;
        public DbSet<PipingCategory> PipingCategorys { get; set; } = null!;
        public DbSet<EngineeringFluidCode> EngineeringFluidCodes { get; set; } = null!;
        public DbSet<PipeTemplate> PipeTemplates { get; set; } = null!;
        public DbSet<PipingAccesoryImage> PipingAccesoryImages { get; set; } = null!;
        public DbSet<PipingConnectionType> PipingConnectionTypes { get; set; } = null!;
        public DbSet<PipingAccesoryCodeBrand> PipingAccesoryCodeBrands { get; set; } = null!;

        public DbSet<Quality> Qualitys { get; set; } = null!;
        public DbSet<Communication> Communications { get; set; } = null!;
        public DbSet<Resource> Resources { get; set; } = null!;
        public DbSet<Acquisition> Acquisitions { get; set; } = null!;

        public DbSet<DeliverableResource> DeliverableResources { get; set; } = null!;
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; } = null!;
        public DbSet<PurchaseOrderItemReceived> PurchaseOrderItemReceiveds { get; set; } = null!;
        public DbSet<BasicEquipmentItem> BasicEquipmentItems { get; set; } = null!;
        public DbSet<BasicInstrumentItem> BasicInstrumentItems { get; set; } = null!;
        public DbSet<BasicValveItem> BasicValveItems { get; set; } = null!;
        public DbSet<BasicPipeItem> BasicPipeItems { get; set; } = null!;


        void ConfiguerQueryFilters(ModelBuilder builder)
        {
            builder.Entity<App>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Project>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Objective>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<BackGround>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<StakeHolder>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Scope>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Bennefit>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<KnownRisk>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<AcceptanceCriteria>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<DeliverableResource>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<MainTaskDependency>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<NewGanttTask>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<BudgetItemNewGanttTask>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Deliverable>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Quality>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Communication>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Resource>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Acquisition>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Requirement>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Assumption>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<MonitoringLog>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<OtherTask>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Constrainst>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);

            builder.Entity<ExpertJudgement>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<PurchaseOrder>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<PurchaseOrderItem>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<Meeting>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<MeetingAttendant>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<MeetingAgreement>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<PurchaseOrderItemReceived>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<TaxesItem>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<IsometricItem>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);

            builder.Entity<Nozzle>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<BudgetItem>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
            builder.Entity<BasicEngineeringItem>().HasQueryFilter(p => p.IsDeleted == false && EF.Property<string>(p, "TenantId") == _tenantId);
         

            builder.Entity<BudgetItem>().UseTpcMappingStrategy();
        
            builder.Entity<BasicEngineeringItem>().UseTpcMappingStrategy();
            builder.Entity<Template>().UseTpcMappingStrategy();

            builder.Entity<Brand>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Supplier>().HasQueryFilter(p => p.IsDeleted == false);

            builder.Entity<RoleInsideProject>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<LearnedLesson>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<Template>().HasQueryFilter(p => p.IsDeleted == false);

            builder.Entity<NozzleTemplate>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<PipingAccesory>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<PipingCategory>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<EngineeringFluidCode>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<PipingAccesoryCodeBrand>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<PipingConnectionType>().HasQueryFilter(p => p.IsDeleted == false);
            builder.Entity<PipingAccesoryImage>().HasQueryFilter(p => p.IsDeleted == false);


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            ConfigureDatatTypes(builder);

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ConfiguerQueryFilters(builder);


        }

        void ConfigureDatatTypes(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
           .SelectMany(t => t.GetProperties())
           .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.Name is "LastModifiedBy" or "CreatedBy"))
            {
                property.SetColumnType("nvarchar(128)");
            }
        }

        public async Task<int> SaveChangesAndRemoveCacheAsync(params string[] cacheKeys)
        {
            var result = await SaveChangesAsync();

            if (cacheKeys == null) return result;

            foreach (var cacheKey in cacheKeys)
            {
                var key = $"{cacheKey}-{_tenantId}";
                _cache.Remove(key);
            }
            return result;
        }

        public Task<T> GetOrAddCacheAsync<T>(string key, Func<Task<T>> addItemFactory)
        {
            if (_cache == null)
            {
                throw new ArgumentNullException("cache");
            }
            key = $"{key}";
            return _cache.GetOrAddAsync(key, addItemFactory);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(_tenantId))
                {
                    return await base.SaveChangesAsync();
                }
                var AddedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity is ITenantEntity);
                foreach (var item in AddedEntities)
                {
                    var entity = item.Entity as ITenantEntity;
                    entity!.TenantId = _tenantId;

                }
                var entittes = ChangeTracker.Entries<IAuditableEntity>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified).ToList();


                foreach (var row in entittes)
                {
                    if (row.State == EntityState.Added)
                    {
                        row.Entity.CreatedOn = DateTime.Now;
                        row.Entity.CreatedBy = _tenantId;

                    }

                    if (row.State == EntityState.Modified)
                    {
                        row.Entity.LastModifiedOn = DateTime.Now;
                        row.Entity.LastModifiedBy = _tenantId;
                    }

                }


                return await base.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                string exm = ex.Message;
            }

            return 0;
        }


    }
}
