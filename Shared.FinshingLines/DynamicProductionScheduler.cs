// Archivo: DynamicProductionScheduler.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Shared.FinshingLines.DPSimulation; // Ajusta el namespace según tu estructura
using Shared.FinshingLines.DPSimulation.Mixers;
using Shared.FinshingLines.DPSimulation.Lines;
using Shared.FinshingLines.DPSimulation.WIPs;

namespace Shared.FinshingLines.DPSimulation.Schedulers
{
  
    //public class DynamicProductionScheduler
    //{
    //    private readonly List<LineContext> _productionLines = null!;
    //    private bool _initialSchedulingDone = false;

    //    // Definición de grupos de backbones que requieren limpieza extensa al cambiar entre ellos
    //    private HashSet<Guid> _tripleAccionGroup = null!; // Blanco, Azul, Verde
    //    private HashSet<Guid> _extraBlancuraGroup = null!; // Blanco, Azul Claro, Azul Oscuro
    //    private HashSet<Guid> _kolynosMentaGroup = null!; // Kolynos, Menta

    //    // Priorización de mezcladores para grupos específicos (basado en tu ejemplo)
    //    private Dictionary<Guid, string> _preferredMixerForBackbone = null!; // BackboneId -> Nombre de Mixer preferido

    //    private const double LOOKAHEAD_HOURS = 2.0;
    //    private const double INITIAL_LOOKAHEAD_HOURS = 1.0; // Para planificación inicial
    //    private const double MINIMUM_FILL_RATIO = 0.20;
    //    private const double CRITICAL_FILL_RATIO = 0.2;
    //    private const double OVERSTOCK_RATIO = 0.95;
    //    private const double DYNAMIC_SCHEDULING_INTERVAL = 30; // Minutos

    //    private Dictionary<Guid, double> _lastKnownLevels = new Dictionary<Guid, double>();
    
    //    NewSimulationEngine _engine = null!;
    //    public DynamicProductionScheduler(NewSimulationEngine engine, ReadSimulationFromDatabase config)
    //    {
    //        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
         
    //        _productionLines = engine.Lines ?? throw new ArgumentNullException(nameof(engine.Lines));

    //        InitializeBackboneGroups();
    //        InitializeMixerPreferences();
    //    } /// <summary>
    //      /// Inicializa los grupos de backbones para la lógica de limpieza extensa.
    //      /// </summary>
    //    private void InitializeBackboneGroups()
    //    {
    //        var tripleAccionBackbones =_engine.Database.BackBones
    //            .Where(bb => bb.BackBoneName.Contains("Triple Accion"))
    //            .Select(bb => bb.BackBoneId)
    //            .ToHashSet();
    //        _tripleAccionGroup = tripleAccionBackbones;

    //        var extraBlancuraBackbones = _engine.Database.BackBones
    //            .Where(bb => bb.BackBoneName.Contains("Extrablancura"))
    //            .Select(bb => bb.BackBoneId)
    //            .ToHashSet();
    //        _extraBlancuraGroup = extraBlancuraBackbones;

    //        var kolynosMentaBackbones = _engine.Database.BackBones
    //            .Where(bb => bb.BackBoneName == "Kolynos" || bb.BackBoneName == "Menta")
    //            .Select(bb => bb.BackBoneId)
    //            .ToHashSet();
    //        _kolynosMentaGroup = kolynosMentaBackbones;
    //    }

    //    /// <summary>
    //    /// Inicializa las preferencias de mezcladores para backbones específicos.
    //    /// </summary>
    //    private void InitializeMixerPreferences()
    //    {
    //        _preferredMixerForBackbone = new Dictionary<Guid, string>();

    //        // Preferir Brogly para Triple Accion Azul
    //        var tripleAccionAzul = _engine.Database.BackBones
    //            .FirstOrDefault(bb => bb.BackBoneName == "Triple Accion Azul");
    //        if (tripleAccionAzul != null)
    //        {
    //            _preferredMixerForBackbone[tripleAccionAzul.BackBoneId] = "Brogly";
    //        }

