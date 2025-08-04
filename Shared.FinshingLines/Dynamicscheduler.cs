// Archivo: DynamicProductionScheduler.cs
using Shared.FinshingLines.DPSimulation.Lines;
using Shared.FinshingLines.DPSimulation.Mixers;

namespace Shared.FinshingLines.DPSimulation.Schedulers
{
    /// <summary>
    /// Programa la producción de lotes de mezcladores de forma dinámica y just-in-time,
    /// basándose en las necesidades futuras de las líneas de producción.
    /// Incluye lógica para detectar problemas, replanificar y priorizar mezcladores.
    /// </summary>
    public class DynamicProductionScheduler
    {
        private readonly List<LineContext> _productionLines;
        private readonly NewSimulationEngine _engine;
        private bool _initialSchedulingDone = false;

        // Definición de grupos de backbones que requieren limpieza extensa al cambiar entre ellos
        private HashSet<Guid> _tripleAccionGroup = new HashSet<Guid>();
        private HashSet<Guid> _extraBlancuraGroup = new HashSet<Guid>();
        private HashSet<Guid> _kolynosMentaGroup = new HashSet<Guid>();

        // Priorización de mezcladores para grupos específicos
        private Dictionary<Guid, string> _preferredMixerForBackbone = new Dictionary<Guid, string>();


        private List<MixerPreferenceById> _mixerPreferencesById = new List<MixerPreferenceById>();
        // Seguimiento de niveles anteriores por tanque para detección de problemas
        private Dictionary<Guid, TankLevelInfo> _lastKnownTankLevels = new Dictionary<Guid, TankLevelInfo>();

        public DynamicProductionScheduler(NewSimulationEngine engine, ReadSimulationFromDatabase config)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _productionLines = engine.Lines ?? throw new ArgumentNullException(nameof(engine.Lines));

            // Inicializar grupos y preferencias
            InitializeBackboneGroups(config);
            InitializeMixerPreferences(config);
        }
        /// <summary>
        /// Inicializa las preferencias de mezcladores basadas en los IDs de la configuración.
        /// </summary>
        private void InitializeMixerPreferences(ReadSimulationFromDatabase config)
        {
            _mixerPreferencesById.Clear();
            config.DefinePrefredMixers();
            foreach (var backbone in config.BackBones)
            {
                // Verificar si hay IDs válidos en la lista
                var validPreferredIds = backbone.PreferredMixerIds.Where(id => id != Guid.Empty).ToList();
                if (validPreferredIds.Any())
                {
                    _mixerPreferencesById.Add(new MixerPreferenceById
                    {
                        BackBoneId = backbone.Id,
                        PreferredMixerIds = new List<Guid>(validPreferredIds)
                    });
                }
            }
        }
        /// <summary>
        /// Inicializa los grupos de backbones para la lógica de limpieza extensa.
        /// </summary>
        private void InitializeBackboneGroups(ReadSimulationFromDatabase config)
        {
            var tripleAccionBackbones = config.BackBones
                .Where(bb => bb.Name.Contains("Triple Accion"))
                .Select(bb => bb.Id)
                .ToHashSet();
            _tripleAccionGroup = tripleAccionBackbones;

            var extraBlancuraBackbones = config.BackBones
                .Where(bb => bb.Name.Contains("Extrablancura"))
                .Select(bb => bb.Id)
                .ToHashSet();
            _extraBlancuraGroup = extraBlancuraBackbones;

            var kolynosMentaBackbones = config.BackBones
                .Where(bb => bb.Name == "Kolynos" || bb.Name == "Menta")
                .Select(bb => bb.Id)
                .ToHashSet();
            _kolynosMentaGroup = kolynosMentaBackbones;
        }

        /// <summary>
        /// Inicializa las preferencias de mezcladores para backbones específicos.
        /// </summary>


        /// <summary>
        /// Método Init que se llama durante la inicialización del motor de simulación.
        /// Realiza la planificación inicial de lotes.
        /// </summary>
        public void Init()
        {
            if (!_initialSchedulingDone)
            {
                Console.WriteLine($"[DynamicScheduler] Realizando planificación inicial en minuto 0");
                PerformInitialScheduling();
                _initialSchedulingDone = true;
            }
        }

