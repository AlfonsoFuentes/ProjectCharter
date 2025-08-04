using Shared.FinshingLines.DPSimulation;
using Shared.FinshingLines.DPSimulation.Mixers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FinshingLines
{
    public static class ScheduledBatchTableRowExtensions
{
    public static string GetBackboneClass(this ScheduledBatchTableRow row, string mixerName)
    {
        if (row.MixerBatchInfo.TryGetValue(mixerName, out var cell) && !cell.IsEmpty)
        {
            return $"backbone-{cell.BackBoneName.ToLower().Replace(" ", "-").Replace(".", "")}";
        }
        return "cell-empty";
    }

    public static bool HasData(this ScheduledBatchTableRow row, string mixerName)
    {
        return row.MixerBatchInfo.TryGetValue(mixerName, out var cell) && !cell.IsEmpty;
    }

    public static string GetBackboneName(this ScheduledBatchTableRow row, string mixerName)
    {
        return row.MixerBatchInfo.TryGetValue(mixerName, out var cell) && !cell.IsEmpty
            ? cell.BackBoneName
            : string.Empty;
    }

    public static double GetBatchTime(this ScheduledBatchTableRow row, string mixerName)
    {
        return row.MixerBatchInfo.TryGetValue(mixerName, out var cell) && !cell.IsEmpty
            ? cell.BatchTimeMinutes
            : 0;
    }

    public static double GetAmountKg(this ScheduledBatchTableRow row, string mixerName)
    {
        return row.MixerBatchInfo.TryGetValue(mixerName, out var cell) && !cell.IsEmpty
            ? cell.AmountKg
            : 0;
    }
}
public class ScheduledBatchTable
{
    public List<ScheduledBatchTableRow> Rows { get; set; } = new();
    public List<string> Mixers { get; set; } = new();
}

public class ScheduledBatchTableRow
{
    public double StartTimeMinute { get; set; }
    public Dictionary<string, ScheduledBatchTableCell> MixerBatchInfo { get; set; } = new();
}
public class ScheduledBatchTableCell
{
    public Guid BackBoneId { get; set; }
    public string BackBoneName { get; set; } = string.Empty;
    public double AmountKg { get; set; }
    public double BatchTimeMinutes { get; set; }
    public bool IsEmpty => BackBoneId == Guid.Empty;
}
    //    public class BackboneRequirementInfo
    //    {
    //        public Guid BackBoneId { get; set; }
    //        public string BackBoneName { get; set; } = string.Empty;
    //        public double TotalRequiredKg { get; set; }
    //        public int Priority { get; set; } = 1;
    //        public double EstimatedStartMinute { get; set; } = double.MaxValue;
    //        public List<Guid> LineIds { get; set; } = new();


    //        public int MinOrder { get; set; } = 0;

    //        public List<string> LineNames { get; set; } = new();

    //        public override string ToString()
    //        {
    //            return $"Backbone: {BackBoneName} | " +
    //                   $"Requerido: {TotalRequiredKg:F2} kg | " +
    //                   $"Prioridad: {Priority} | " +
    //                   $"Orden Mínimo: {MinOrder} | " +
    //                   $"Líneas: {string.Join(", ", LineNames.Select(l => l.ToString()))} | " +
    //                   $"Inicio Estimado: {EstimatedStartMinute:F0} min";
    //        }
    //    }
    //    public class BackboneRequirement
    //    {
    //        public Guid BackboneId { get; set; }
    //        public Guid LineId { get; set; }
    //        public string BackBoneName { get; set; } = string.Empty;
    //        public string LineName { get; set; } = string.Empty;
    //        public string ProductName { get; set; } = string.Empty;
    //        public double RequiredKg { get; set; } = 0;
    //        public int Order { get; set; }
    //        public double LineEstimatedStartMinute { get; set; } = 0;
    //        public override string ToString()
    //        {
    //            return $"Backbone: {BackBoneName} | " +
    //                   $"Línea: {LineName} | " +
    //                   $"Producto: {ProductName} | " +
    //                   $"Requerido: {RequiredKg:F2} kg | " +
    //                   $"Orden: {Order} | " +
    //                   $"Inicio Estimado de Línea: {LineEstimatedStartMinute:F0} min";
    //        }

    //    }
    //    public class ProductionScheduler
    //    {

    //        public List<ScheduledBatch> ScheduledBatches { get; private set; } = new();
    //        public List<NewScheduledBatch> NewScheduledBatches { get; private set; } = new();
    //        public List<RequiredBackbone> RequiredBackbones { get; private set; } = new();
    //        public void ScheduleProduction(ReadSimulationFromDatabase config, NewSimulationEngine engine)
    //        {
    //            var requiredBackbonesDict = new Dictionary<Guid, List<BackboneRequirement>>();
    //            foreach (var line in engine.Lines)
    //            {
    //                var orderedskus = line.ScheduledSKULists.OrderBy(x => x.Order).ToList();
    //                foreach (var sku in orderedskus)
    //                {
    //                    foreach (var backBone in sku.SKU.ProductBackBones)
    //                    {
    //                        Guid backboneId = backBone.BackBone.Id;
    //                        double requiredKg = sku.MassPlanned * (backBone.Percentage / 100);
    //                        if (!requiredBackbonesDict.ContainsKey(backboneId))
    //                            requiredBackbonesDict[backboneId] = new List<BackboneRequirement>();
    //                        requiredBackbonesDict[backboneId].Add(new BackboneRequirement
    //                        {
    //                            BackboneId = backboneId,
    //                            LineId = line.Line.LineId,
    //                            ProductName = backBone.BackBone.Name,
    //                            RequiredKg = requiredKg,
    //                            Order = sku.Order,
    //                            LineEstimatedStartMinute = sku.StartTimeMinute,
    //                        });
    //                    }

    //                }
    //            }
    //            var availableBackbones = new Dictionary<Guid, double>();
    //            // Paso 2: Restar masa disponible en WIP Tanks


    //            foreach (var line in engine.Lines)
    //            {
    //                foreach (var tank in line.WIPs)
    //                {
    //                    if (tank.BackBoneId.HasValue)
    //                    {
    //                        var backboneId = tank.BackBoneId.Value;
    //                        if (!availableBackbones.ContainsKey(backboneId))
    //                        {
    //                            availableBackbones[backboneId] = 0;
    //                        }

    //                        availableBackbones[backboneId] += tank.CurrentLevelKg;
    //                    }
    //                }
    //            }
    //            // Paso 3: Acumular requerimientos por backbone
    //            var backboneRequirements = requiredBackbonesDict
    //                .Select(kvp =>
    //                {
    //                    var backBoneConfig = config.BackBones.FirstOrDefault(b => b.Id == kvp.Key);
    //                    var lineIds = kvp.Value.Select(r => r.LineId).Distinct().ToList();
    //                    double available = availableBackbones.TryGetValue(kvp.Key, out var a) ? a : 0;
    //                    double requiredKg = kvp.Value.Sum(r => r.RequiredKg);
    //                    double neededKg = Math.Max(0, requiredKg - available);
    //                    return new BackboneRequirementInfo
    //                    {
    //                        BackBoneId = kvp.Key,
    //                        BackBoneName = backBoneConfig?.Name ?? "Desconocido",
    //                        TotalRequiredKg = neededKg,

    //                        EstimatedStartMinute = kvp.Value.Min(r => r.LineEstimatedStartMinute),
    //                        LineIds = lineIds
    //                    };
    //                })
    //                .ToList();
    //            // Paso 3: Calcular masa real necesitada

    //            var backbonesToProduce = backboneRequirements
    //                .OrderBy(r => r.EstimatedStartMinute)
    //                .ThenByDescending(r => r.Priority)
    //                .ToList();



    //            // Paso 4: Asignar producción a mezcladores
    //            var availableMixers = engine.Mixers.Select(m => new MixerAssignmentState
    //            {
    //                MixerId = m.Mixer.MixerId,
    //                MixerName = m.Name,

    //                Configuration = m.Mixer,
    //                RealMixer = m, // ✅ Referencia al mezclador real
    //            }).ToList();

    //            foreach (var req in backbonesToProduce)
    //            {
    //                Guid backBoneId = req.BackBoneId;
    //                double remaining = req.TotalRequiredKg;
    //                while (remaining > 0)
    //                {
    //                    var capableMixers = availableMixers
    //                    .Where(m => m.Configuration.Capabilities.Any(c => c.BackBone.Id == backBoneId))
    //                    .OrderBy(m => m.AvailableAtMinute)
    //                    .ToList();

    //                    if (!capableMixers.Any())
    //                    {
    //                        continue;
    //                    }
    //                    MixerAssignmentState? mixer = null!;
    //                    if (capableMixers.Count <= 2)
    //                    {
    //                        mixer = capableMixers
    //                         .OrderBy(m => m.CurrentBackboneId != backBoneId ? 1 : 0) // Quien ya lo produce, primero
    //                         .ThenBy(m => m.AvailableAtMinute) // Luego por disponibilidad
    //                         .FirstOrDefault();
    //                    }
    //                    else
    //                    {
    //                        mixer = capableMixers
    //                             .OrderBy(m => m.AvailableAtMinute) // por disponibilidad
    //                            .ThenBy(m => m.CurrentBackboneId != backBoneId ? 1 : 0) // Quien ya lo produce, primero
    //                           .FirstOrDefault();
    //                    }
    //                    if (mixer == null)
    //                    {
    //                        // Si no hay mezclador disponible ahora, pasamos al siguiente
    //                        continue;
    //                    }
    //                    var capability = mixer.Configuration.Capabilities
    //                        .FirstOrDefault(c => c.BackBone.Id == backBoneId);

    //                    if (capability == null)
    //                        continue;

    //                    double batchTimeMinutes = capability.BatchTime.GetValue(TimeUnits.Minute);
    //                    double batchAmount = capability.Capacity.GetValue(MassUnits.KiloGram);


    //                    double startTime = mixer.AvailableAtMinute;
    //                    double endTime = startTime + batchTimeMinutes;

    //                    //Paso 8: Programar el lote
    //                    var scheduledBatch = new NewScheduledBatch
    //                    {
    //                        BackBone = new()
    //                        {
    //                            Id = backBoneId,
    //                            Name = req.BackBoneName,
    //                        },
    //                        //Mixer = mixer.Configuration,
    //                        BatchSize = batchAmount,
    //                        BatchTime = batchTimeMinutes,
    //                        Order = 1,
    //                        StartTimeMinute = startTime,
    //                        EndTimeMinute = endTime,


    //                    };

    //                    NewScheduledBatches.Add(scheduledBatch);

    //                    // ✅ Agregar el lote al mezclador real
    //                    mixer.RealMixer.ScheduleBatch(scheduledBatch);
    //                    // Actualizar disponibilidad del mezclador
    //                    mixer.AvailableAtMinute = endTime;
    //                    mixer.CurrentBackboneId = backBoneId;
    //                    remaining -= batchAmount;
    //                    if (capableMixers.Count > 2)
    //                    {
    //                        capableMixers.Remove(mixer);
    //                        capableMixers.Insert(capableMixers.Count, mixer);
    //                    }
    //                }


    //            }



    //            // Paso 6: Programar limpiezas si hay cambio de backbone
    //            foreach (var mixer in engine.Mixers)
    //            {
    //                var scheduledBatches = NewScheduledBatches
    //                    .Where(b => b.MixerContext.Id== mixer.Mixer.MixerId)
    //                    .OrderBy(b => b.StartTimeMinute)
    //                    .ToList();

    //                for (int i = 1; i < scheduledBatches.Count; i++)
    //                {
    //                    var prev = scheduledBatches[i - 1];
    //                    var curr = scheduledBatches[i];

    //                    if (prev.BackBone.Id != curr.BackBone.Id)
    //                    {
    //                        curr.StartTimeMinute += mixer.CleaningTime;
    //                        curr.EndTimeMinute += curr.BatchTime;
    //                    }
    //                }
    //            }
    //            // Paso 11: Ordenar los lotes por tiempo de inicio
    //            ScheduledBatches = ScheduledBatches.OrderBy(b => b.StartTimeMinute).ToList();
    //            // Paso 12: Generar tabla de lotes programados
    //            ScheduledBatchTable = GenerateBatchTable();

    //        }

    //        public ScheduledBatchTable ScheduledBatchTable { get; private set; } = new ScheduledBatchTable();


    //        public ScheduledBatchTable GenerateBatchTable()
    //        {
    //            var table = new ScheduledBatchTable();

    //            // Paso 1: Recopilar todos los tiempos de inicio de lotes
    //            var allStartTimes = NewScheduledBatches
    //                .Select(b => b.StartTimeMinute)
    //                .Distinct()
    //                .OrderBy(t => t);

    //            // Paso 2: Recopilar todos los mezcladores que producen algo
    //            table.Mixers = NewScheduledBatches
    //                .Select(b => b.MixerName)
    //                .Distinct()
    //                .ToList();

    //            // Paso 3: Crear filas por minuto de inicio
    //            foreach (var startTime in allStartTimes)
    //            {
    //                var row = new ScheduledBatchTableRow
    //                {
    //                    StartTimeMinute = startTime,
    //                    MixerBatchInfo = new Dictionary<string, ScheduledBatchTableCell>()
    //                };

    //                // Paso 4: Inicializar todas las columnas con vacío
    //                foreach (var mixer in table.Mixers)
    //                {
    //                    row.MixerBatchInfo[mixer] = new ScheduledBatchTableCell(); // Vacío por defecto
    //                }

    //                // Paso 5: Rellenar solo los lotes que comienzan en este minuto
    //                var batchesThisMinute = NewScheduledBatches
    //                    .Where(b => b.StartTimeMinute == startTime)
    //                    .ToList();

    //                foreach (var batch in batchesThisMinute)
    //                {
    //                    if (!row.MixerBatchInfo.ContainsKey(batch.MixerName))
    //                    {
    //                        row.MixerBatchInfo[batch.MixerName] = new ScheduledBatchTableCell();
    //                    }

    //                    row.MixerBatchInfo[batch.MixerName] = new ScheduledBatchTableCell
    //                    {
    //                        BackBoneId = batch.BackBone.Id,
    //                        BackBoneName = batch.BackBone.Name,
    //                        AmountKg = batch.BatchSize,
    //                        BatchTimeMinutes = batch.EndTimeMinute - batch.StartTimeMinute
    //                    };
    //                }

    //                table.Rows.Add(row);
    //            }

    //            return table;
    //        }



    //    }
    //    public class MixerAssignmentState
    //    {
    //        public Guid MixerId { get; set; }
    //        public string MixerName { get; set; } = string.Empty;
    //        public MixerConfiguration Configuration { get; set; } = null!;
    //        public List<ScheduledBatch> ScheduledBatches { get; set; } = new();
    //        public Guid? CurrentBackboneId { get; set; } = null;
    //        public string CurrentBackboneName { get; set; } = string.Empty;
    //        public double AvailableAtMinute { get; set; }

    //        public MixerContext RealMixer { get; set; } = null!;


    //        public override string ToString() =>
    //            $"Mixer: {MixerName} | Último backbone: {CurrentBackboneName ?? "Ninguno"} | Disponible en: {AvailableAtMinute:F0} min";
    //    }
    //    public class ScheduledBatch
    //    {
    //        public Guid MixerId { get; set; }
    //        public string MixerName { get; set; } = string.Empty;
    //        public Guid BackBoneId { get; set; }
    //        public string BackBoneName { get; set; } = string.Empty;
    //        public double StartTimeMinute { get; set; }
    //        public double EndTimeMinute { get; set; }
    //        public double TotalBatchTime { get; set; }
    //        public double AmountKg { get; set; }

    //        public override string ToString() =>
    //            $"Mixer: {MixerName} | Backbone: {BackBoneName} | {StartTimeMinute}-{EndTimeMinute} min | {AmountKg} kg";


    //    }
}