    //        // Preferir Fryma para Triple Accion Verde
    //        var tripleAccionVerde = _engine.Database.BackBones
    //            .FirstOrDefault(bb => bb.BackBoneName == "Triple Accion Verde");
    //        if (tripleAccionVerde != null)
    //        {
    //            _preferredMixerForBackbone[tripleAccionVerde.BackBoneId] = "Fryma";
    //        }

    //        // Se pueden añadir más preferencias aquí...
    //    }

    //    /// <summary>
    //    /// Método Init que se llama durante la inicialización del motor de simulación.
    //    /// Realiza la planificación inicial de lotes.
    //    /// </summary>
    //    public void Init()
    //    {
    //        // Inicializar grupos de backbones y preferencias usando _config
          
    //        if (!_initialSchedulingDone)
    //        {
    //            Console.WriteLine($"[DynamicScheduler] Realizando planificación inicial en minuto 0");
    //            PerformInitialScheduling(0); // Pasar _engine y tiempo 0
    //            _initialSchedulingDone = true;
    //        }
    //    }
    //    public void ScheduleJustInTime( double currentTime)
    //    {
    //        Console.WriteLine($"[DynamicScheduler] Iniciando planificación JIT en minuto {currentTime}");


    //        var criticalIssues = DetectCriticalIssues(currentTime);
    //        if (criticalIssues.Any())
    //        {
    //            Console.WriteLine($"[DynamicScheduler] Se detectaron {criticalIssues.Count} problemas críticos. Iniciando replanificación de emergencia.");
    //            HandleCriticalIssues(criticalIssues, currentTime);
    //        }

    //        if (currentTime > 0 && currentTime % DYNAMIC_SCHEDULING_INTERVAL == 0)
    //        {
    //            var immediateBackboneNeeds = IdentifyImmediateBackboneNeeds( currentTime);
    //            foreach (var need in immediateBackboneNeeds)
    //            {
    //                ScheduleBatchesIfNeeded(need, currentTime);
    //            }
    //        }

    //        UpdateLastKnownLevels();
    //    }

    //    private void PerformInitialScheduling( double currentTime)
    //    {
    //        var immediateBackboneNeeds = IdentifyImmediateBackboneNeeds( currentTime, INITIAL_LOOKAHEAD_HOURS);
    //        Console.WriteLine($"[DynamicScheduler] Planificación inicial encontró {immediateBackboneNeeds.Count} backbones necesarios.");

    //        // Para la planificación inicial, puede ser más agresiva o solo para los más críticos
    //        var criticalInitialNeeds = immediateBackboneNeeds
    //            .Where(n => n.Priority >= 2 || n.EarliestNeededTime <= 60) // Prioridad alta o necesarios en primera hora
    //            .ToList();

    //        foreach (var need in criticalInitialNeeds)
    //        {
    //            ScheduleInitialBatch(need, currentTime);
    //        }
    //    }

    //    private void ScheduleInitialBatch( BackboneNeed need, double currentTime)
    //    {
    //        // Buscar el mejor mezclador disponible, considerando preferencias
    //        var selectedMixer = FindBestMixerForBackbone(need.BackBoneId, currentTime, need.Priority);

    //        if (selectedMixer == null)
    //        {
    //            Console.WriteLine($"[DynamicScheduler] No hay mezcladores capaces o disponibles para producir el backbone {need.BackBoneName} en planificación inicial.");
    //            return;
    //        }

    //        var capability = selectedMixer.Mixer.Capabilities
    //            .FirstOrDefault(c => c.BackBone.BackBoneId == need.BackBoneId);

    //        if (capability == null) return;

    //        double batchAmountKg = capability.Capacity.GetValue(MassUnits.KiloGram);
    //        double batchTimeMinutes = capability.BatchTime.GetValue(TimeUnits.Minute);

    //        double startTimeMinute = selectedMixer.CurrentBatch?.EndTimeMinute ?? currentTime;
    //        if (startTimeMinute < currentTime) startTimeMinute = currentTime;

