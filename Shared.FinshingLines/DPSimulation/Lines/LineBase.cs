using Shared.FinshingLines.DPSimulation.Mixers;
using Shared.FinshingLines.DPSimulation.WIPs;
using System.ComponentModel;

namespace Shared.FinshingLines.DPSimulation.Lines
{

    public abstract class LineState
    {
        public abstract void Handle();
        public abstract string CurrentState { get; }
    }

    public class ProducingLineState : LineState
    {


        public override string CurrentState => "Producing";

        LineContext _context = null!;

        double massflow = 0;
        public ProducingLineState(LineContext context)
        {
            _context = context;



        }
        public override void Handle()
        {
            massflow = Math.Min(_context.MassFlowKg, _context.MassPending);
            _context.MassProduced += massflow;


            if (_context.MassPending <= 0)
            {
                _context.State = new ChangeProductLineState(_context);

            }
            else if (_context.WipsHasMinimumLevel)
            {

                _context.State = new StarvedByLoLevelLineState(_context);
            }



        }
    }

    public class CleaningLineState : LineState
    {
        public double CleaningTime { get; set; }
        public double CurrentTime { get; set; }
        public override string CurrentState => "Cleaning";

        LineContext _context = null!;
        public CleaningLineState(LineContext context)
        {

            _context = context;
            CleaningTime = _context.CleaningTime;
        }
        public override void Handle()
        {
            CurrentTime++;
            _context.PendingTimeToCleaning = CleaningTime - CurrentTime;
            if (_context.PendingTimeToCleaning <= 0)
            {
                _context.PendingTimeToCleaning = 0;
                _context.State = new ProducingLineState(_context);
            }



        }
    }
    public class InitProductLineState : LineState
    {
        public override string CurrentState => "Changing Backbone";


        LineContext _context = null!;
        public InitProductLineState(LineContext context)
        {

            _context = context;
        }
        public override void Handle()
        {
            if (_context.CurrentSKU != null)
            {
                if (_context.CurrentSKU.MassPlanned > 0)
                {
                    if (_context.InUseWips.Count > 0)
                    {
                        if (_context.WipsHasMinimumLevel)
                        {
                            _context.State = new StarvedByLoLevelLineState(_context);
                        }
                        else
                        {
                            _context.State = new ProducingLineState(_context);
                        }
                    }
                }
                else if (_context.ScheduleCount > 0)
                {
                    _context.State = new ChangeProductLineState(_context);
                }
                return;
            }
            _context.SetWipsToAvailable();
            _context.State = new AvailableLineState(_context);


        }
    }
    public class ChangeProductLineState : LineState
    {
        public override string CurrentState => "Changing Backbone";


        LineContext _context = null!;
        public ChangeProductLineState(LineContext context)
        {

            _context = context;
        }
        public override void Handle()
        {
            if (_context.NextSKU != null)
            {
                _context.CurrentSKU = _context.GetNewSKU();
                _context.NextSKU = _context.PeekNewSKU();
                _context.AssigWipsTanks();
                _context.MassProduced = 0;
                if (_context.IsNextBackBoneSameAsActual)
                {
                    if (_context.CurrentSKU.MassPlanned > 0)
                    {
                        if (_context.InUseWips.Count > 0)
                        {
                            if (_context.WipsHasMinimumLevel)
                            {
                                _context.State = new StarvedByLoLevelLineState(_context);
                            }
                            else
                            {
                                _context.State = new ChangeFormatLineState(_context);
                            }
                        }


                    }
                    else
                    {
                        _context.State = new ChangeProductLineState(_context);
                    }
                }
                else
                {

                    _context.State = new CleaningLineState(_context);


                }
            }
            else
            {
                _context.SetWipsToAvailable();
                _context.State = new AvailableLineState(_context);

            }

        }
    }
    public class AvailableLineState : LineState
    {
        public override string CurrentState => "Available";


        LineContext _context = null!;
        public AvailableLineState(LineContext context)
        {

            _context = context;
            _context.CurrentSKU = null!;
        }
        public override void Handle()
        {


        }
    }
    public class ChangeFormatLineState : LineState
    {
        public override string CurrentState => "Changing format";


        LineContext _context = null!;
        double timetochangeformat = 0;
        double currenttime = 0;
        public ChangeFormatLineState(LineContext context)
        {

            _context = context;

            timetochangeformat = _context.TimeToChangeFormat;
        }
        public override void Handle()
        {
            currenttime++;
            _context.PendingTimeToChangeFormat = timetochangeformat - currenttime;

            if (_context.PendingTimeToChangeFormat <= 0)
            {
                _context.PendingTimeToChangeFormat = 0;
                _context.State = new ProducingLineState(_context);
            }


        }
    }
    public class CleaningWIPsAtInitLineState : LineState
    {
        public override string CurrentState => "Cleaning Wips At Init";


