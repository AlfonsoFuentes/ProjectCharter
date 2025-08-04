using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FinshingLines
{
    public class SimulationStatusDto
    {
        public int CurrentMinute { get; set; }
        public List<MixerStatusDto> Mixers { get; set; } = new();
        public List<BigWipTankStatusDto> BigWipTanks { get; set; } = new();
        public List<ProductionLineStatusDto> ProductionLines { get; set; } = new();
        public List<ScheduledBatchDto> ScheduledBatches { get; set; } = new();
        public CIPStatusDto CIPMaking { get; set; } = new();
        public CIPStatusDto CIPWIp { get; set; } = new();
    }
    public class CIPStatusDto
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
    public class MixerStatusDto
    {
        public Guid MixerId { get; set; }
        public string MixerName { get; set; } = string.Empty;
        public string CurrentBackboneName { get; set; } = string.Empty;
        public Guid? CurrentBackboneId { get; set; }
        public double AvailableAtMinute { get; set; }
        public int BatchsScheduled { get; set; } // Número de lotes programados
        // ✅ Masa total programada por backbone
        public Dictionary<string, double> TotalScheduledByBackbone { get; set; } = new();

        // ✅ Masa total producida por backbone
        public Dictionary<string, double> TotalProducedByBackbone { get; set; } = new();
        // ✅ Añadimos esta propiedad
        public double UsagePercentage => CalculateUsagePercentage();

        public double BatchSizeKg { get; set; } // Tamaño del lote en kg    

        private double CalculateUsagePercentage()
        {
            if (TotalBatchTime <= 0)
                return 0;

            double timeElapsed = TotalBatchTime - RemainingBatchTime;
            return Math.Min(100, (timeElapsed / TotalBatchTime) * 100);
        }
        public double TotalBatchTime { get; set; } // Tiempo total del lote en minutos
        public double RemainingBatchTime { get; set; } // Tiempo restante del lote en minutos
                                                       // ✅ Estado actual del mezclador
        public double RemainingCleaningTime { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
        // ✅ Clase de color como texto
        public string StatusColorClass
        {
            get
            {
                if (RemainingCleaningTime > 0)
                    return "status-cleaning";

                if (RemainingBatchTime > 0)
                    return "status-producing";

                return "status-available";
            }
        }
    }
    public class BigWipTankStatusDto
    {
        public Guid TankId { get; set; }
        public string TankName { get; set; } = string.Empty;
        public Guid BackBoneId { get; set; }
        public string BackBoneName { get; set; } = string.Empty;
        public double CurrentLevelKg { get; set; }
        public double CapacityKg { get; set; }
        public double UsagePercentage => (CurrentLevelKg / CapacityKg) * 100;
        // ✅ Origen de la masa actual

        public string CurrentSourceMixerName { get; set; } = "None";

        // ✅ Destino de la masa actual
        public double PendingToReceivedMass { get; set; }
        public string CurrentDestinationWipTankName { get; set; } = "None";
    }
    public class ProductionLineStatusDto
    {
        public Guid LineId { get; set; }
        public string LineName { get; set; } = string.Empty;
        public string CurrentProduct { get; set; } = string.Empty;
        public double MassProduced { get; set; }
        public double MassPlanned { get; set; }
        public double PendingTime { get; set; }
        public double PendingCleaningTime { get; set; }
        public double PendingFormatChangeTime { get; set; }
        public double ProgressPercentage => MassPlanned == 0 ? 0 : (MassProduced / MassPlanned) * 100;
        public List<WipTankStatusDto> WipTanks { get; set; } = new();
        public string Status { get; set; }=string.Empty;
    }
    public class WipTankStatusDto
    {
        public Guid TankId { get; set; }
        public string TankName { get; set; } = string.Empty;
        public string BackBoneName { get; set; } = string.Empty;
        public double CurrentLevelKg { get; set; }
        public double CapacityKg { get; set; }
        public double UsagePercentage => CapacityKg == 0 ? 0 : (CurrentLevelKg / CapacityKg) * 100;
        public double ReceivedMass { get; set; }
        public double ProducedMass { get; set; }
        public double PendingToReceivedMass { get; set; }
        public double MassPlanned { get; set; }
        public double PendingToCleaning { get; set; }
    }
    public class ScheduledBatchDto
    {
        public Guid MixerId { get; set; }
        public string MixerName { get; set; } = string.Empty;
        public Guid BackBoneId { get; set; }
        public string BackBoneName { get; set; } = string.Empty;
        public double StartTimeMinute { get; set; }
        public double EndTimeMinute { get; set; }
        public double AmountKg { get; set; }
    }
}