    //        var scheduledBatch = new NewScheduledBatch
    //        {
    //            BackBone = new NewBackBone
    //            {
    //                Id = need.BackBoneId,
    //                Name = need.BackBoneName,
    //                Priority = need.Priority
    //            },
    //            Mixer = selectedMixer.Mixer,
    //            BatchSize = batchAmountKg,
    //            BatchTime = batchTimeMinutes,
    //            Order = 0,
    //            StartTimeMinute = startTimeMinute,
    //            EndTimeMinute = startTimeMinute + batchTimeMinutes,
    //        };

    //        selectedMixer.ScheduleBatch(scheduledBatch);
    //        _engine.CreateSendToBigWip(selectedMixer);

    //        Console.WriteLine($"[Scheduler] Programado lote INICIAL: {scheduledBatch} para el mezclador {selectedMixer.Name} en el minuto {startTimeMinute:F2}");
    //    }

    //    /// <summary>
    //    /// Encuentra el mejor mezclador disponible para producir un backbone específico,
    //    /// considerando capacidades, disponibilidad, preferencias y evitando limpiezas extensas.
    //    /// </summary>
    //    private MixerContext? FindBestMixerForBackbone( Guid backboneId, double currentTime, int backbonePriority)
    //    {
    //        var capableMixers = _engine.Mixers
    //            .Where(m => m.Mixer.Capabilities.Any(c => c.BackBone.BackBoneId == backboneId))
    //            .ToList();

    //        if (!capableMixers.Any()) return null;

    //        // 1. Preferir mezcladores que ya estén produciendo este backbone
    //        var producingThisBackbone = capableMixers
    //            .Where(m => m.CurrentBatch?.BackBone?.Id == backboneId)
    //            .OrderBy(m => m.CurrentBatch?.EndTimeMinute ?? currentTime)
    //            .FirstOrDefault();

    //        if (producingThisBackbone != null) return producingThisBackbone;

    //        // 2. Preferir mezcladores que no requieran limpieza extensa
    //        var mixersWithoutExtensiveCleaning = capableMixers
    //            .Where(m => !RequiresExtensiveCleaning(m, backboneId))
    //            .ToList();

    //        // 3. Aplicar preferencias de mezclador
    //        string? preferredMixerName = null;
    //        if (_preferredMixerForBackbone.ContainsKey(backboneId))
    //        {
    //            preferredMixerName = _preferredMixerForBackbone[backboneId];
    //        }

    //        MixerContext? preferredMixer = null;
    //        if (!string.IsNullOrEmpty(preferredMixerName))
    //        {
    //            preferredMixer = mixersWithoutExtensiveCleaning
    //                .FirstOrDefault(m => m.Name.Equals(preferredMixerName, StringComparison.OrdinalIgnoreCase));
    //        }

    //        if (preferredMixer != null) return preferredMixer;

    //        // 4. Seleccionar el más disponible entre los que no requieren limpieza extensa
    //        if (mixersWithoutExtensiveCleaning.Any())
    //        {
    //            return mixersWithoutExtensiveCleaning
    //                .OrderBy(m => m.CurrentBatch?.EndTimeMinute ?? currentTime)
    //                .ThenBy(m => m.ScheduledBatches.Count)
    //                .FirstOrDefault();
    //        }

    //        // 5. Si todos requieren limpieza extensa, seleccionar el más disponible
    //        // (Esto puede ser modificado para penalizar más fuertemente las limpiezas)
    //        Console.WriteLine($"[Scheduler] No se encontró mezclador sin limpieza extensa para backbone {backboneId}. Se procederá con uno que la requiere.");
    //        return capableMixers
    //            .OrderBy(m => m.CurrentBatch?.EndTimeMinute ?? currentTime)
    //            .ThenBy(m => m.ScheduledBatches.Count)
    //            .FirstOrDefault();
    //    }

