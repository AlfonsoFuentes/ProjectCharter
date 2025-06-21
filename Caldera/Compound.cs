using Caldera.Nueva_clases;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnitSystem;

namespace Caldera
{
    public class Compound
    {
        int temperatureformula = 0;
        public string Name { get; init; } = string.Empty;
        public string Formula { get; init; } = string.Empty;

        public MassFlow MassFlow { get; set; } = new MassFlow(MassFlowUnits.Kg_hr);
        public MolarFlow MolarFlow { get; set; } = new MolarFlow(MolarFlowUnits.Kgmol_hr);

        double _Molar_Percentage = 0;
        double _Mass_Percentage = 0;
        public double Molar_Percentage
        {
            get { return _Molar_Percentage; }
            set
            {
                _Molar_Percentage = value;
            }
        }
        public double Mass_Percentage
        {
            get { return _Mass_Percentage; }
            set { _Mass_Percentage = value; }
        }

        public double MolecularWeight { get; set; } // Peso molecular en g/mol

        //Cp=a+b(T)+c(T)^2+d(T)^3
        public double A { get; set; } //a
        public double B { get; set; } //b*102
        public double C { get; set; } //c*105
        public double D { get; set; } //d*109
        public Temperature Temperature { get; set; } = new Temperature(TemperatureUnits.DegreeCelcius);
        public MassEntropy SpecificHeat { get; private set; } = new MassEntropy(MassEntropyUnits.KJ_Kg_C);
        public MassEnergy MassEntalpy { get; private set; } = new MassEnergy(MassEnergyUnits.KJ_Kg); // kJ/kg
        public EnergyFlow EnthalpyFlow { get; private set; } = new EnergyFlow(EnergyFlowUnits.KJ_hr); // kJ/hr
        public Compound(
            string nombre,
            string formula,
            double pm,
            double a, double b, double c, double d, int temperatureformula)
        {
            Name = nombre;
            Formula = formula;
            A = a;
            B = b * 1e-2;
            C = c * 1e-5;
            D = d * 1e-9;
            this.temperatureformula = temperatureformula;
            MolecularWeight = pm;
        }
        public void CalculateEnergyChanges()
        {
            double actualTemp = temperatureformula == 1 ? Temperature.GetValue(TemperatureUnits.Kelvin) : Temperature.GetValue(TemperatureUnits.DegreeCelcius);
            double cpkjmol = A + B * actualTemp + C * Math.Pow(actualTemp, 2) + D * Math.Pow(actualTemp, 3);
            double cpkjgr = cpkjmol / MolecularWeight;

            SpecificHeat.SetValue(cpkjgr, MassEntropyUnits.KJ_Kg_C);
            double TREF = temperatureformula == 1 ? 298.15 : 25;

            double molarenthalpy =
                A * (actualTemp - TREF) +
                B * (Math.Pow(actualTemp, 2) - Math.Pow(TREF, 2)) / 2 +
                C * (Math.Pow(actualTemp, 3) - Math.Pow(TREF, 3)) / 3 +
                D * (Math.Pow(actualTemp, 4) - Math.Pow(TREF, 4)) / 4;
            double massenthalpy = molarenthalpy / MolecularWeight;
            MassEntalpy.SetValue(massenthalpy, MassEnergyUnits.KJ_Kg);


        }
        public void SetMolarFlow(double kgmolhr)
        {
            MolarFlow.SetValue(kgmolhr, MolarFlowUnits.Kgmol_hr);
            MassFlow.SetValue(kgmolhr * MolecularWeight, MassFlowUnits.Kg_hr);
        }


    }
    public abstract class CompoundList
    {
        public virtual List<Compound> List { get; } = new List<Compound>();

        public MassFlow MassFlow { get; set; } = new MassFlow(MassFlowUnits.Kg_hr);
        public MolarFlow MolarFlow { get; set; } = new MolarFlow(MolarFlowUnits.Kgmol_hr);

