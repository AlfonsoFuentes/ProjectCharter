using Shared.FinshingLines.DPSimulation.Mixers;
using Shared.FinshingLines.DPSimulation.WIPs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FinshingLines.DPSimulation.CIPs
{
    public abstract class CIPState
    {
        public abstract void Handle();
        public abstract string CurrentState { get; }
    }
    public class CIPAvailableState : CIPState
    {
        public override string CurrentState => "Available";
        CIPContext _context = null!;
        public CIPAvailableState(CIPContext context)
        {
            _context = context;
        }
        public override void Handle()
        {
            if (_context.CIPQueue.Count > 0)
            {
                _context.CurrentCIP = _context.CIPQueue.Dequeue();
                _context.CurrentCIP.SetWashing();
                _context.State = new CIPWashingState(_context);
            }
        }
    }
    public class CIPWashingState : CIPState
    {
        public override string CurrentState => $"Washing {tankname} pending time: {pendingtime}, min";
        CIPContext _context = null!;
        double currentime = 0;
        double cleaningtime = 0;
        string tankname = string.Empty;
        double pendingtime => cleaningtime - currentime;
        public CIPWashingState(CIPContext context)
        {
            _context = context;
            cleaningtime = _context.CurrentCIP.CleaningTime;
            tankname = context.CurrentCIP.TankName;
        }
        public override void Handle()
        {
            currentime++;
            if (currentime >= cleaningtime)
            {
                _context.CurrentCIP.ReleaseVessel();
                _context.State = new CIPAvailableState(_context);
            }
        }
    }
    public class CIPContext : ISimulator
    {
        public string Name { get; set; } = string.Empty;
        public CIPState State { get; set; } = null!;
        public string StateString => State == null! ? string.Empty : State.CurrentState;
        public string CurrentState => StateString;

        public Queue<ScheduledCIP> CIPQueue { get; set; } = new();

        public ScheduledCIP CurrentCIP { get; set; } = null!;
        public void GenerateReport()
        {

        }

        public void Init()
        {
            State = new CIPAvailableState(this);
        }

        public void Reset()
        {

        }

        public void SimulateOneMinute()
        {
            State.Handle();
        }
        public void GetRequestFromBigWip(BigWipContex bigwip)
        {
            ScheduledCIP scheduledCIP = new()
            {
                BigWip = bigwip,


            };
            CIPQueue.Enqueue(scheduledCIP);
        }
        public void GetRequestFromWipLine(WipTankLineContext wip)
        {
            ScheduledCIP scheduledCIP = new()
            {
                Wip = wip,


            };
            CIPQueue.Enqueue(scheduledCIP);
        }
        public void GetRequestFromMixer(MixerContext mixer)
        {
            ScheduledCIP scheduledCIP = new()
            {
                Mixer = mixer,


            };
            CIPQueue.Enqueue(scheduledCIP);
        }

        public class ScheduledCIP
        {
            public WipTankLineContext Wip { get; set; } = null!;
            public BigWipContex BigWip { get; set; } = null!;
            public MixerContext Mixer { get; set; } = null!;
            public double CleaningTime => Wip == null ? BigWip == null ? 0 : BigWip.CleaningTime : Wip.CleaningTime;
            public string TankName => Wip == null ? BigWip == null ? string.Empty : BigWip.Name : Wip.Name;
            public void ReleaseVessel()
            {
                if (Wip != null)
                {
                    Wip.TerminateCleaning();
                    return;
                }
                if (BigWip != null)
                {
                    BigWip.TerminateCleaning();
                    return;
                }
                if (Mixer != null)
                {
                    Mixer.TerminateCleaning();
                }
            }
            public void SetWashing()
            {
                if (Wip != null)
                {
                    Wip.SetCleaning();
                    return;
                }
                if (BigWip != null)
                {
                    BigWip.SetCleaning();
                    return;
                }
                if (Mixer != null)
                {
                    Mixer.SetCleaning();
                }
            }
        }
    }
}