        // En DynamicProductionScheduler.cs
        public void ScheduleJustInTime(double currentTime)
        {
            var linesWithWips = _engine.Lines.SelectMany(line => line.WIPs.Where(wip => wip.BackBoneId.HasValue)).ToList();


            foreach (var bigWip in _engine.BigWips)
            {


                if (bigWip.BackboneId == Guid.Empty) continue; // No hay backbone asignado
                BackBoneConfiguration? backbone = _engine.Database.BackBones.FirstOrDefault(b => b.Id == bigWip.BackboneId);
                if (backbone == null) continue; // No se encontró el backbone
                double AverageWipOutletflow = linesWithWips.Where(x => x.BackBoneId == backbone.Id).Sum(x => x.MassFlowKg);
                if (AverageWipOutletflow <= 0) continue;//no esta consumiendo nada, no programamos nada
                string backbonemane = backbone.Name ?? "Desconocido";
                double massProducing = _engine.Mixers
                    .Where(m => m.CurrentBatch?.BackBone?.Id == bigWip.BackboneId)
                    .Sum(m => m.CurrentBatch?.BatchSize ?? 0);
                double massScheduled = _engine.Mixers.Sum(m => m.ScheduledBatches
                    .Where(b => b.BackBone.Id == bigWip.BackboneId)
                    .Sum(b => b.BatchSize));
                double CurrentWipLevel = bigWip.CurrentLevelKg + bigWip.InletQueue.Sum(x => x.PendingToReceive);
                double totalmassProducing = massProducing + massScheduled;


                var preferedMixer = FindBestMixerForBackbone(backbone.Id, currentTime);
                if (preferedMixer == null)
                {

                    continue;
                }
                var capability = preferedMixer.Mixer.Capabilities
                   .FirstOrDefault(c => c.BackBone.Id == backbone.Id);

                if (capability == null)
                {

                    continue;
                }
                var batchtime = capability.BatchTime.GetValue(TimeUnits.Minute);
                double batchSize = capability.Capacity.GetValue(MassUnits.KiloGram);

                double timetoConsumeMass = totalmassProducing / AverageWipOutletflow;
                double massExpectedToConsume = AverageWipOutletflow * batchtime;
                double futureBigWiplevel = CurrentWipLevel - massExpectedToConsume + batchSize + totalmassProducing;
                if (futureBigWiplevel <= bigWip.CapacityKg * 0.85)
                {
                    // Si el tiempo para consumir la masa es menor que el tiempo de lote, programar un lote

                    var newBatch = new NewScheduledBatch
                    {
                        BackBone = backbone,
                        MixerContext = preferedMixer,
                        BatchSize = batchSize,
                        BatchTime = batchtime, // Tiempo de lote fijo para simplificar
                        StartTimeMinute = currentTime,

                    };
                    preferedMixer.ScheduleBatch(newBatch);
                    _engine.ScheduledBatches.Add(newBatch);
                    bigWip.MassScheduled += newBatch.BatchSize;

                }


            }

            UpdateLastKnownLevels();
        }