        public double MolecularWeight { get; set; } // Peso molecular en g/mol
        public Temperature Temperature { get; set; } = new Temperature(TemperatureUnits.DegreeCelcius);
        public Pressure Pressure { get; set; } = new Pressure(PressureUnits.Atmosphere);
        public MassEntropy SpecificHeat { get; private set; } = new MassEntropy(MassEntropyUnits.KJ_Kg_C);
        public MassEnergy MassEntalpy { get; private set; } = new MassEnergy(MassEnergyUnits.KJ_Kg); // kJ/kg
        public EnergyFlow EnthalpyFlow { get; private set; } = new EnergyFlow(EnergyFlowUnits.KJ_hr); // kJ/hr
        public MassDensity Density { get; private set; } = new MassDensity(MassDensityUnits.Kg_m3);
        public void CalculateEnergyChanges()
        {
            double massflow = MassFlow.GetValue(MassFlowUnits.Kg_hr);

            double masscp = 0;
            double massenthalpy = 0;
            foreach (var item in List)
            {
                item.Temperature = Temperature;
                item.CalculateEnergyChanges();
                masscp += item.Mass_Percentage * item.SpecificHeat.GetValue(MassEntropyUnits.KJ_Kg_C);
                massenthalpy += item.Mass_Percentage * item.MassEntalpy.GetValue(MassEnergyUnits.KJ_Kg);
            }
            SpecificHeat.SetValue(masscp, MassEntropyUnits.KJ_Kg_C);
            MassEntalpy.SetValue(massenthalpy, MassEnergyUnits.KJ_Kg);
            double energyflow = massflow * massenthalpy;
            EnthalpyFlow.SetValue(energyflow, EnergyFlowUnits.KJ_hr);
            CalculateDensity();
        }
        public void CalculateMassPercentage()
        {
            double massflow = List.Sum(x => x.MassFlow.GetValue(MassFlowUnits.Kg_hr));
            List.ForEach(x =>
            {
                x.Mass_Percentage = x.MassFlow.GetValue(MassFlowUnits.Kg_hr) / massflow;
            });
            MassFlow.SetValue(massflow, MassFlowUnits.Kg_hr);
        }
        public void CalculateDensity()
        {
            double presion = Pressure.GetValue(PressureUnits.Atmosphere);
            double temperatura = Temperature.GetValue(TemperatureUnits.Kelvin);
            double R = 0.082;//lt*atm/mol K
            double densidad = presion * MolecularWeight / (temperatura * R);
            Density.SetValue(densidad, MassDensityUnits.g_L);

        }
    }
    public class Air : CompoundList
    {
        public Compound O2 = new("Oxigen", "O2", 32, 29.1, 1.158, -0.06076, 1.311, 2);
        public Compound N2 = new("Nitrogen", "N2", 28.02, 29, 0.2199, 0.5723, -2.871, 2);
        public Compound H2O = new("Water", "H2O", 18.016, 33.46, 0.688, 0.7604, -3.593, 2);
        public Air()
        {


            List.Add(O2);
            List.Add(N2);
            List.Add(H2O);
        }
    }
    public class CombustionGases : CompoundList
    {
        public Compound CO2 = new("Carbon Dioxide", "CO2", 44.01, 36.11, 4.233, -2.887, 7.464, 2);
        public Compound O2 = new("Oxigen", "O2", 32, 29.1, 1.158, -0.06076, 1.311, 2);
        public Compound N2 = new("Nitrogen", "N2", 28.02, 29, 0.2199, 0.5723, -2.871, 2);
        public Compound H2O = new("Water", "H2O", 18.016, 33.46, 0.688, 0.7604, -3.593, 2);
        public CombustionGases()
        {


            List.Add(CO2);
            List.Add(O2);
            List.Add(N2);
            List.Add(H2O);
        }
    }
    public class Combustible : Compound
    {

