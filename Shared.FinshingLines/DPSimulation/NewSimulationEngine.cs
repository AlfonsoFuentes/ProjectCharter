using Shared.FinshingLines.DPSimulation.CIPs;
using Shared.FinshingLines.DPSimulation.Lines;
using Shared.FinshingLines.DPSimulation.Mixers;
using Shared.FinshingLines.DPSimulation.Schedulers;
using Shared.FinshingLines.DPSimulation.WIPs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FinshingLines.DPSimulation
{
    public class NewSimulationEngine : ISimulator
    {
        public ReadSimulationFromDatabase Database { get; set; } = new();
        public List<NewScheduledBatch> ScheduledBatches { get; set; } = new();
        public List<NewScheduledSKU> ScheduledSKUs { get; set; } = new();
        public string CurrentState => throw new NotImplementedException();
        public List<BigWipContex> BigWips { get; set; } = new();
        public List<MixerContext> Mixers { get; set; } = new();
        public List<LineContext> Lines { get; set; } = new();
      
        public DynamicProductionScheduler DynamicScheduler { get; private set; } // Se inicializa en el constructor

     
        public CIPContext CIPWIps { get; set; } = new();
        public CIPContext CIPMaking { get; set; } = new();
        public NewSimulationEngine(ReadSimulationFromDatabase _database)
        {
            Database = _database;
            Database.BigWIPTanks.ForEach(b => { BigWips.Add(new BigWipContex(this, b)); });
            Database.Mixers.ForEach(m => { Mixers.Add(new MixerContext(this, m)); });
            Database.ProductionLines.ForEach(m => { Lines.Add(new LineContext(this, m)); });
      
            DynamicScheduler = new DynamicProductionScheduler(this, Database);
        }
        public void GenerateReport()
        {
            Mixers.ForEach(m => m.GenerateReport());
            BigWips.ForEach(m => m.GenerateReport());
            Lines.ForEach(m => m.GenerateReport());

        }

        public void Init()
        {
            Lines.ForEach(m => m.Init());
     
            Mixers.ForEach(m => m.Init());
            BigWips.ForEach(m => m.Init());
            DynamicScheduler.Init();
            CIPWIps.Init();
            CIPMaking.Init();
           

        }

        public void SimulateOneMinute()
        {
            if (!IsRunning || IsPaused)
                return;

           
            // Planificación dinámica - Llamar en cada minuto, el scheduler decide cuándo actuar
            Console.WriteLine($"[Engine] Llamando a DynamicScheduler en el minuto {CurrentMinute}");
            
            CurrentMinute++;
            CIPWIps.SimulateOneMinute();
            CIPMaking.SimulateOneMinute();
            Mixers.ForEach(m => m.SimulateOneMinute());
            BigWips.ForEach(m => m.SimulateOneMinute());
            Lines.ForEach(m => m.SimulateOneMinute());
            DynamicScheduler.ScheduleJustInTime(CurrentMinute);
            NotifyStatusUpdated();
        }
        public void CreateSendToBigWip(MixerContext mixer)
        {
            var backbone = mixer.CurrentBatch.BackBone;
            if (backbone != null)
            {
                var wip = BigWips.FirstOrDefault(x => x.BackboneId == backbone.Id);
                if (wip != null)
                {
                    ScheduleIntet schedule = new ScheduleIntet()
                    {
                        Amount = mixer.CurrentBatch.BatchSize,
                        Mixer = mixer,
                    };
                    wip.ReceiveFromMixer(schedule);
                }
            }
        }
        public void CreateTransferToWipLine(WipTankLineContext wipline, double amount)
        {
            var backbone = wipline.BackBoneId;
            if (backbone != null)
            {
                var wip = BigWips.FirstOrDefault(x => x.BackboneId == backbone);

                if (wip != null)
                {
                    if (wip.OutletQueue.Any(x => x.Wip.TankId == wipline.TankId))
                    {
                        return;
                    }
                       
                    ScheduleOutlet schedule = new ScheduleOutlet()
                    {
                        Amount = amount,
                        Wip = wipline,
                    };
                    wip.RequestFromWip(schedule);
                }
            }
        }
        public void CreateWashingFromWipLine(WipTankLineContext wip)
        {
            CIPWIps.GetRequestFromWipLine(wip);
        }
        public void CreateWashingFromBigWipTan(BigWipContex wip)
        {
            CIPWIps.GetRequestFromBigWip(wip);
        }
        public void CreateWashingFromMixer(MixerContext mixer)
        {
            CIPMaking.GetRequestFromMixer(mixer);

        }
        public event Action? SimulationUpdated;
        public int CurrentMinute { get; private set; } = 0;
        public bool IsRunning { get; private set; } = false;
        public bool IsPaused { get; private set; } = false;
        private void NotifyStatusUpdated()
        {
            SimulationUpdated?.Invoke();
        }
        public void Start()
        {
            IsRunning = true;
            IsPaused = false;
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void Stop()
        {
            IsRunning = false;
            IsPaused = false;
            CurrentMinute = 0;
            ResetAll();
        }

        private void ResetAll()
        {
            CurrentMinute = 0;
            foreach (var mixer in Mixers)
            {
                mixer.Reset();
            }

            foreach (var tank in BigWips)
            {
                tank.Reset();
            }

            foreach (var line in Lines)
            {
                line.Reset();
            }
        }

        public void Reset()
        {
            ResetAll();
        }
    }
    public interface ISimulator
    {
        void Init();
        void SimulateOneMinute();
        void GenerateReport();
        string CurrentState { get; }
        void Reset();
    }
}