        private void PerformInitialScheduling()
        {
            var wipsinLines = _engine.Lines.SelectMany(line => line.WIPs.Where(x => x.BackBoneId.HasValue)).ToList();

            foreach (var bigWip in _engine.BigWips)
            {
                if (bigWip.BackboneId == Guid.Empty) continue; // No hay backbone asignado
                BackBoneConfiguration? backbone = _engine.Database.BackBones.FirstOrDefault(b => b.Id == bigWip.BackboneId);
                if (backbone == null) continue; // No se encontró el backbone
                string backboneName = backbone.Name ?? "Desconocido";

                var WipsInBackbone = wipsinLines.Where(wip => wip.BackBoneId == bigWip.BackboneId).ToList();
                if (WipsInBackbone.Count == 0)
                {

                    continue;
                }
                double massneededByWipsinLines = WipsInBackbone
                    .Sum(wip => wip.InitialMassToFillWipKg);
                double massToProduceinLines = WipsInBackbone
                    .Sum(wip => wip.PendingToProduce);

                double massNeededByBigWip = bigWip.MassNeededtoFillVessel;

                double totalMassNeeded = massNeededByBigWip + massneededByWipsinLines;
                double starttime = 0;
                while (totalMassNeeded > 0)
                {


                    var selectedMixer = FindBestMixerForBackboneInitial(backbone.Id, 0);

                    if (selectedMixer == null)
                    {
                        totalMassNeeded = 0;
                        continue;
                    }
                    var capability = selectedMixer.Mixer.Capabilities
                   .FirstOrDefault(c => c.BackBone.Id == backbone.Id);

                    if (capability == null)
                    {

                        continue;
                    }
                    double batchSize = capability.Capacity.GetValue(MassUnits.KiloGram);
                    if (totalMassNeeded > batchSize)
                    {
                        var initialBatch = new NewScheduledBatch
                        {
                            BackBone = backbone,
                            MixerContext = selectedMixer,
                            StartTimeMinute = starttime,
                            BatchSize = capability.Capacity.GetValue(MassUnits.KiloGram), // Llenar el BigWIP completamente
                            BatchTime = capability.BatchTime.GetValue(TimeUnits.Minute), // Tiempo de lote fijo para simplificar

                        };
                        selectedMixer.ScheduleBatch(initialBatch);
                        totalMassNeeded -= initialBatch.BatchSize;
                        _engine.ScheduledBatches.Add(initialBatch);
                        bigWip.MassScheduled += initialBatch.BatchSize;
                        starttime += initialBatch.BatchTime; // Actualizar el tiempo de inicio para el próximo lote
                    }
                    else
                    {
                        totalMassNeeded = 0;//Evitamos programar un lote menor al tamaño mínimo el sistema continuo se encargara de programarlo segun la necesidad de producir  
                    }

                }



            }
        }

