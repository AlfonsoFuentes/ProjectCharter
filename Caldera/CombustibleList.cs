using UnitSystem;

namespace Caldera
{
    public class CombustibleList
    {
        public MassFlow MassFlow { get; set; } = new MassFlow(MassFlowUnits.Kg_hr);
        public MolarFlow MolarFlow { get; set; } = new MolarFlow(MolarFlowUnits.Kgmol_hr);

        public double MolecularWeight { get; set; } // Peso molecular en g/mol
        public Temperature Temperature { get; set; } = new Temperature(TemperatureUnits.DegreeCelcius);
        public Pressure Pressure { get; set; } = new Pressure(PressureUnits.Atmosphere);
        public MassEntropy SpecificHeat { get; private set; } = new MassEntropy(MassEntropyUnits.KJ_Kg_C);
        public MassEnergy MassEntalpy { get; private set; } = new MassEnergy(MassEnergyUnits.KJ_Kg); // kJ/kg
        public EnergyFlow EnthalpyFlow { get; private set; } = new EnergyFlow(EnergyFlowUnits.KJ_hr); // kJ/hr
        public MassDensity Density { get; private set; } = new MassDensity(MassDensityUnits.Kg_m3);
        public MassDensity DensityNormalConditions { get; private set; } = new MassDensity(MassDensityUnits.Kg_m3);
        public VolumeEnergy VolumeEnthalpy { get; private set; } = new VolumeEnergy(VolumeEnergyUnits.KJ_m3);
        public List<Combustible> List { get; set; } = new();

        public MolarFlow O2Required => new MolarFlow(List.Sum(x => x.O2Required.GetValue(MolarFlowUnits.Kgmol_hr)), MolarFlowUnits.Kgmol_hr);
        public MolarFlow CO2Produced => new MolarFlow(List.Sum(x => x.CO2Produced.GetValue(MolarFlowUnits.Kgmol_hr)), MolarFlowUnits.Kgmol_hr);
        public MolarFlow H2OProduced => new MolarFlow(List.Sum(x => x.H2OProduced.GetValue(MolarFlowUnits.Kgmol_hr)), MolarFlowUnits.Kgmol_hr);


    }
}
