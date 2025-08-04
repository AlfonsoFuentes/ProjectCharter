using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FinshingLines.DPSimulation
{

    public class NewSimulationServices : IDisposable
    {
        private readonly NewSimulationEngine _engine;

        public event Action? OnStatusUpdated;

        public SimulationStatusDto Status { get; private set; } = new();

        public NewSimulationServices(NewSimulationEngine engine)
        {
            _engine = engine;
            _engine.SimulationUpdated += UpdateStatusFromEngine;
            UpdateStatusFromEngine();
        }

        private void UpdateStatusFromEngine()
        {
            Status.CurrentMinute = _engine.CurrentMinute;

            Status.Mixers = _engine.Mixers.Select(m => new MixerStatusDto
            {

                MixerName = m.Name,
                BatchSizeKg = m.CurrentBatch == null ? 0 : Math.Round(m.CurrentBatch.BatchSize),
                CurrentBackboneName = m.CurrentBatch == null ? string.Empty : m.CurrentBatch.BackBone.Name,
                Status = m.CurrentState,
                //AvailableAtMinute = m.AvailableAtMinute,
                TotalBatchTime = m.TotalBatchTime,
                RemainingBatchTime = m.RemainingBatchTime,
                RemainingCleaningTime = m.RemainingCleaningTime,
                // ✅ Masa total programada por backbone

                BatchsScheduled = m.ScheduledBatches.Count,


            }).ToList();

            Status.BigWipTanks = _engine.BigWips.Select(t => new BigWipTankStatusDto
            {

                TankName = t.Name,

                CurrentLevelKg = Math.Round(t.CurrentLevelKg),
                CapacityKg = t.CapacityKg,
                // ✅ Origen de la masa

                CurrentSourceMixerName = t.CurrentSourceMixerName,
                PendingToReceivedMass = Math.Round(t.AmountPendingToReceive),

                CurrentDestinationWipTankName = t.CurrentDestinationWipTankName
            }).ToList();

            Status.ProductionLines = _engine.Lines.Select(l => new ProductionLineStatusDto
            {
                Status=l.CurrentState,
                LineName = l.Name,
                CurrentProduct = l.CurrentSKU == null ? string.Empty : l.CurrentSKU.SKU.Name,
                MassPlanned = l.MassPlanned,
                MassProduced = Math.Round(l.MassProduced),
                PendingTime = Math.Round(l.PendingProductionTime),
                PendingCleaningTime = Math.Round(l.PendingTimeToCleaning),
                PendingFormatChangeTime = Math.Round(l.PendingTimeToChangeFormat),
                WipTanks = l.WIPs.Select(t => new WipTankStatusDto()
                {
                    BackBoneName = t.BackBoneName,
                    CapacityKg = t.CapacityKg,
                    //CurrentBackboneId = t.CurrentBackboneId,
                    CurrentLevelKg = Math.Round(t.CurrentLevelKg),
                    TankName = t.Name,
                    ReceivedMass = Math.Round(t.Received),
                    ProducedMass = Math.Round(t.Produced),
                    PendingToReceivedMass = Math.Round(t.PendingToReceive),
                    MassPlanned = Math.Round(t.ToProduce),
                    PendingToCleaning = Math.Round(t.PendingToCleaning),

                }).ToList()
            }).ToList();

            //Status.ScheduledBatches = _engine.Mixers
            //    .SelectMany(m => m.GetScheduledBatches().Select(b => new ScheduledBatchDto
            //    {
            //        MixerId = m.MixerId,
            //        MixerName = m.Name,
            //        BackBoneId = b.BackBoneId,
            //        BackBoneName = b.BackBoneName,
            //        StartTimeMinute = b.StartTimeMinute,
            //        EndTimeMinute = b.EndTimeMinute,
            //        AmountKg = b.AmountKg
            //    }))
            //    .ToList();
            Status.CIPMaking = new CIPStatusDto()
            {
                Name = _engine.CIPMaking.Name,
                Status = _engine.CIPMaking.StateString,
            };
            Status.CIPWIp = new CIPStatusDto()
            {
                Name = _engine.CIPWIps.Name,
                Status = _engine.CIPWIps.StateString,
            };
            OnStatusUpdated?.Invoke();
        }


        public void Dispose()
        {
            _engine.SimulationUpdated -= UpdateStatusFromEngine;
        }



    }
}