        private MixerContext? FindBestMixerForBackbone(Guid backboneId, double currentTime)
        {
            // 1. Filtrar mezcladores capaces de producir este backbone
            var capableMixers = _engine.Mixers
                .Where(m => m.Mixer.Capabilities.Any(c => c.BackBone.Id == backboneId))
                .ToList();

            if (!capableMixers.Any())
            {
                Console.WriteLine($"[Scheduler] No se encontraron mezcladores capaces de producir el backbone {backboneId}.");
                return null;
            }

            // 2. Separar mezcladores que requieren limpieza extensa de los que no
            var mixersWithoutExtensiveCleaning = capableMixers
                .Where(m => !RequiresExtensiveCleaning(m, backboneId))
                .ToList();

            var mixersWithExtensiveCleaning = capableMixers
                .Where(m => RequiresExtensiveCleaning(m, backboneId))
                .ToList();

            // 3. Obtener preferencias para este backbone
            var backbonePreferences = _mixerPreferencesById.FirstOrDefault(p => p.BackBoneId == backboneId);
            List<Guid> preferredMixerIds = backbonePreferences?.PreferredMixerIds ?? new List<Guid>();

            // --- LÓGICA PRINCIPAL DE SELECCIÓN ---
            // Prioridad: 1. Sin limpieza extensa, 2. Preferidos, 3. Más disponible

            // a) Primero, intentar con mezcladores SIN limpieza extensa
            if (mixersWithoutExtensiveCleaning.Any())
            {
                // Crear diccionario para ordenar por preferencia rápidamente
                var preferenceOrder = preferredMixerIds
                    .Select((id, index) => new { id, index })
                    .ToDictionary(x => x.id, x => x.index);
                if (capableMixers.Count == 2)
                {
                    var selectedMixer = mixersWithoutExtensiveCleaning
                    .OrderBy(m => // Primero por preferencia (0, 1, 2, ...), luego por disponibilidad
                    {
                        // Si es preferido, usar su índice de preferencia
                        // Si no es preferido, asignar un valor alto (int.MaxValue) para ir al final
                        return preferenceOrder.TryGetValue(m.Mixer.MixerId, out int order) ? order : int.MaxValue;
                    })
                    .ThenBy(m => m.EndTime) // Luego por disponibilidad (más pronto primero)
                    .ThenBy(m => m.ScheduledBatches.Count) // Finalmente por carga de trabajo
                    .FirstOrDefault();

                    if (selectedMixer != null)
                    {
                        string preferenceInfo = preferenceOrder.ContainsKey(selectedMixer.Mixer.MixerId) ?
                            $"preferido (orden {preferenceOrder[selectedMixer.Mixer.MixerId]})" : "no preferido";
                        Console.WriteLine($"[Scheduler] Seleccionado {selectedMixer.Name} para {backboneId} (sin limpieza, {preferenceInfo}, disponible en {(selectedMixer.CurrentBatch?.EndTimeMinute ?? currentTime):F0} min).");
                        return selectedMixer;
                    }
                }
                else
                {
                    var selectedMixer = mixersWithoutExtensiveCleaning
                    .OrderBy(m => m.EndTime) // Luego por disponibilidad (más pronto primero)
                    .ThenBy(m => m.ScheduledBatches.Count) // Finalmente por carga de trabajo
                    .FirstOrDefault();

                    if (selectedMixer != null)
                    {
                        string preferenceInfo = preferenceOrder.ContainsKey(selectedMixer.Mixer.MixerId) ?
                            $"preferido (orden {preferenceOrder[selectedMixer.Mixer.MixerId]})" : "no preferido";
                        Console.WriteLine($"[Scheduler] Seleccionado {selectedMixer.Name} para {backboneId} (sin limpieza, {preferenceInfo}, disponible en {(selectedMixer.CurrentBatch?.EndTimeMinute ?? currentTime):F0} min).");
                        return selectedMixer;
                    }
                }

            }

            // b) Si no hay ninguno sin limpieza extensa (o no se pudo seleccionar), 
            //    intentar con los que SÍ la requieren, aplicando la misma lógica de preferencia/disponibilidad.
            Console.WriteLine($"[Scheduler] Todos los mezcladores capaces para {backboneId} requieren limpieza extensa. Buscando el mejor disponible.");
            if (mixersWithExtensiveCleaning.Any())
            {
                var preferenceOrder = preferredMixerIds
                    .Select((id, index) => new { id, index })
                    .ToDictionary(x => x.id, x => x.index);

                var selectedMixer = mixersWithExtensiveCleaning
                    .OrderBy(m =>
                    {
                        return preferenceOrder.TryGetValue(m.Mixer.MixerId, out int order) ? order : int.MaxValue;
                    })
                    .ThenBy(m => m.EndTime)
                    .ThenBy(m => m.ScheduledBatches.Count)
                    .FirstOrDefault();

                if (selectedMixer != null)
                {
                    string preferenceInfo = preferenceOrder.ContainsKey(selectedMixer.Mixer.MixerId) ?
                        $"preferido (orden {preferenceOrder[selectedMixer.Mixer.MixerId]})" : "no preferido";
                    Console.WriteLine($"[Scheduler] Seleccionado {selectedMixer.Name} para {backboneId} (con limpieza, {preferenceInfo}, disponible en {(selectedMixer.CurrentBatch?.EndTimeMinute ?? currentTime):F0} min).");
                    return selectedMixer;
                }
            }

            // c) Último recurso: Si por alguna razón la lógica anterior no devuelve nada,
            //    seleccionar cualquier mezclador capaz, priorizando por disponibilidad.
            Console.WriteLine($"[Scheduler] ADVERTENCIA: Seleccionando cualquier mezclador capaz para {backboneId} como último recurso.");
            return capableMixers
                .OrderBy(m => m.EndTime)
                .ThenBy(m => m.ScheduledBatches.Count)
                .FirstOrDefault();
        }


