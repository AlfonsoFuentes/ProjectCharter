using Shared.FinshingLines.DPSimulation.Mixers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shared.FinshingLines.DPSimulation.WIPs
{
    public abstract class BigWipTankInletState
    {
        public abstract void Handle();
        public abstract string CurrentState { get; }
    }
    public abstract class BigWipTankOutletState
    {
        public abstract void Handle();
        public abstract string CurrentState { get; }
    }
    public class StarvedBigWipInletState : BigWipTankInletState
    {
        private readonly BigWipContex _context;

        public override string CurrentState => "Review receive";

        public StarvedBigWipInletState(BigWipContex context)
        {
            _context = context;
        }

        public override void Handle()
        {
            if (_context.InletQueue.Count > 0)
            {
                _context.CurrentInlet = _context.InletQueue.Dequeue();
                _context.InletState = new ReceivingBigWipInletState(_context);
            }

        }
    }
    public class ReceivingBigWipInletState : BigWipTankInletState
    {
        private readonly BigWipContex _context;

        public override string CurrentState => "Receiving";
        double AmountToReceive;
        double InletFlowKg;

        public ReceivingBigWipInletState(BigWipContex context)
        {
            _context = context;
            AmountToReceive = _context.CurrentInlet.Amount;
            InletFlowKg = _context.InletFlow;

        }

        public override void Handle()
        {
            double PendingToReceive = AmountToReceive - _context.CurrentInlet.ReceivedAmount;
            double Inlet = Math.Min(PendingToReceive, InletFlowKg);
            var currentlelvel = _context.CurrentLevelKg + Inlet;
            if (currentlelvel >= _context.CapacityKg)
            {
                _context.InletState = new StarvedReceivingBigWipInletState(_context);
                return;
            }
            _context.CurrentInlet.ReceivedAmount += Inlet;

            _context.CurrentLevelKg += Inlet;
            PendingToReceive -= Inlet;
            if (PendingToReceive <= 0)
            {
                _context.InletState = new StarvedBigWipInletState(_context);
            }


        }
    }
    public class StarvedReceivingBigWipInletState : BigWipTankInletState
    {
        private readonly BigWipContex _context;

        public override string CurrentState => "Receiving";

        public StarvedReceivingBigWipInletState(BigWipContex context)
        {
            _context = context;


        }

        public override void Handle()
        {

            if (_context.CurrentLevelKg <= _context.MinimumLevelToReceiveKg)
            {
                _context.InletState = new ReceivingBigWipInletState(_context);
            }


        }
    }
    public class CleaningBigWipInletState : BigWipTankInletState
    {
        private readonly BigWipContex _context;

        public override string CurrentState => "Cleaning";

        public CleaningBigWipInletState(BigWipContex context)
        {
            _context = context;


        }

        public override void Handle()
        {


        }
    }
    public class ReleaseCleaningBigWipInletState : BigWipTankInletState
    {
        private readonly BigWipContex _context;

        public override string CurrentState => "Release Cleaning";

        public ReleaseCleaningBigWipInletState(BigWipContex context)
        {
            _context = context;


        }

        public override void Handle()
        {


        }
    }
    public class StarvedBigWipOutletState : BigWipTankOutletState
    {
        private readonly BigWipContex _context;

        public override string CurrentState => "Review Send";

        public StarvedBigWipOutletState(BigWipContex context)
        {
            _context = context;
        }

        public override void Handle()
        {
            _context.SetOutletFlow(0); // <-- Registrar el flujo de salida
            if (_context.OutletQueue.Count > 0)
            {
                _context.CurrentOutlet = _context.OutletQueue.Dequeue();
                _context.OutletState = new SendingBigWipOutletState(_context);
            }


        }
    }
    public class SendingBigWipOutletState : BigWipTankOutletState
    {
        private readonly BigWipContex _context;
        // Campo para almacenar la cantidad transferida en el último minuto
        private double _lastOutletAmount = 0;
        // Propiedad pública para acceder a la cantidad del último minuto
        public double LastOutletAmount => _lastOutletAmount;
        public override string CurrentState => "Receiving";
        double AmountToSend;
        double OutletFlowKg;
        double AmountSent = 0;
        double PendingToSend => AmountToSend - AmountSent;
        WipTankLineContext Wip = null!;
        public SendingBigWipOutletState(BigWipContex context)
        {
            _context = context;
            AmountToSend = _context.CurrentOutlet.Amount;
            OutletFlowKg = _context.OutletFlow;
            Wip = _context.CurrentOutlet.Wip;

        }

        public override void Handle()
        {
            _lastOutletAmount = 0; // Reiniciar al inicio del manejo de este minuto
            double Outlet = Math.Min(OutletFlowKg, PendingToSend);

            if (_context.CurrentLevelKg < _context.MinimumLevelKg)
            {
                _context.SetOutletFlow(0); // <-- Registrar el flujo de salida
                _context.OutletState = new StarvedSendByLoLevelBigWipOutletState(_context);
                return;
            }
            if (Outlet > 0)
            {
                _lastOutletAmount = Outlet; // <-- Registrar la cantidad transferida este minuto
                _context.SetOutletFlow(Outlet); // <-- Registrar el flujo de salida
                AmountSent += Outlet;
                _context.CurrentLevelKg -= Outlet; // <-- Esto modifica el nivel del BigWip
                Wip.SetInlet(Outlet); // <-- Esto aumenta el nivel del Wip
                if (_context.CurrentLevelKg < _context.MinimumLevelKg)
                {
                    _context.CurrentOutlet.Amount = PendingToSend;

                    _context.OutletState = new StarvedSendByLoLevelBigWipOutletState(_context);
                }

            }
            if (PendingToSend <= 0)
            {
                Wip.StopReceiving();
                _context.OutletState = new StarvedBigWipOutletState(_context);
                _context.CurrentOutlet = null!;

            }

        }
    }
    public class StarvedSendByLoLevelBigWipOutletState : BigWipTankOutletState
    {
        private readonly BigWipContex _context;

        public override string CurrentState => "Starved by Level";

        public StarvedSendByLoLevelBigWipOutletState(BigWipContex context)
        {
            _context = context;


        }

        public override void Handle()
        {
            _context.SetOutletFlow(0); // <-- Registrar el flujo de salida
            if (_context.CurrentLevelKg > _context.MinimumLevelKg)
            {

                _context.OutletState = new SendingBigWipOutletState(_context);
            }



        }
    }
    public class CleaningBigWipOutletState : BigWipTankOutletState
    {
        private readonly BigWipContex _context;

        public override string CurrentState => "Cleaning";

        public CleaningBigWipOutletState(BigWipContex context)
        {
            _context = context;


        }

        public override void Handle()
        {


        }
    }
    public class ReleaseCleaningBigWipOutletState : BigWipTankOutletState
    {
        private readonly BigWipContex _context;

        public override string CurrentState => "Cleaning";

        public ReleaseCleaningBigWipOutletState(BigWipContex context)
        {
            _context = context;


        }

        public override void Handle()
        {


        }
    }
    public class BigWipContex : ISimulator
    {
        public Guid BigWipId => config == null! ? Guid.Empty : config.TankId;
        public BackBoneConfiguration? CurrentBackBone { get; private set; }
        public Guid? BackboneId => CurrentBackBone?.Id ?? Guid.Empty;
        public string BackBoneName => CurrentBackBone?.Name ?? string.Empty;
        BigWIPTankConfiguration config = null!;
        NewSimulationEngine engine = null!;
        public BigWipTankInletState InletState { get; set; } = null!;
        public BigWipTankOutletState OutletState { get; set; } = null!;
        public string Name => config?.TankName ?? string.Empty;
        public string CurrentState => $"{Name} {CurrentInletState} {CurrentOutletState}";
        public string CurrentInletState => InletState == null ? string.Empty : $"[Inlet: {InletState.CurrentState}]";
        public string CurrentOutletState => InletState == null ? string.Empty : $"[Outlet: {OutletState.CurrentState}]";
        public double CapacityKg { get; set; } = 2000; // Puedes tomarlo de config si existe
        public double MinimumLevelPercentage { get; set; } = 1;
        public double MinimumLevelToReceivePercentage { get; set; } = 80;
        public double CurrentLevelKg { get; set; } = 0;
        public string CurrentSourceMixerName => CurrentInlet?.Mixer?.Name ?? string.Empty;
        public string CurrentDestinationWipTankName => CurrentOutlet?.Wip?.Name ?? string.Empty;
        public double OutletFlow { get; set; } = 0;
        public double InletFlow { get; set; } = 0;
        public double MinimumLevelToReceiveKg => CapacityKg * MinimumLevelToReceivePercentage / 100;
        public double MassNeededtoFillVessel => CapacityKg - CurrentLevelKg ;
        public double MinimumLevelKg => CapacityKg * MinimumLevelPercentage / 100;
        public double MassScheduled { get; set; } = 0; // Cantidad programada para recibir en este minuto
        public double MassReceived { get; set; } = 0; // Cantidad recibida en este minuto
        public Queue<ScheduleOutlet> OutletQueue { get; set; } = new();
        public Queue<ScheduleIntet> InletQueue { get; set; } = new();
        public ScheduleIntet CurrentInlet { get; set; } = null!;
        public ScheduleOutlet CurrentOutlet { get; set; } = null!;
        public double CleaningTime => config == null ? 0 : config.CleaningTime.GetValue(TimeUnits.Minute);
        public double AmountPendingToReceive => InletQueue.Count == 0 ? 0 : InletQueue.Sum(x => x.PendingToReceive);
        private readonly Queue<double> _outletRateHistory = new Queue<double>();

        // Número máximo de muestras a mantener en el historial (por ejemplo, 60 para 1 hora)
        private const int MaxOutletRateHistorySamples = 120;
        public double AverageOutletRateKgPerMinLastHour
        {
            get
            {
                if (_outletRateHistory.Count == 0)
                {
                    return 0.0; // O CurrentOutletRateKgPerMin si prefieres la última muestra
                }
                
                return _outletRateHistory.Average();
               
            }
        }
        public BigWipContex(NewSimulationEngine _engine, BigWIPTankConfiguration _config)
        {
            config = _config;
            engine = _engine;
        }
        public void Init()
        {
            CurrentLevelKg = config.InitialLevelKg;

            CapacityKg = config.Capacity.GetValue(MassUnits.KiloGram);
            MinimumLevelPercentage = config.MinimumTransferLevelKgPercentage;
            InletState = new StarvedBigWipInletState(this);
            OutletState = new StarvedBigWipOutletState(this);
            InletFlow = config.InletMassFlow.GetValue(MassFlowUnits.Kg_min);
            OutletFlow = config.OutletMassFlow.GetValue(MassFlowUnits.Kg_min);
            CurrentBackBone = config.BackBone;
        }

        public void SimulateOneMinute()
        {

            InletState.Handle();
            OutletState.Handle();

        }

        public void GenerateReport()
        {

        }

        public void RequestFromWip(ScheduleOutlet request)
        {
            OutletQueue.Enqueue(request);
        }

        public void ReceiveFromMixer(ScheduleIntet inlet)
        {
            MassScheduled-= inlet.Amount;
            InletQueue.Enqueue(inlet);
        }

        public void Reset()
        {

        }
        public void TerminateCleaning()
        {
            InletState = new ReleaseCleaningBigWipInletState(this);
            OutletState = new ReleaseCleaningBigWipOutletState(this);
        }
        public void SetCleaning()
        {
            CurrentLevelKg = 0;
            InletState = new CleaningBigWipInletState(this);
            OutletState = new CleaningBigWipOutletState(this);
        }
        public void SetOutletFlow(double flow)
        {
            _outletRateHistory.Enqueue(flow);
            if (_outletRateHistory.Count> MaxOutletRateHistorySamples)
            {
                _outletRateHistory.Dequeue(); // Mantener solo las últimas 60 muestras
            }
         
        }

    }
    public class ScheduleIntet
    {
        public MixerContext Mixer { get; set; } = null!;
        public double Amount { get; set; }
        public double ReceivedAmount { get; set; } = 0;
        public double PendingToReceive => Amount - ReceivedAmount;

    }
    public class ScheduleOutlet
    {
        public WipTankLineContext Wip { get; set; } = null!;
        public double Amount { get; set; }

    }


}