    //    /// <summary>
    //    /// Determina si cambiar a producir un backbone específico desde el lote actual del mezclador
    //    /// requerirá una limpieza extensa.
    //    /// </summary>
    //    private bool RequiresExtensiveCleaning(MixerContext mixer, Guid newBackboneId)
    //    {
    //        // Si el mezclador no tiene lote actual, no hay cambio
    //        if (mixer.CurrentBatch?.BackBone?.Id == null) return false;

    //        Guid currentBackboneId = mixer.CurrentBatch.BackBone.Id;

    //        // Si es el mismo backbone, no hay limpieza
    //        if (currentBackboneId == newBackboneId) return false;

    //        // Verificar si ambos backbones pertenecen al mismo grupo "seguro"
    //        bool currentInTriple = _tripleAccionGroup.Contains(currentBackboneId);
    //        bool newInTriple = _tripleAccionGroup.Contains(newBackboneId);
    //        bool currentInExtra = _extraBlancuraGroup.Contains(currentBackboneId);
    //        bool newInExtra = _extraBlancuraGroup.Contains(newBackboneId);
    //        bool currentInKM = _kolynosMentaGroup.Contains(currentBackboneId);
    //        bool newInKM = _kolynosMentaGroup.Contains(newBackboneId);

    //        // Si ambos están en el mismo grupo extenso, se requiere limpieza
    //        if ((currentInTriple && newInTriple) ||
    //            (currentInExtra && newInExtra) ||
    //            (currentInKM && newInKM))
    //        {
    //            return true;
    //        }

    //        // Si uno está en Triple Acción y el otro en Extra Blancura, se requiere limpieza
    //        if ((currentInTriple && newInExtra) || (currentInExtra && newInTriple))
    //        {
    //            return true;
    //        }

    //        // Si uno está en Triple/KM y el otro en Extra Blancura, se requiere limpieza
    //        if (((currentInTriple || currentInKM) && newInExtra) ||
    //            ((newInTriple || newInKM) && currentInExtra))
    //        {
    //            return true;
    //        }

    //        // Si uno está en Triple Accion y el otro en Kolynos/Menta, se requiere limpieza
    //        if ((currentInTriple && newInKM) || (currentInKM && newInTriple))
    //        {
    //            return true;
    //        }

    //        // En otros casos, no se requiere la limpieza extensa (solo la estándar entre backbones)
    //        return false;
    //    }


    //    private List<CriticalIssue> DetectCriticalIssues( double currentTime)
    //    {
    //        var issues = new List<CriticalIssue>();

    //        foreach (var line in _engine.Lines)
    //        {
    //            foreach (var wip in line.WIPs)
    //            {
    //                if (wip.BackBoneId.HasValue)
    //                {
    //                    Guid backboneId = wip.BackBoneId.Value;
    //                    double currentLevelRatio = wip.CurrentLevelKg / wip.CapacityKg;

    //                    if (currentLevelRatio <= CRITICAL_FILL_RATIO && wip.IsLineProducing && wip.IsLineHasThisBackBone)
    //                    {
    //                        issues.Add(new CriticalIssue
    //                        {
    //                            Type = IssueType.CriticalLowLevelInLineWip,
    //                            BackBoneId = backboneId,
    //                            EntityId = wip.TankId,
    //                            EntityName = wip.Name,
    //                            Description = $"Nivel críticamente bajo en WIP Tank {wip.Name} (Backbone {wip.BackBoneName}). Riesgo de parada de línea {line.Name}.",
    //                            Severity = 1
    //                        });
    //                    }
    //                }
    //            }
    //        }

    //        foreach (var bigWip in _engine.BigWips)
    //        {
    //            if (bigWip.BackboneId.HasValue)
    //            {
    //                Guid backboneId = bigWip.BackboneId.Value;
    //                double currentLevelRatio = bigWip.CurrentLevelKg / bigWip.CapacityKg;