        private MixerContext? FindBestMixerForBackboneInitial(Guid backboneId, double currentTime)
        {
            var capableMixers = _engine.Mixers
                .Where(m => m.Mixer.Capabilities.Any(c => c.BackBone.Id == backboneId))
                .ToList();

            if (!capableMixers.Any()) return null;

            MixerContext? producingThisBackbone = null!;
            if (capableMixers.Count <= 2)
            {
                producingThisBackbone = capableMixers
               .Where(m => m.CurrentBatch?.BackBone?.Id == backboneId)
               .FirstOrDefault();

                if (producingThisBackbone != null) return producingThisBackbone;
            }
            else
            {
                producingThisBackbone = capableMixers
               .Where(m => m.CurrentBatch?.BackBone?.Id == backboneId)
               .OrderBy(m => m.EndTime)
               .FirstOrDefault();

                if (producingThisBackbone != null) return producingThisBackbone;
            }

            // 1. Preferir mezcladores que ya estén produciendo este backbone


            // 2. Separar mezcladores que requieren limpieza extensa de los que no
            var mixersWithoutExtensiveCleaning = capableMixers
                .Where(m => !RequiresExtensiveCleaning(m, backboneId))
                .ToList();

            var mixersWithExtensiveCleaning = capableMixers
                .Where(m => RequiresExtensiveCleaning(m, backboneId))
                .ToList();

            // 3. Si hay mezcladores sin limpieza extensa, aplicar preferencias entre ellos
            if (mixersWithoutExtensiveCleaning.Any())
            {
                // Buscar si algún mezclador preferido está disponible sin limpieza
                var backbonePreferences = _mixerPreferencesById.FirstOrDefault(p => p.BackBoneId == backboneId);

                if (backbonePreferences != null && backbonePreferences.PreferredMixerIds.Any())
                {
                    // Crear diccionario de prioridades
                    var preferencePriority = new Dictionary<Guid, int>();
                    for (int i = 0; i < backbonePreferences.PreferredMixerIds.Count; i++)
                    {
                        preferencePriority[backbonePreferences.PreferredMixerIds[i]] = i;
                    }

                    // Buscar primero preferidos que no requieran limpieza
                    var preferredWithoutCleaning = mixersWithoutExtensiveCleaning
                        .Where(m => preferencePriority.ContainsKey(m.Mixer.MixerId))
                        .OrderBy(m => preferencePriority[m.Mixer.MixerId])
                        .ThenBy(m => m.EndTime)
                        .FirstOrDefault();

                    if (preferredWithoutCleaning != null)
                    {
                        return preferredWithoutCleaning;
                    }
                }

                // Si no hay preferidos disponibles sin limpieza, tomar el más disponible sin limpieza
                return mixersWithoutExtensiveCleaning
                    .OrderBy(m => m.EndTime)
                    .ThenBy(m => m.ScheduledBatches.Count)
                    .FirstOrDefault();
            }

            // 4. Si solo hay mezcladores con limpieza extensa, aplicar preferencias entre ellos
            Console.WriteLine($"[Scheduler] Todos los mezcladores para backbone {backboneId} requieren limpieza extensa.");

            if (mixersWithExtensiveCleaning.Any())
            {
                var backbonePreferences = _mixerPreferencesById.FirstOrDefault(p => p.BackBoneId == backboneId);

                if (backbonePreferences != null && backbonePreferences.PreferredMixerIds.Any())
                {
                    var preferencePriority = new Dictionary<Guid, int>();
                    for (int i = 0; i < backbonePreferences.PreferredMixerIds.Count; i++)
                    {
                        preferencePriority[backbonePreferences.PreferredMixerIds[i]] = i;
                    }

                    // Buscar preferidos aunque requieran limpieza
                    var preferredWithCleaning = mixersWithExtensiveCleaning
                        .Where(m => preferencePriority.ContainsKey(m.Mixer.MixerId))
                        .OrderBy(m => preferencePriority[m.Mixer.MixerId])
                        .ThenBy(m => m.EndTime)
                        .FirstOrDefault();

                    if (preferredWithCleaning != null)
                    {
                        return preferredWithCleaning;
                    }
                }

                // Si no hay preferidos, tomar el más disponible aunque requiera limpieza
                return mixersWithExtensiveCleaning
                    .OrderBy(m => m.EndTime)
                    .ThenBy(m => m.ScheduledBatches.Count)
                    .FirstOrDefault();
            }

            // 5. Último recurso: cualquier mezclador capaz
            Console.WriteLine($"[Scheduler] No se encontró mezclador óptimo para backbone {backboneId}.");
            return capableMixers
                .OrderBy(m => m.EndTime)
                .ThenBy(m => m.ScheduledBatches.Count)
                .FirstOrDefault();
        }
        private bool RequiresExtensiveCleaning(MixerContext mixer, Guid newBackboneId)
        {
            if (mixer.CurrentBatch?.BackBone?.Id == null) return false;

            Guid currentBackboneId = mixer.CurrentBatch.BackBone.Id;

            if (currentBackboneId == newBackboneId) return false;

            bool currentInTriple = _tripleAccionGroup.Contains(currentBackboneId);
            bool newInTriple = _tripleAccionGroup.Contains(newBackboneId);
            bool currentInExtra = _extraBlancuraGroup.Contains(currentBackboneId);
            bool newInExtra = _extraBlancuraGroup.Contains(newBackboneId);
            bool currentInKM = _kolynosMentaGroup.Contains(currentBackboneId);
            bool newInKM = _kolynosMentaGroup.Contains(newBackboneId);

            // Si ambos están en el mismo grupo extenso, se requiere limpieza
            if ((currentInTriple && newInTriple) ||
                (currentInExtra && newInExtra) ||
                (currentInKM && newInKM))
            {
                return true;
            }

            // Si uno está en Triple Acción y el otro en Extra Blancura, se requiere limpieza
            if ((currentInTriple && newInExtra) || (currentInExtra && newInTriple))
            {
                return true;
            }

            // Si uno está en Triple/KM y el otro en Extra Blancura, se requiere limpieza
            if (((currentInTriple || currentInKM) && newInExtra) ||
                ((newInTriple || newInKM) && currentInExtra))
            {
                return true;
            }

            // Si uno está en Triple Accion y el otro en Kolynos/Menta, se requiere limpieza
            if ((currentInTriple && newInKM) || (currentInKM && newInTriple))
            {
                return true;
            }

            return false;
        }