        public MolarFlow O2Required { get; set; } = new MolarFlow(MolarFlowUnits.Kgmol_hr);
        public MolarFlow CO2Produced { get; set; } = new MolarFlow(MolarFlowUnits.Kgmol_hr);
        public MolarFlow H2OProduced { get; set; } = new MolarFlow(MolarFlowUnits.Kgmol_hr);
        public Combustible(
            string nombre,
            string formula,
            double pm,
            double porc_v,
            double molarcombustion,
            double a, double b, double c, double d, int temperatureformula) : base(nombre, formula, pm, a, b, c, d, temperatureformula)
        {
            var massenergy = molarcombustion / pm;
            MassEntalpy.SetValue(massenergy, MassEnergyUnits.KJ_g);
            Molar_Percentage = porc_v;
            SetMolarFlow();



        }
        private (int C, int H, int O) ParsearFormula(string formula)
        {
            int C = 0, H = 0, O = 0;

            if (formula.StartsWith("i") || formula.StartsWith("n"))
            {
                formula = formula.Remove(0, 1);
            }
            if (formula.Contains("CH")) C = 1;
            else if (formula.StartsWith("C2")) C = 2;
            else if (formula.StartsWith("C3")) C = 3;
            else if (formula.StartsWith("C4")) C = 4;
            else if (formula.StartsWith("C5")) C = 5;
            else if (formula.StartsWith("C6")) C = 6;

            var hPart = formula.Split('H');
            if (hPart.Length > 1)
            {
                string hStr = "";
                foreach (char c in hPart[1])
                {
                    if (char.IsDigit(c))
                        hStr += c;
                    else break;
                }

                if (int.TryParse(hStr, out int h))
                    H = h;
            }

            return (C, H, O);
        }
        public void SetMolarFlow(MolarFlow molarflow = null!)
        {
            double localmolarflow = 1;
            if (molarflow != null)
            {
                MolarFlow = molarflow;
                localmolarflow = MolarFlow.GetValue(MolarFlowUnits.gmol_hr);

            }


            double massflow = localmolarflow * MolecularWeight;
            MassFlow.SetValue(massflow, MassFlowUnits.g_hr);
            var (C, H, _) = ParsearFormula(Formula);

            if (C > 0 && H > 0)
            {
                double o2_requerido = localmolarflow * (C + H / 4.0);
                double co2_producido = localmolarflow * C;
                double h2o_producido = localmolarflow * H / 2.0;
                O2Required.SetValue(o2_requerido, MolarFlowUnits.gmol_hr);
                CO2Produced.SetValue(co2_producido, MolarFlowUnits.gmol_hr);
                H2OProduced.SetValue(h2o_producido, MolarFlowUnits.gmol_hr);

            }
        }
    }
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
    public class GasNatural : CombustibleList
    {
        public Combustible metano = new Combustible("Methane", "CH4", 16.04, 0.8309, 890.4, 34.31, 5.469, 0.3661, -11, 2);
        public Combustible etano = new Combustible("Ethane", "C2H6", 30.07, 0.1046, 1559.9, 49.37, 13.92, -5.816, 7.280, 2);
        public Combustible propano = new Combustible("Propane", "C3H8", 44.097, 0.0316, 2220, 68.032, 22.59, -13.11, 31.71, 2);
        public Combustible i_butano = new Combustible("i-Butane", "iC4H10", 58.12, 0.0047, 2868.8, 89.46, 30.13, -18.91, 49.87, 2);
        public Combustible n_butano = new Combustible("n-Butane", "nC4H10", 58.12, 0.0044, 2878.52, 92.30, 27.88, -15.47, 34.98, 2);
        public Combustible i_pentano = new Combustible("i-Pentane", "iC5H12", 72.15, 0.0011, 3536.15, 114.8, 34.09, -18.99, 42.26, 2);
        public Combustible n_pentano = new Combustible("n-Pentane", "nC5H12", 72.15, 0.0005, 3536.15, 114.8, 34.09, -18.99, 42.26, 2);
        public Combustible n_hexano = new Combustible("n-Hexane", "C6H14", 86.18, 0.0003, 4194.753, 137.44, 40.85, -23.92, 57.66, 2);
        public Combustible N2 = new("Nitrogen", "N2", 28.02, 0.00471, 0, 29, 0.2199, 0.5723, -2.871, 2);
        public Combustible CO2 = new("Carbon Dioxide", "CO2", 44.01, 0.01782, 0, 36.11, 4.233, -2.887, 7.464, 2);
        public GasNatural()
        {

            List.Add(metano);
            List.Add(etano);
            List.Add(propano);
            List.Add(i_butano);
            List.Add(n_butano);
            List.Add(n_pentano);
            List.Add(i_pentano);
            List.Add(n_hexano);
            List.Add(N2);
            List.Add(CO2);
            CalculateMolecularWeight();
            CalculateDensityNormalCondictions();
            CalculateEnthalpy();



        }
        public void SetMolarFlow(MolarFlow molarFlow)
        {
            MolarFlow = molarFlow;
            double molarflow = MolarFlow.GetValue(MolarFlowUnits.gmol_hr);
            double massflow = 0;
            foreach (var item in List)
            {
                double molesi = molarflow * item.Molar_Percentage;
                MolarFlow molarflowi = new MolarFlow(molesi, MolarFlowUnits.gmol_hr);
                item.SetMolarFlow(molarflowi);
                double massflowi = item.MassFlow.GetValue(MassFlowUnits.Kg_hr);
                massflow += massflowi;

            }
            MassFlow.SetValue(massflow, MassFlowUnits.Kg_hr);
        }
        void CalculateMolecularWeight()
        {

            double molar = List.Sum(x => x.Molar_Percentage);
            List.ForEach(x =>
            {
                x.Molar_Percentage = x.Molar_Percentage / molar;

            });
            molar = List.Sum(x => x.Molar_Percentage);
            double mass = List.Sum(x => x.Molar_Percentage * x.MolecularWeight);
            List.ForEach(x =>
            {

                x.Mass_Percentage = x.Molar_Percentage * x.MolecularWeight / mass;

            });

            MolecularWeight = mass / molar;
        }
        void CalculateEnthalpy()
        {
            double masentalpy = List.Sum(x => x.Mass_Percentage * x.MassEntalpy.GetValue(MassEnergyUnits.KJ_Kg));


            MassEntalpy.SetValue(masentalpy, MassEnergyUnits.KJ_Kg);
            double densitynormalconditions = DensityNormalConditions.GetValue(MassDensityUnits.Kg_m3);
            double volumenenthalpi = masentalpy * densitynormalconditions;
            VolumeEnthalpy.SetValue(volumenenthalpi, VolumeEnergyUnits.KJ_m3);
            double btup3 = VolumeEnthalpy.GetValue(VolumeEnergyUnits.BTU_ft3);
        }
        void CalculateDensityNormalCondictions()
        {
            double P = 101.325; // kPa – condición normal
            double T = 273.15;  // K – condición normal
            double R = 8.314;   // kJ/(kmol·K)
            double M = MolecularWeight; // g/mol → kg/kmol

            double rho = (P * M) / (R * T); // kg/m³
            DensityNormalConditions.SetValue(rho, MassDensityUnits.Kg_m3);
        }
        public void CalculateEnthalpyFlow()
        {
            double massflow = MassFlow.GetValue(MassFlowUnits.Kg_hr);
            double massentalpy = MassEntalpy.GetValue(MassEnergyUnits.KJ_Kg);
            double entalpyflow = massentalpy * massflow;
            EnthalpyFlow.SetValue(entalpyflow, EnergyFlowUnits.KJ_hr);
        }
    }
    public class MassEnergyBalance
    {
        GasNatural gn = new();
        Air air = new();
        CombustionGases cg = new();