    //                if (currentLevelRatio <= CRITICAL_FILL_RATIO)
    //                {
    //                    issues.Add(new CriticalIssue
    //                    {
    //                        Type = IssueType.CriticalLowLevelInBigWip,
    //                        BackBoneId = backboneId,
    //                        EntityId = bigWip.BigWipId,
    //                        EntityName = bigWip.Name,
    //                        Description = $"Nivel críticamente bajo en BigWIP Tank {bigWip.Name} (Backbone {bigWip.BackBoneName}).",
    //                        Severity = 1
    //                    });
    //                }
    //            }
    //        }

    //        return issues.OrderBy(i => i.Severity).ToList();
    //    }

    //    private void HandleCriticalIssues(List<CriticalIssue> issues, double currentTime)
    //    {
    //        foreach (var issue in issues)
    //        {
    //            Console.WriteLine($"[DynamicScheduler] Manejando problema: {issue.Description}");

    //            switch (issue.Type)
    //            {
    //                case IssueType.CriticalLowLevelInLineWip:
    //                case IssueType.CriticalLowLevelInBigWip:
    //                    var need = new BackboneNeed
    //                    {
    //                        BackBoneId = issue.BackBoneId,
    //                        BackBoneName = "Desconocido",
    //                        Priority = 3,
    //                        RequiredKg = 1000,
    //                        NetRequiredKg = 1000,
    //                        EarliestNeededTime = currentTime
    //                    };

    //                    var backboneConfig = _engine.Database.BackBones.FirstOrDefault(b => b.BackBoneId == issue.BackBoneId);
    //                    if (backboneConfig != null)
    //                    {
    //                        need.BackBoneName = backboneConfig.BackBoneName;
    //                    }

    //                    ScheduleEmergencyBatch(need, currentTime);
    //                    break;
    //            }
    //        }
    //    }

    //    private void ScheduleEmergencyBatch( BackboneNeed need, double currentTime)
    //    {
    //        // Para emergencias, priorizar el mezclador más rápido disponible, incluso si requiere limpieza
    //        var capableMixers =_engine.Mixers
    //            .Where(m => m.Mixer.Capabilities.Any(c => c.BackBone.BackBoneId == need.BackBoneId))
    //            .OrderBy(m => m.CurrentBatch?.EndTimeMinute ?? currentTime)
    //            .ToList();

    //        if (!capableMixers.Any())
    //        {
    //            Console.WriteLine($"[DynamicScheduler] No hay mezcladores capaces para producir el backbone {need.BackBoneName} en emergencia.");
    //            return;
    //        }

    //        var selectedMixer = capableMixers.First();
    //        var capability = selectedMixer.Mixer.Capabilities
    //            .FirstOrDefault(c => c.BackBone.BackBoneId == need.BackBoneId);

    //        if (capability == null) return;

    //        // Lote de emergencia, quizás más pequeño
    //        double batchAmountKg = capability.Capacity.GetValue(MassUnits.KiloGram) * 0.7;
    //        double batchTimeMinutes = capability.BatchTime.GetValue(TimeUnits.Minute);

    //        double startTimeMinute = selectedMixer.CurrentBatch?.EndTimeMinute ?? currentTime;
    //        if (startTimeMinute < currentTime) startTimeMinute = currentTime;

    //        var scheduledBatch = new NewScheduledBatch
    //        {
    //            BackBone = new NewBackBone
    //            {
    //                Id = need.BackBoneId,
    //                Name = need.BackBoneName,
    //                Priority = need.Priority
    //            },
    //            Mixer = selectedMixer.Mixer,
    //            BatchSize = batchAmountKg,
    //            BatchTime = batchTimeMinutes,
    //            Order = 999,
    //            StartTimeMinute = startTimeMinute,
    //            EndTimeMinute = startTimeMinute + batchTimeMinutes,
    //        };

    //        selectedMixer.ScheduleBatch(scheduledBatch);
    //        _engine.CreateSendToBigWip(selectedMixer);

    //        Console.WriteLine($"[Scheduler] Programado lote de EMERGENCIA: {scheduledBatch} para el mezclador {selectedMixer.Name} en el minuto {startTimeMinute:F2}");
    //    }