        private void UpdateLastKnownLevels()
        {
            _lastKnownTankLevels.Clear();

            foreach (var bigWip in _engine.BigWips)
            {
                _lastKnownTankLevels[bigWip.BigWipId] = new TankLevelInfo
                {
                    TankId = bigWip.BigWipId,
                    BackboneId = bigWip.BackboneId,
                    TankName = bigWip.Name,
                    LastKnownLevelKg = bigWip.CurrentLevelKg,
                    Type = TankType.BigWip
                };
            }

            foreach (var line in _engine.Lines)
            {
                foreach (var wip in line.WIPs)
                {
                    _lastKnownTankLevels[wip.TankId] = new TankLevelInfo
                    {
                        TankId = wip.TankId,
                        BackboneId = wip.BackBoneId,
                        TankName = wip.Name,
                        LastKnownLevelKg = wip.CurrentLevelKg,
                        Type = TankType.LineWip
                    };
                }
            }
        }
    }

    public class BackboneNeed
    {
        public Guid BackBoneId { get; set; }
        public string BackBoneName { get; set; } = string.Empty;
        public int Priority { get; set; }
        public double RequiredKg { get; set; }
        public double NetRequiredKg { get; set; }
        public double EarliestNeededTime { get; set; }
        public override string ToString()
        {
            return $"{BackBoneName} Required: {RequiredKg}, Kg, NetRequired: {NetRequiredKg}, Kg";
        }
    }

    public class CriticalIssue
    {
        public IssueType Type { get; set; }
        public Guid BackBoneId { get; set; }
        public Guid EntityId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Severity { get; set; }
        public override string ToString()
        {
            return $"{EntityName} - {Description} (Severity: {Severity})";
        }
    }

    public enum IssueType
    {
        CriticalLowLevelInLineWip,
        CriticalLowLevelInBigWip,
    }

    public class TankLevelInfo
    {
        public Guid TankId { get; set; }
        public Guid? BackboneId { get; set; }
        public string TankName { get; set; } = string.Empty;
        public double LastKnownLevelKg { get; set; }
        public TankType Type { get; set; }
    }

    public enum TankType
    {
        BigWip,
        LineWip
    }
    public class MixerPreferenceById
    {
        public Guid BackBoneId { get; set; }
        // Ahora almacenamos los IDs preferidos en orden
        public List<Guid> PreferredMixerIds { get; set; } = new List<Guid>();
    }


}