        double FlujoGN_Nm3_hr = 250;
        double ExcesoAire = 50;
        public Length Altura { get; set; } = new Length(1000, LengthUnits.Meter);
        public double HumedadRelativa { get; set; } = 70;
        public double HumedadMolar { get; set; } = 0;
        public Temperature TemperatureAmbiente { get; set; } = new Temperature(25, TemperatureUnits.DegreeCelcius);
        public void Calculate()
        {


            // Paso 1: Calcular moles del gas natural
            const double VOLUMEN_MOLAR_NORMAL = 22.414; // Nm³/kmol
            double molesTotalesGN = FlujoGN_Nm3_hr / VOLUMEN_MOLAR_NORMAL;
            MolarFlow molarFlow = new MolarFlow(molesTotalesGN, MolarFlowUnits.Kgmol_hr);
            gn.SetMolarFlow(molarFlow);

            CalcularComposicionBaseHumedaddesdeBaseSeca(gn.O2Required);
            CalcularGasesdeCombustion();
            CalculateEnergyStreams();

        }

        public Pressure PresionAtmosferica { get; set; } = new Pressure(101.325, PressureUnits.KiloPascal); // Presión atmosférica en kPa
        public double PresionVapor(double Temp)
        {
            double D = 0;
            double temp;

            temp = Temp * 1.8 + 32;
            if (Temp > 0 && Temp < 60)
                D = Math.Pow(10, 8.10765 - 1750.286 / (Temp + 235));
            if (Temp > 61 && Temp < 150)
                D = Math.Pow(10, 7.96681 - 1668.21 / (Temp + 228));

            return D / 760 * 101.3249966;
        }
        void CalcularHumedadMolar()
        {
            double altura = Altura.GetValue(LengthUnits.Meter); // Convertir altura a metros

            double pAtm = 1 / Math.Exp(altura / (8430.15 - altura * 0.09514)) * 101.3249966;
            PresionAtmosferica.SetValue(pAtm, PressureUnits.KiloPascal);
            double tempAtm = TemperatureAmbiente.GetValue(TemperatureUnits.DegreeCelcius);
            double Pv = PresionVapor(tempAtm);//en KiloPascales
            double PH2O = HumedadRelativa * Pv / 100.0;
            HumedadMolar = PH2O / (pAtm);

        }
        public void CalcularComposicionBaseHumedaddesdeBaseSeca(MolarFlow oxigenMolarFlow)
        {
            // 1. Calcular humedad molar del ambiente
            CalcularHumedadMolar();

            // 2. Obtener flujo molar base seca del O2
            double O2FlujoMolar = oxigenMolarFlow.GetValue(MolarFlowUnits.Kgmol_hr) * (1 + ExcesoAire / 100.0);

            // 3. Calcular N2 según proporción estequiométrica del aire (O2:N2 = 21:79)
            double N2FlujoMolar = O2FlujoMolar * 79 / 21;

            // 4. Flujo total de aire seco
            double AireSecoFlujoMolar = O2FlujoMolar + N2FlujoMolar;

            // 5. H2O según humedad molar
            double H2OFlujoMolar = AireSecoFlujoMolar * HumedadMolar;


            // 7. Asignar flujos molares a cada componente
            air.O2.SetMolarFlow(O2FlujoMolar);
            air.N2.SetMolarFlow(N2FlujoMolar);
            air.H2O.SetMolarFlow(H2OFlujoMolar);

            air.CalculateMassPercentage();

            // 9. Calcular porcentajes molares y másicos

        }
        void CalcularGasesdeCombustion()
        {
            double co2producid = gn.CO2Produced.GetValue(MolarFlowUnits.Kgmol_hr);
            double co2inGas = gn.CO2.MolarFlow.GetValue(MolarFlowUnits.Kgmol_hr);
            double o2air = air.O2.MolarFlow.GetValue(MolarFlowUnits.Kgmol_hr);
            double o2teorico = gn.O2Required.GetValue(MolarFlowUnits.Kgmol_hr);
            double n2gas = gn.N2.MolarFlow.GetValue(MolarFlowUnits.Kgmol_hr);
            double n2air = air.N2.MolarFlow.GetValue(MolarFlowUnits.Kgmol_hr);
            double h2oair = air.H2O.MolarFlow.GetValue(MolarFlowUnits.Kgmol_hr);
            double h2oproducido = gn.H2OProduced.GetValue(MolarFlowUnits.Kgmol_hr);

            cg.CO2.SetMolarFlow(co2inGas + co2producid);
            cg.O2.SetMolarFlow(o2air - o2teorico);
            cg.N2.SetMolarFlow(n2gas + n2air);
            cg.H2O.SetMolarFlow(h2oair + h2oproducido);

            cg.CalculateMassPercentage();

        }
        void CalculateEnergyStreams()
        {
            gn.CalculateEnthalpyFlow();
            air.Temperature.SetValue(25, TemperatureUnits.DegreeCelcius);
            air.CalculateEnergyChanges();

            ResolverTemperaturaGases(out double tempSolucionK, out double errorFinal);


        }
        double Objetivefunction(double tempGuessK)
        {
            var combustibleEnergy = gn.EnthalpyFlow.GetValue(EnergyFlowUnits.KJ_hr);
            var airenergy = air.EnthalpyFlow.GetValue(EnergyFlowUnits.KJ_hr);
            cg.Temperature.SetValue(tempGuessK, TemperatureUnits.Kelvin);
            cg.CalculateEnergyChanges();

            double energiaSalida = cg.EnthalpyFlow.GetValue(EnergyFlowUnits.KJ_hr);
            var result = energiaSalida - (combustibleEnergy + airenergy);
            return result;
        }
        void ResolverTemperaturaGases(out double tempSolucionK, out double errorFinal)
        {
            double tempInicialK = 1000;
            NewtonRaphsonSolver.IterarParaTemperatura(
                Objetivefunction,
                tempInicialK,
                out tempSolucionK,
                out errorFinal);
        }
    }
    public static class NewtonRaphsonSolver
    {
        private const double TOL = 1e-6;
        private const int MAX_ITER = 100;

        public static void IterarParaTemperatura(
            Func<double, double> objectiveFunction,
            double tempGuess,
            out double solution,
            out double error)
        {
            double x = tempGuess;

            for (int i = 0; i < MAX_ITER; i++)
            {
                double fx = objectiveFunction(x);
                double dfx = Derivative(objectiveFunction, x);

                if (Math.Abs(dfx) < TOL)
                {
                    // Evitar división por cero
                    solution = double.NaN;
                    error = double.PositiveInfinity;
                    return;
                }

                double dx = -fx / dfx;
                x += dx;

                if (Math.Abs(dx) < TOL)
                {
                    solution = x;
                    objectiveFunction(x);
                    error = Math.Abs(fx);
                    return;
                }
            }

            // Si no converge
            solution = x;
            error = Math.Abs(objectiveFunction(x));
        }

        private static double Derivative(Func<double, double> f, double x)
        {
            double h = 1e-4;
            return (f(x + h) - f(x - h)) / (2 * h);
        }
    }
}
