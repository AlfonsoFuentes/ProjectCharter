using Shared.FinshingLines.DPSimulation.WIPs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FinshingLines.DPSimulation.Mixers
{
    public abstract class MixerState
    {
        public abstract void Handle();
        public abstract string CurrentState { get; }
    }

    public class ProducingMixerState : MixerState
    {

        public override string CurrentState => "Producing";
        double BatchTime { get; set; }
        double CurrenTime { get; set; }
        MixerContext _context = null!;
        public ProducingMixerState(MixerContext context)
        {
            _context = context;
            _context.CurrentBatch = _context.ScheduledBatches.Dequeue();
            BatchTime = _context.CurrentBatch.BatchTime;
            _context.SetStartimeCurrentBatch();
            CurrenTime = 0;
        }
        public override void Handle()
        {
            CurrenTime++;
            _context.RemainingBatchTime = BatchTime - CurrenTime;
            if (CurrenTime >= BatchTime)
            {
                _context.CreateSendToBigWip();
                _context.State = new ChangeBackboneMixerState(_context);
            }

        }
    }

    public class CleaningMixerState : MixerState
    {
        public override string CurrentState => "Cleaning";
        double CleaningTime { get; set; }
        double CurrenTime { get; set; }
        MixerContext _context = null!;
        public CleaningMixerState(MixerContext context)
        {
            CleaningTime = context.CleaningTime;
            CurrenTime = 0;
            _context = context;
            _context.CurrentBatch = _context.ScheduledBatches.Dequeue();
        }
        public override void Handle()
        {
            CurrenTime++;
            _context.RemainingCleaningTime = CleaningTime - CurrenTime;
            if (_context.RemainingCleaningTime <= 0)
            {
                _context.State = new ProducingMixerState(_context);
            }

        }
    }
    public class ChangeBackboneMixerState : MixerState
    {
        public override string CurrentState => "Changing Backbone";


        MixerContext _context = null!;
        public ChangeBackboneMixerState(MixerContext context)
        {

            _context = context;
        }
        public override void Handle()
        {
            if (_context.ScheduledBatches.Count > 0)
            {

                if (_context.CurrentBatch == null)
                {
                    _context.State = new ProducingMixerState(_context);
                }
                else
                {
                    var nextbatch = _context.ScheduledBatches.Peek();
                    if (nextbatch.BackBone.Id != _context.CurrentBatch.BackBone.Id)
                    {
                        _context.State = new CleaningMixerState(_context);
                    }
                    else
                    {
                        _context.State = new ProducingMixerState(_context);
                    }
                }


            }
            else
            {
                _context.State = new AvailableMixer(_context);
            }

        }
    }
    public class AvailableMixer : MixerState
    {
        public override string CurrentState => "Available";


        MixerContext _context = null!;
        public AvailableMixer(MixerContext context)
        {

            _context = context;
            _context.CurrentBatch = null!;
        }
        public override void Handle()
        {
            if (_context.ScheduledBatches.Count > 0)
            {
                _context.State = new ChangeBackboneMixerState(_context);
            }

        }
    }
    public class ReleaseCleaningMixerState : MixerState
    {
        public override string CurrentState => "Cleaning";

        MixerContext _context = null!;
        public ReleaseCleaningMixerState(MixerContext context)
        {

            _context = context;
            _context.CurrentBatch = _context.ScheduledBatches.Dequeue();
        }
        public override void Handle()
        {


        }
    }
    /// <summary>
    /// The 'Context' class
    /// </summary>
    public class MixerContext : ISimulator
    {
        public override string ToString()
        {
            return $"State{CurrentState}: end time:{EndTime}";
        }
        public Guid Id => Mixer == null ? Guid.Empty : Mixer.MixerId;
        public string Name => Mixer == null ? string.Empty : Mixer.Name;
        public MixerState State { get; set; } = null!;
        NewSimulationEngine Engine = null!;
        public double CleaningTime => Mixer == null ? 0 : Mixer.CleaningTime.GetValue(TimeUnits.Minute);
        // Constructor
        public Queue<NewScheduledBatch> ScheduledBatches { get; set; } = new();
        public NewScheduledBatch CurrentBatch { get; set; } = null!;
        public MixerConfiguration Mixer { get; set; } = null!;
        public double TotalBatchTime => CurrentBatch == null ? 0 : CurrentBatch.BatchTime;
        public double EndTime => CurrentBatch == null ? 0 : CurrentBatch.EndTimeMinute;
        public double RemainingBatchTime { get; set; }
        public double RemainingCleaningTime { get; set; }
        public MixerContext(NewSimulationEngine _engine, MixerConfiguration _Mixer)
        {
            Engine = _engine;
            Mixer = _Mixer;


        }
        // Gets or sets the state
        public void Init()
        {

            State = new AvailableMixer(this);
        }
        public void SimulateOneMinute()
        {
            State.Handle();
        }
        public void GenerateReport()
        {
        }
        public void CreateSendToBigWip()
        {
            Engine.CreateSendToBigWip(this);
        }

        public void Reset()
        {

        }
        public void ScheduleBatch(NewScheduledBatch batch)
        {
            ScheduledBatches.Enqueue(batch);

        }
        string StringState => State == null ? string.Empty : $"State:{State.CurrentState}";
        public string CurrentState => $"{Name} {StringState}";

        public void TerminateCleaning()
        {
            State = new ReleaseCleaningMixerState(this);
        }
        public void SetCleaning()
        {
            State = new CleaningMixerState(this);
        }
        public void SetStartimeCurrentBatch()
        {
            if (CurrentBatch != null)
            {
                CurrentBatch.StartTimeMinute = Engine.CurrentMinute;

            }
        }
    }
}