    //    private List<BackboneNeed> IdentifyImmediateBackboneNeeds( double currentTime, double lookaheadHours = 2.0)
    //    {
    //        var needs = new Dictionary<Guid, BackboneNeed>();
    //        double lookAheadMinutes = lookaheadHours * 60;

    //        foreach (var line in _productionLines)
    //        {
    //            var upcomingSkus = line.ScheduledSKULists
    //                .Where(sku => sku.StartTimeMinute <= (currentTime + lookAheadMinutes) && sku.EndTimeMinute > currentTime)
    //                .OrderBy(sku => sku.StartTimeMinute).ToList();

    //            foreach (var sku in upcomingSkus)
    //            {
    //                double timeOverlapStart = Math.Max(currentTime, sku.StartTimeMinute);
    //                double timeOverlapEnd = Math.Min(currentTime + lookAheadMinutes, sku.EndTimeMinute);
    //                double timeOverlapMinutes = timeOverlapEnd - timeOverlapStart;

    //                if (timeOverlapMinutes <= 0) continue;

    //                double productionRateKgPerMin = sku.MassFlow;
    //                double requiredProductionKg = timeOverlapMinutes * productionRateKgPerMin;

    //                foreach (var backboneComponent in sku.SKU.ProductBackBones)
    //                {
    //                    Guid backboneId = backboneComponent.BackBone.Id;
    //                    double percentage = backboneComponent.Percentage / 100.0;
    //                    double requiredBackboneKg = requiredProductionKg * percentage;

    //                    if (!needs.ContainsKey(backboneId))
    //                    {
    //                        needs[backboneId] = new BackboneNeed
    //                        {
    //                            BackBoneId = backboneId,
    //                            BackBoneName = backboneComponent.BackBone.Name,
    //                            Priority = backboneComponent.BackBone.Priority,
    //                            RequiredKg = 0,
    //                            EarliestNeededTime = sku.StartTimeMinute
    //                        };
    //                    }

    //                    needs[backboneId].RequiredKg += requiredBackboneKg;
    //                    if (sku.StartTimeMinute < needs[backboneId].EarliestNeededTime)
    //                    {
    //                        needs[backboneId].EarliestNeededTime = sku.StartTimeMinute;
    //                    }
    //                }
    //            }
    //        }

    //        var netNeeds = new List<BackboneNeed>();
    //        foreach (var need in needs.Values)
    //        {
    //            double availableInBigWip = _engine.BigWips
    //                .Where(b => b.BackboneId == need.BackBoneId)
    //                .Sum(b => b.CurrentLevelKg);

    //            double availableInLineWips = _engine.Lines
    //                .SelectMany(l => l.WIPs)
    //                .Where(w => w.BackBoneId == need.BackBoneId)
    //                .Sum(w => w.CurrentLevelKg);

    //            double totalAvailableKg = availableInBigWip + availableInLineWips;
    //            double netRequiredKg = Math.Max(0, need.RequiredKg - totalAvailableKg);

    //            double totalCapacityInBigWip = _engine.BigWips
    //                .Where(b => b.BackboneId == need.BackBoneId)
    //                .Sum(b => b.CapacityKg);

    //            if (totalAvailableKg / totalCapacityInBigWip > OVERSTOCK_RATIO)
    //            {
    //                netRequiredKg = 0;
    //            }

    //            if (netRequiredKg > 0)
    //            {
    //                need.NetRequiredKg = netRequiredKg;
    //                netNeeds.Add(need);
    //            }
    //        }

    //        return netNeeds
    //            .OrderBy(n => n.EarliestNeededTime)
    //            .ThenByDescending(n => n.Priority)
    //            .ToList();
    //    }
    //    // En DynamicProductionScheduler.cs, modificar el método:

    //    private void ScheduleBatchesIfNeeded( BackboneNeed need, double currentTime)
    //    {
    //        // Cambiar la lógica para usar el BigWIP como referencia principal
    //        var relevantBigWips = _engine.BigWips
    //            .Where(b => b.BackboneId == need.BackBoneId)
    //            .ToList();

