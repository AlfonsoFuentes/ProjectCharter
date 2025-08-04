using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FinshingLines
{
    public enum MassUnits
    {
        KiloGram,
        Gram
    }

    public enum TimeUnits
    {
        Minute,
        Hour
    }

    public enum LineVelocityUnits
    {
        EA_min
    }

    public enum MassFlowUnits
    {
        Kg_min,
        g_min
    }
    public class Mass
    {
        public double Value { get; set; }
        public MassUnits Unit { get; set; }

        public Mass(double value, MassUnits unit)
        {
            Value = value;
            Unit = unit;
        }

        public double GetValue(MassUnits targetUnit)
        {
            if (Unit == targetUnit) return Value;
            if (Unit == MassUnits.Gram && targetUnit == MassUnits.KiloGram) return Value / 1000;
            if (Unit == MassUnits.KiloGram && targetUnit == MassUnits.Gram) return Value * 1000;
            return Value;
        }

        public override string ToString() => $"{Value:F2} {Unit}";
    }

    public class Time
    {
        public double Value { get; set; }
        public TimeUnits Unit { get; set; }

        public Time(double value, TimeUnits unit)
        {
            Value = value;
            Unit = unit;
        }

        public double GetValue(TimeUnits targetUnit)
        {
            if (Unit == targetUnit) return Value;
            if (Unit == TimeUnits.Hour && targetUnit == TimeUnits.Minute) return Value * 60;
            if (Unit == TimeUnits.Minute && targetUnit == TimeUnits.Hour) return Value / 60;
            return Value;
        }

        public override string ToString() => $"{Value:F2} {Unit}";
    }

    public class LineVelocity
    {
        public double Value { get; set; }
        public LineVelocityUnits Unit { get; set; }

        public LineVelocity(double value, LineVelocityUnits unit)
        {
            Value = value;
            Unit = unit;
        }

        public double GetValue(LineVelocityUnits targetUnit)
        {
            return Value;
        }

        public override string ToString() => $"{Value:F2} {Unit}";
    }

    public class MassFlow
    {
        public double Value { get; set; }
        public MassFlowUnits Unit { get; set; }

        public MassFlow(double value, MassFlowUnits unit)
        {
            Value = value;
            Unit = unit;
        }

        public double GetValue(MassFlowUnits targetUnit)
        {
            return Value;
        }

        public override string ToString() => $"{Value:F2} {Unit}";
    }
}