        LineContext _context = null!;
        public CleaningWIPsAtInitLineState(LineContext context)
        {

            _context = context;
            _context.CurrentSKU = null!;
        }
        public override void Handle()
        {
            foreach (var row in _context.InUseWips)
            {
                row.SetCleaning();
            }
            _context.State = new ChangeProductLineState(_context);
        }
    }
    public class StarvedByLoLevelLineState : LineState
    {
        public override string CurrentState => "Starved By Lo level in Wips";


        LineContext _context = null!;
        public StarvedByLoLevelLineState(LineContext context)
        {

            _context = context;

        }
        public override void Handle()
        {
            if (!_context.WipsHasMinimumLevel)
            {
                _context.State = new ProducingLineState(_context);
            }

        }
    }

    /// <summary>
    /// The 'Context' class
    /// </summary>
    public class LineContext : ISimulator
    {
        public List<WipTankLineContext> WIPs { get; set; } = new();
        public List<WipTankLineContext> InUseWips => WIPs.Count == 0 || CurrentSKU == null ? new List<WipTankLineContext>() :
            WIPs.Where(x => CurrentSKU.SKU.ProductBackBones.Any(y => y.BackBone.Id == x.BackBoneId)).ToList();
        public List<WipTankLineContext> NotInUseWips => InUseWips.Count == 0 ? new() : WIPs.Except(InUseWips).ToList();


        public string Name => Line == null ? string.Empty : Line.Name;
        public LineState State { get; set; } = null!;
        NewSimulationEngine Engine = null!;
        public double CleaningTime { get; set; }
        public double TimeToChangeFormat => CurrentSKU == null ? 0 : CurrentSKU.SKU.TimeToChangeFormat;
        // Constructor
        public Queue<NewScheduledSKU> ScheduledSKUs { get; set; } = new();
        public List<NewScheduledSKU> ScheduledSKULists { get; set; } = new();
        public NewScheduledSKU CurrentSKU { get; set; } = null!;
        public NewScheduledSKU NextSKU { get; set; } = null!;
        public ProductionLineConfiguration Line { get; set; } = null!;
        public double MassFlowKg => CurrentSKU == null ? 0 : CurrentSKU.MassFlow;
        public double MassPlanned => CurrentSKU == null ? 0 : CurrentSKU.MassPlanned;
        public double MassProduced { get; set; }
        public bool WipsHasMinimumLevel => InUseWips.Any(x => x.HasReachedMinimumLevel);
        public double MassPending => MassPlanned - MassProduced;
        public double PendingTimeToChangeFormat { get; set; }
        public double PendingTimeToCleaning { get; set; }
        public double PendingProductionTime => MassFlowKg == 0 ? 0 : Math.Round(MassPending / MassFlowKg);
        public bool NextSkuStartCleaningTime => NextSKU == null ? false : !NextProductNeedsCleaning ? false : PendingProductionTime <= NextCleaningTime;
        public double NextCleaningTime => NotInUseWips.Count == 0 ? 0 : NotInUseWips.Sum(x => x.CleaningTime);
        public List<Guid> NextProductId => NextSKU?.SKU?.BackboneIds ?? new();
        public List<Guid> CurrentProductId => CurrentSKU?.SKU?.BackboneIds ?? new();

        public bool HasNextSchedule => NextSKU != null;
        // Verifica si las listas tienen los mismos elementos (sin importar orden)
        public bool IsNextBackBoneSameAsActual =>
      NextProductId.Count == CurrentProductId.Count &&
      (NextProductId.Count == 0 ||
       new HashSet<Guid>(NextProductId).SetEquals(CurrentProductId));

        public bool NextProductNeedsCleaning => !IsNextBackBoneSameAsActual;
        public LineContext(NewSimulationEngine _engine, ProductionLineConfiguration _Line)
        {
            Engine = _engine;
            Line = _Line;
            Line.WIPTanks.ForEach(x =>
            {
                WIPs.Add(new WipTankLineContext(_engine, x, this));
            });
            CleaningTime = _Line.CleaningTime.GetValue(TimeUnits.Minute);

        }

        public void Init()
        {
            WIPs.ForEach(x => x.Init());
            ReadProductionsPlans();
            if (ScheduleCount > 0)
            {
                CurrentSKU = GetNewSKU();
                NextSKU = PeekNewSKU();
                AssigWipsTanks();
                State = new InitProductLineState(this);
            }



        }

        void ReadProductionsPlans()
        {
            double starttime = 0;
            double endtime = 0;
            foreach (var plan in Line.OrderedMassPlannedPerLineProducts)
            {
                starttime = endtime;
                NewScheduledSKU scheduledsku = new();
                scheduledsku.Id = plan.Id;
                scheduledsku.Order = plan.Order;
                scheduledsku.MassPlanned = plan.MassPlanned.GetValue(MassUnits.KiloGram);
                scheduledsku.MassFlow = plan.MassFlow.GetValue(MassFlowUnits.Kg_min);
                scheduledsku.EstimatedRunTime = scheduledsku.MassPlanned / scheduledsku.MassFlow;
                scheduledsku.StartTimeMinute = starttime;
                endtime = scheduledsku.EndTimeMinute;
                scheduledsku.SKU = new();


                scheduledsku.SKU.Id = plan.Product.ProductId;
                scheduledsku.SKU.Name = plan.Product.ProductName;
                scheduledsku.SKU.TimeToChangeFormat = plan.TimeToChangeFormat.GetValue(TimeUnits.Minute);
                foreach (var component in plan.Product.Components)
                {
                    scheduledsku.SKU.ProductBackBones.Add(component);


                }

                ScheduledSKUs.Enqueue(scheduledsku);
                ScheduledSKULists.Add(scheduledsku);

            }

        }
        List<WipTankLineContext> AssignesWIPs => CurrentSKU == null ? new() : WIPs.Where(x => CurrentSKU.SKU.ProductBackBones.Any(y => y.BackBone.Id == x.BackBoneId)).ToList();
        List<WipTankLineContext> NonAssignedWips => CurrentSKU == null ? new() : WIPs.Except(AssignesWIPs).ToList();