    //        bool shouldRequestProduction = false;
    //        BigWipContex? targetBigWip = null;

    //        // Verificar si algún BigWIP necesita producción
    //        foreach (var bigWip in relevantBigWips)
    //        {
    //            if (bigWip.ShouldRequestProduction)
    //            {
    //                shouldRequestProduction = true;
    //                targetBigWip = bigWip;
    //                break;
    //            }
    //        }

    //        if (shouldRequestProduction && targetBigWip != null)
    //        {
    //            // Usar la lógica existente para encontrar el mejor mixer
    //            var selectedMixer = FindBestMixerForBackbone( need.BackBoneId, currentTime, need.Priority);

    //            if (selectedMixer == null)
    //            {
    //                Console.WriteLine($"[DynamicScheduler] No hay mezcladores capaces para producir el backbone {need.BackBoneName}.");
    //                return;
    //            }

    //            var capability = selectedMixer.Mixer.Capabilities
    //                .FirstOrDefault(c => c.BackBone.BackBoneId == need.BackBoneId);

    //            if (capability == null) return;

    //            double batchAmountKg = capability.Capacity.GetValue(MassUnits.KiloGram);
    //            double batchTimeMinutes = capability.BatchTime.GetValue(TimeUnits.Minute);

    //            // Calcular hora de inicio considerando cuándo se necesita
    //            double startTimeMinute = selectedMixer.CurrentBatch?.EndTimeMinute ?? currentTime;
    //            if (startTimeMinute < currentTime) startTimeMinute = currentTime;

    //            // Asegurar que no se programe si ya hay suficiente programado
    //            double totalScheduledForThisBackbone = _engine.Mixers
    //                .Where(m => m.Mixer.Capabilities.Any(c => c.BackBone.BackBoneId == need.BackBoneId))
    //                .Sum(m => m.ScheduledBatches
    //                    .Where(b => b.BackBone.Id == need.BackBoneId && b.StartTimeMinute > currentTime)
    //                    .Sum(b => b.BatchSize));

    //            double currentAvailable = relevantBigWips.Sum(b => b.CurrentLevelKg);
    //            double projectedConsumption = targetBigWip.CurrentOutletRateKgPerMin *
    //                                        (batchTimeMinutes + (startTimeMinute - currentTime));

    //            if (currentAvailable + totalScheduledForThisBackbone > projectedConsumption)
    //            {
    //                // Ya hay suficiente programado
    //                return;
    //            }

    //            var scheduledBatch = new NewScheduledBatch
    //            {
    //                BackBone = new NewBackBone
    //                {
    //                    Id = need.BackBoneId,
    //                    Name = need.BackBoneName,
    //                    Priority = need.Priority
    //                },
    //                Mixer = selectedMixer.Mixer,
    //                BatchSize = batchAmountKg,
    //                BatchTime = batchTimeMinutes,
    //                Order = 1,
    //                StartTimeMinute = startTimeMinute,
    //                EndTimeMinute = startTimeMinute + batchTimeMinutes,
    //            };

    //            selectedMixer.ScheduleBatch(scheduledBatch);
    //            _engine.CreateSendToBigWip(selectedMixer);

    //            Console.WriteLine($"[Scheduler] Programado lote PROACTIVO: {scheduledBatch} para el mezclador {selectedMixer.Name} en el minuto {startTimeMinute:F2}");
    //        }
    //    }
    //    //private void ScheduleBatchesIfNeeded2(BackboneNeed need, double currentTime)
    //    //{
    //    //    double currentLevelInBigWip = _engine.BigWips
    //    //        .Where(b => b.BackboneId == need.BackBoneId)
    //    //        .Sum(b => b.CurrentLevelKg);

    //    //    double currentLevelInLineWips = _engine.Lines
    //    //        .SelectMany(l => l.WIPs)
    //    //        .Where(w => w.BackBoneId == need.BackBoneId)
    //    //        .Sum(w => w.CurrentLevelKg);

