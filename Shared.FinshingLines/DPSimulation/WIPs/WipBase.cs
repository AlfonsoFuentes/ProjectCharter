using Shared.FinshingLines.DPSimulation.Lines;
using Shared.FinshingLines.DPSimulation.Mixers;
using System.Data;

namespace Shared.FinshingLines.DPSimulation.WIPs
{


    public abstract class WipTankLineState
    {
        public abstract void Handle();
        public abstract string CurrentState { get; }
    }
    public class ConsumingWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Consuming";


        public ConsumingWipLineState(WipTankLineContext context)
        {
            _context = context;

        }

        public override void Handle()
        {

            _context.SetOutlet();
            if (_context.HasMinimumLevelToRequest)
            {
                if (_context.PendingToReceive > 0)
                {
                    _context.RequestTransferFromBigWip();
                    _context.State = new ReceivingWipLineState(_context);
                    return;
                }
                else if (_context.HasNextSchedule)
                {
                    if (_context.IsNextBackBoneSameAsActual)
                    {
                        _context.ToReceive = _context.NextSkuPendingToReceive;
                        _context.Received = 0;
                        _context.RequestTransferFromBigWip();
                        _context.State = new ReceivingWipLineState(_context);
                        return;
                    }

                }
                _context.State = new EmptyVesselWipLineState(_context);




            }



        }
    }
    public class ReceivingWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Consuming reaching Lo level";

        public ReceivingWipLineState(WipTankLineContext context)
        {
            _context = context;

        }

        public override void Handle()
        {
            _context.SetOutlet();
            if (!_context.ReceivingScheduled)
            {
                _context.State = new ConsumingWipLineState(_context);
            }

        }
    }
    public class LoLoLevelWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Stopped by Lo level, waiting for backbone";
        public LoLoLevelWipLineState(WipTankLineContext context)
        {
            _context = context;
        }

        public override void Handle()
        {

            if (_context.HasReachedMinimumLevel)
            {
                if (_context.PendingToReceive > 0)
                {

                }
            }
            else
            {
                _context.State = new ConsumingWipLineState(_context);
            }
        }
    }
    public class EmptyVesselWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Consuming until empty";

        public EmptyVesselWipLineState(WipTankLineContext context)
        {
            _context = context;
        }

        public override void Handle()
        {

            _context.SetOutlet();
            if (_context.CurrentLevelKg <= 0)
            {
                _context.State = new AvailablelWipLineState(_context);
            }
        }
    }
    public class AvailablelWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Cleaned available to produced";

        public AvailablelWipLineState(WipTankLineContext context)
        {
            _context = context;
        }

        public override void Handle()
        {


        }
    }
    public class InitLoLoLevelWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Checking product availability";

        public InitLoLoLevelWipLineState(WipTankLineContext context)
        {
            _context = context;
        }

        public override void Handle()
        {
            if (_context.HasMinimumLevelToRequest)
            {
                _context.RequestTransferFromBigWip();
                _context.State = new ReceivingWipLineState(_context);
            }
            else
            {
                _context.State = new ConsumingWipLineState(_context);
            }

        }
    }
    public class ReleaseCleaningWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => "Releasing Cleaning";

        public ReleaseCleaningWipLineState(WipTankLineContext context)
        {
            _context = context;
        }

        public override void Handle()
        {

            _context.State = new InitLoLoLevelWipLineState(_context);
        }
    }

    public class CleaningWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Cleaning, pending time: {pendingtime}, min";
        double cleaningTime = 0;
        double currentime = 0;
        double pendingtime = 0;
        public CleaningWipLineState(WipTankLineContext context)
        {
            _context = context;
            cleaningTime = context.CleaningTime;
        }

        public override void Handle()
        {
            currentime++;
            pendingtime = cleaningTime - currentime;
            _context.PendingToCleaning = pendingtime;
            if (_context.PendingToCleaning <= 0)
            {
                _context.State = new AvailablelWipLineState(_context);

            }
        }
    }
    public class StatvedCleaningWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Waiting to Cleaning";

        public StatvedCleaningWipLineState(WipTankLineContext context)
        {
            _context = context;

        }

        public override void Handle()
        {

        }
    }
    public class InitCleaningWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Init Cleaning";

        public InitCleaningWipLineState(WipTankLineContext context)
        {
            _context = context;
        }

        public override void Handle()
        {

            _context.RequestCleaningFromCIP();

            _context.State = new StatvedCleaningWipLineState(_context);

        }
    }
    public class StarvedByScheduleWipLineState : WipTankLineState
    {
        private readonly WipTankLineContext _context;

        public override string CurrentState => $"Init Cleaning";

        public StarvedByScheduleWipLineState(WipTankLineContext context)
        {
            _context = context;
        }

        public override void Handle()
        {
            if (_context.PendingToReceive > 0 && !_context.ReceivingScheduled && !_context.Receiving && !_context.HasMaximunLevel)
            {
                _context.RequestTransferFromBigWip();

            }

        }
    }
    public class WipTankLineContext : ISimulator
    {

        public Guid? BackBoneId { get; set; }
        public string BackBoneName { get; set; } = string.Empty;
        public Guid TankId => config?.TankId ?? Guid.Empty;
        public string Name => config?.Name ?? string.Empty;
        public double CapacityKg { get; set; } = 2000; // Puedes tomarlo de config si existe
        public double LoLevel { get; set; } = 50;
        public double LoLoLevel { get; set; } = 20;
        public double CurrentLevelKg { get; set; } = 0;
        public double MassFlowKg { get; set; }
        public double MinimumLevelToRequest => CapacityKg * LoLevel / 100;
        public double MinimumLevelKg => CapacityKg * LoLoLevel / 100;
        public WipTankLineState State { get; set; } = null!;
        public bool HasMinimumLevelToRequest =>
           CurrentLevelKg <= MinimumLevelToRequest;
        public bool HasMaximunLevel =>
           CurrentLevelKg >= CapacityKg;
        public bool HasReachedMinimumLevel => State is EmptyVesselWipLineState ? false : CurrentLevelKg <= MinimumLevelKg;
        public double CleaningTime { get; set; }
        public bool Receiving { get; set; }
        public bool ReceivingScheduled { get; set; }
        public double Produced { get; set; } = 0;
        public double ToProduce { get; set; } = 0;
        public double ToReceive { get; set; }
        public double Received { get; set; }
        public double PendingToReceive => Math.Round(ToReceive - Received);
        public double PendingToProduce => Math.Round(ToProduce - Produced);
        public bool IsNextBackBoneSameAsActual => Line.IsNextBackBoneSameAsActual;
        public double PendingToCleaning { get; set; }
        public NewScheduledSKU NextSKU => Line.NextSKU;
        public double NextSkuPendingToReceive => NextSKU == null || BackBoneId == null ? 0 : NextSKU.GetMassPlannedByBackBone(BackBoneId.Value);
        public bool HasNextSchedule => Line.HasNextSchedule;
        public bool IsLineProducing => Line == null ? false : Line.State is ProducingLineState;
        public bool IsLineHasThisBackBone => Line == null || Line.CurrentSKU == null ? false : Line.CurrentSKU.SKU.ProductBackBones.Any(x => x.BackBone.Id == BackBoneId);
        public string CurrentState => $"{Name} {BackBoneName} [{State.CurrentState}]";
        WIPtankForLineConfiguration? config = null!;
        NewSimulationEngine engine = null!;
        public LineContext Line { get; set; } = null!;
        public double InitialMassToFillWipKg => CapacityKg - CurrentLevelKg;

        public WipTankLineContext(NewSimulationEngine _engine, WIPtankForLineConfiguration _config, LineContext _line)
        {
            config = _config;
            engine = _engine;
            BackBoneId = _config.Id;
            Line = _line;
        }

        public void Init()
        {
            CapacityKg = config!.Capacity.GetValue(MassUnits.KiloGram);
            CurrentLevelKg = config.InitialLevelKg;
            LoLoLevel = config.MinimumLevelPercentage;
            CleaningTime = config.CleaningTime.GetValue(TimeUnits.Minute);


        }

        public void AssigInitProducingState()
        {
            if (HasReachedMinimumLevel)
            {
                State = new InitLoLoLevelWipLineState(this);
                return;
            }

            State = new ConsumingWipLineState(this);
        }
        public void AssignAvailable()

        {
            State = new AvailablelWipLineState(this);
        }
        public void SimulateOneMinute()
        {

            State.Handle();
        }
        public void SetInlet(double inlet)
        {
            Receiving = true;
            CurrentLevelKg += inlet;
            Received += inlet;


        }
        public void StopReceiving()
        {
            Receiving = false;
            ReceivingScheduled = false;
        }
        public void SetOutlet()
        {
            if (IsLineProducing && IsLineHasThisBackBone)
            {

                CurrentLevelKg -= MassFlowKg;
                Produced += MassFlowKg;

            }
        }


        public void GenerateReport() { }

        public void RequestTransferFromBigWip()
        {

            var amountneed = CapacityKg - CurrentLevelKg;
            amountneed = Math.Min(amountneed, PendingToReceive);
            engine.CreateTransferToWipLine(this, amountneed);
            ReceivingScheduled = true;
        }

        public void Reset()
        {

            BackBoneId = null;
            BackBoneName = string.Empty;
            ToReceive = 0;
            Received = 0;
            MassFlowKg = 0;
            CurrentLevelKg = 0;
            ToProduce = 0;
            Produced = 0;

        }
        public void TerminateCleaning()
        {
            State = new ReleaseCleaningWipLineState(this);
        }
        public void SetCleaning()
        {
            CurrentLevelKg = 0;
            State = new CleaningWipLineState(this);
        }
        public void RequestCleaningFromCIP()
        {
            engine.CreateWashingFromWipLine(this);
        }
    }
}