        public void AssigWipsTanks()
        {
            var currentsku = CurrentSKU;
            if (currentsku == null)
            {
                State = new AvailableLineState(this);
                return;
            }

            //Caso 1: Los tanques WIPs tienen los backbones asignados segun el primer sku

            currentsku.SKU.ProductBackBones.ForEach(x =>
            {
                var wip = WIPs.FirstOrDefault(y => y.BackBoneId == x.BackBone.Id);
                if (wip != null)
                {
                    wip.BackBoneName = x.BackBone.Name;
                    wip.Received = wip.CurrentLevelKg;
                    wip.ToReceive = currentsku.MassPlanned * x.Percentage / 100;
                    wip.MassFlowKg = currentsku.MassFlow * x.Percentage / 100;
                    wip.ToProduce = currentsku.MassPlanned * x.Percentage / 100;
                    wip.Produced = 0;
                    wip.AssigInitProducingState();
                }
                else if(NonAssignedWips.Count > 0)
                {
                    wip = NonAssignedWips.First();
                    wip.BackBoneId = x.BackBone.Id;
                    wip.BackBoneName = x.BackBone.Name;
                    wip.Received = wip.CurrentLevelKg;
                    wip.ToReceive = currentsku.MassPlanned * x.Percentage / 100;
                    wip.MassFlowKg = currentsku.MassFlow * x.Percentage / 100;
                    wip.ToProduce = currentsku.MassPlanned * x.Percentage / 100;
                    wip.Produced = 0;
                    wip.AssigInitProducingState();
                }


            });
            var localNonAssigned = NonAssignedWips;
            if (NextSKU != null)
            {
                foreach (var backbones in NextSKU.SKU.ProductBackBones)
                {
                    if (localNonAssigned.Any(x => x.BackBoneId == backbones.BackBone.Id))
                    {
                        var wip = localNonAssigned.First(x => x.BackBoneId == backbones.BackBone.Id);
                        wip.BackBoneName = backbones.BackBone.Name;
                        wip.Received = wip.CurrentLevelKg;
                        wip.ToReceive = NextSKU.MassPlanned * backbones.Percentage / 100;
                        wip.MassFlowKg = NextSKU.MassFlow * backbones.Percentage / 100;
                        wip.ToProduce = NextSKU.MassPlanned * backbones.Percentage / 100;
                        wip.Produced = 0;
                        wip.State = new StarvedByScheduleWipLineState(wip);
                        localNonAssigned.Remove(wip);
                    }
                    else
                    {
                        var wip = localNonAssigned.First();
                        wip.BackBoneName = backbones.BackBone.Name;
                        wip.Received = wip.CurrentLevelKg;
                        wip.ToReceive = NextSKU.MassPlanned * backbones.Percentage / 100;
                        wip.MassFlowKg = NextSKU.MassFlow * backbones.Percentage / 100;
                        wip.ToProduce = NextSKU.MassPlanned * backbones.Percentage / 100;
                        wip.BackBoneId = backbones.BackBone.Id;
                        wip.Produced = 0;
                        wip.State = new InitCleaningWipLineState(wip);
                        localNonAssigned.Remove(wip);

                    }
                }
            }
            localNonAssigned.ForEach(x =>
                {
                    x.Reset();
                    x.State = new InitCleaningWipLineState(x);
                });




        }

        public void SimulateOneMinute()
        {
            State.Handle();
            WIPs.ForEach((x) => { x.SimulateOneMinute(); });
        }
        public void GenerateReport()
        {
        }

        public void Reset()
        {

        }

        public string CurrentState => $"{Name} State:{State.CurrentState}";
        public void SetWipsToAvailable()
        {
            InUseWips.ForEach(x => { x.Reset(); });
        }

        public NewScheduledSKU GetNewSKU()
        {
            var result = ScheduledSKUs.Dequeue();
            return result;
        }
        public NewScheduledSKU PeekNewSKU()
        {
            if (ScheduledSKUs.Count == 0) return null!;
            var result = ScheduledSKUs.Peek();
            return result;
        }
        public int ScheduleCount => ScheduledSKUs.Count;
        public void ResetWipsProduced()
        {
            InUseWips.ForEach(x => { x.Produced = 0; });
        }
    }
}