    //    //    double totalCurrentLevelKg = currentLevelInBigWip + currentLevelInLineWips;
    //    //    double capacityInBigWip = _engine.BigWips
    //    //        .Where(b => b.BackboneId == need.BackBoneId)
    //    //        .Sum(b => b.CapacityKg);

    //    //    double requestThresholdKg = capacityInBigWip * MINIMUM_FILL_RATIO;

    //    //    if (totalCurrentLevelKg <= requestThresholdKg)
    //    //    {
    //    //        Usar la nueva lógica de selección de mezclador
    //    //        var selectedMixer = FindBestMixerForBackbone(need.BackBoneId, currentTime, need.Priority);

    //    //        if (selectedMixer == null)
    //    //        {
    //    //            Console.WriteLine($"[DynamicScheduler] No hay mezcladores capaces para producir el backbone {need.BackBoneName}.");
    //    //            return;
    //    //        }

    //    //        var capability = selectedMixer.Mixer.Capabilities
    //    //            .FirstOrDefault(c => c.BackBone.BackBoneId == need.BackBoneId);

    //    //        if (capability == null) return;

    //    //        double batchAmountKg = capability.Capacity.GetValue(MassUnits.KiloGram);
    //    //        double batchTimeMinutes = capability.BatchTime.GetValue(TimeUnits.Minute);

    //    //        double startTimeMinute = selectedMixer.CurrentBatch?.EndTimeMinute ?? currentTime;
    //    //        if (startTimeMinute < currentTime) startTimeMinute = currentTime;

    //    //        var scheduledBatch = new NewScheduledBatch
    //    //        {
    //    //            BackBone = new NewBackBone
    //    //            {
    //    //                Id = need.BackBoneId,
    //    //                Name = need.BackBoneName,
    //    //                Priority = need.Priority
    //    //            },
    //    //            Mixer = selectedMixer.Mixer,
    //    //            BatchSize = batchAmountKg,
    //    //            BatchTime = batchTimeMinutes,
    //    //            Order = 1,
    //    //            StartTimeMinute = startTimeMinute,
    //    //            EndTimeMinute = startTimeMinute + batchTimeMinutes,
    //    //        };

    //    //        selectedMixer.ScheduleBatch(scheduledBatch);
    //    //        _engine.CreateSendToBigWip(selectedMixer);

    //    //        Console.WriteLine($"[Scheduler] Programado lote: {scheduledBatch} para el mezclador {selectedMixer.Name} en el minuto {startTimeMinute:F2}");
    //    //    }
    //    //}

    //    private void UpdateLastKnownLevels()
    //    {
    //        _lastKnownLevels.Clear();
    //        foreach (var bigWip in _engine.BigWips)
    //        {
    //            if (bigWip.BackboneId.HasValue)
    //            {
    //                _lastKnownLevels[bigWip.BackboneId.Value] = bigWip.CurrentLevelKg;
    //            }
    //        }
    //        foreach (var line in _engine.Lines)
    //        {
    //            foreach (var wip in line.WIPs)
    //            {
    //                if (wip.BackBoneId.HasValue)
    //                {
    //                    _lastKnownLevels[wip.BackBoneId.Value] = wip.CurrentLevelKg;
    //                }
    //            }
    //        }
    //    }
    //}

    //public class BackboneNeed
    //{
    //    public Guid BackBoneId { get; set; }
    //    public string BackBoneName { get; set; } = string.Empty;
    //    public int Priority { get; set; }
    //    public double RequiredKg { get; set; }
    //    public double NetRequiredKg { get; set; }
    //    public double EarliestNeededTime { get; set; }
    //}

    //public class CriticalIssue
    //{
    //    public IssueType Type { get; set; }
    //    public Guid BackBoneId { get; set; }
    //    public Guid EntityId { get; set; }
    //    public string EntityName { get; set; } = string.Empty;
    //    public string Description { get; set; } = string.Empty;
    //    public int Severity { get; set; }
    //}

    //public enum IssueType
    //{
    //    CriticalLowLevelInLineWip,
    //    CriticalLowLevelInBigWip,
    //}
}