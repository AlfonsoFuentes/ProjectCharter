using Shared.Models.FileResults.Generics.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.FinshingLines
{
    public class MixerConfiguration
    {
        public Guid MixerId { get; set; }
        public string Name { get; set; } = string.Empty;

        public Time CleaningTime { get; set; } = new(0, TimeUnits.Minute);
        public List<BackboneCapability> Capabilities { get; set; } = new();

        public override string ToString() =>
            $"Mixer: {Name} |  Cleaning Time: {CleaningTime.GetValue(TimeUnits.Minute):F2} min";
    }

    public class RequiredBackbone
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double RequiredKg { get; set; }
        public int Priority { get; set; } = 1; // 1 = normal, 2 = alto, 3 = crítico
        public int Order { get; set; } = 0; // Orden de prioridad para el procesamiento 
        public double EstimatedStartMinute { get; set; } = int.MaxValue;
        public override string ToString()
        {
            return $"Backbone: {Name}  | Requerido: {RequiredKg:F2} kg";
        }
    }
    public class BackBoneConfiguration
    {
        public int Order { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Guid> PreferredMixerIds { get; set; } = new List<Guid>();
        public override string ToString() => $"Backbone: {Name} (Order: {Order})";
    }
    public class BackboneCapability
    {
        public BackBoneConfiguration BackBone { get; set; } = new();
        public Time BatchTime { get; set; } = new(0, TimeUnits.Minute);
        public Mass Capacity { get; set; } = new(0, MassUnits.KiloGram);
        public override string ToString() =>
            $"Backbone: {BackBone.Name} | Batch Time: {BatchTime.GetValue(TimeUnits.Minute):F2} min";
    }
    public class ProductConfiguration
    {

        public BackBoneConfiguration BackBone { get; set; } = new();
        public double Percentage { get; set; }

        public override string ToString() =>
            $"Component: {BackBone.Name} | {Percentage:F2}%";
    }
    public class ProductBaseConfiguration
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public List<ProductConfiguration> Components { get; set; } = new();
    }

    public class BigWIPTankConfiguration
    {
        public Guid TankId { get; set; }
        public string TankName => $"Big WIP {BackBone.Name}";
        public Mass Capacity { get; set; } = new(0, MassUnits.KiloGram);
        public MassFlow InletMassFlow { get; set; } = new(0, MassFlowUnits.Kg_min);
        public MassFlow OutletMassFlow { get; set; } = new(0, MassFlowUnits.Kg_min);
        public BackBoneConfiguration BackBone { get; set; } = new();
        public double MinimumTransferLevelKgPercentage { get; set; } = 5; // 20% por defecto
        public double InitialLevelKg { get; set; } = 0; // Nivel inicial en kg  
        public Time CleaningTime { get; set; } = new(180, TimeUnits.Minute); // 3 horas por defecto
        public override string ToString() =>
            $"Tank: {TankName} | Capacity: {Capacity.GetValue(MassUnits.KiloGram):F2} kg | Backbone: {BackBone.Name}";
    }
    public class WIPtankForLineConfiguration
    {
        public Guid TankId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double MinimumLevelPercentage { get; set; } = 20; // 20% por defecto
        public double InitialLevelKg { get; set; } = 0; // Nivel inicial en kg  
        public override string ToString() => $"WIP Tank: {Name}";
        public Time CleaningTime { get; set; } = new(180, TimeUnits.Minute); // 3 horas por defecto
        public Mass Capacity { get; set; } = new Mass(2000, MassUnits.KiloGram);
        public Guid? Id { get; set; }
    }
    public class ProductionLineConfiguration
    {
        public Guid LineId { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<WIPtankForLineConfiguration> WIPTanks { get; set; } = new();
        public Time CleaningTime { get; set; } = new Time(1, TimeUnits.Minute);
        public Time FormatChangeTime { get; set; } = new Time(1, TimeUnits.Minute);
        public override string ToString() =>
            $"Production Line: {Name} | WIP Tanks: {WIPTanks.Count}";
        public List<MassPlannedPerLineProductBaseConfiguration> MassPlannedPerLineProducts { get; set; } = new();

        public List<MassPlannedPerLineProductBaseConfiguration> OrderedMassPlannedPerLineProducts =>
            MassPlannedPerLineProducts.OrderBy(x => x.Order).ToList();
    }

    public class MassPlannedPerLineProductBaseConfiguration
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Order { get; set; }
        public Mass MassPlanned { get; set; } = new(0, MassUnits.KiloGram);
        public ProductBaseConfiguration Product { get; set; } = new();
        public LineVelocity LineVelocity { get; set; } = new LineVelocity(0, LineVelocityUnits.EA_min);
        public Mass FormatMass { get; set; } = new Mass(0, MassUnits.Gram);
        public MassFlow MassFlow => new MassFlow(
            FormatMass.GetValue(MassUnits.KiloGram) * LineVelocity.GetValue(LineVelocityUnits.EA_min),
            MassFlowUnits.Kg_min);
        public double RunTimeMinutes =>
            MassPlanned.GetValue(MassUnits.KiloGram) / MassFlow.GetValue(MassFlowUnits.Kg_min);
        public double InitRunTimeMinutes { get; set; }
        public Time TimeToChangeFormat { get; set; } = new Time(60, TimeUnits.Minute);
        public override string ToString() =>
            $"Product: {Product.ProductName} | Planned: {MassPlanned.GetValue(MassUnits.KiloGram):F2} kg | Speed: {LineVelocity.GetValue(LineVelocityUnits.EA_min):F2} units/min";
    }
    public class ReadSimulationFromDatabase : IResponse

    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public override string ToString() => $@"
Simulation Configuration:
- Backbones: {BackBones.Count}
- Mixers: {Mixers.Count}
- Products: {Products.Count}
- BigWIP Tanks: {BigWIPTanks.Count}
- Production Lines: {ProductionLines.Count}

";
        public List<BackBoneConfiguration> BackBones { get; set; } = new();
        public List<ProductBaseConfiguration> Products { get; set; } = new();
        public List<MixerConfiguration> Mixers { get; set; } = new();

        public List<BigWIPTankConfiguration> BigWIPTanks { get; set; } = new();
        public List<ProductionLineConfiguration> ProductionLines { get; set; } = new();

        public void ReadDatabase()
        {
            CreateBackBones();
            CreateProductBase();
            CreateBigWIPTanks();
            CreateMixers();
            DefinePrefredMixers();
            CreateProductionLines();

        }
        public void DefinePrefredMixers()
        {
            var broglyMixer = Mixers.FirstOrDefault(m => m.Name == "Brogly");
            var frymaMixer = Mixers.FirstOrDefault(m => m.Name == "Fryma");
            var liYuan1Mixer = Mixers.FirstOrDefault(m => m.Name == "Li Yuan 1");
            var liYuan2Mixer = Mixers.FirstOrDefault(m => m.Name == "Li Yuan 2");
            var tripleAccionBlanco = BackBones.FirstOrDefault(b => b.Name == "Triple Accion Blanco");
            var tripleAccionAzul = BackBones.FirstOrDefault(b => b.Name == "Triple Accion Azul");
            var tripleAccionVerde = BackBones.FirstOrDefault(b => b.Name == "Triple Accion Verde");
            tripleAccionBlanco!.PreferredMixerIds = new List<Guid> {
            liYuan1Mixer?.MixerId ?? Guid.Empty,
            liYuan2Mixer?.MixerId ?? Guid.Empty,
            broglyMixer?.MixerId ?? Guid.Empty
        }.Where(id => id != Guid.Empty).ToList(); // Filtrar IDs vacíos si el mixer no se encontró
            tripleAccionAzul!.PreferredMixerIds = new List<Guid> {
            broglyMixer?.MixerId ?? Guid.Empty,
            frymaMixer?.MixerId ?? Guid.Empty
        }.Where(id => id != Guid.Empty).ToList();
            tripleAccionVerde!.PreferredMixerIds = new List<Guid> {
            frymaMixer?.MixerId ?? Guid.Empty,
            broglyMixer?.MixerId ?? Guid.Empty
        }.Where(id => id != Guid.Empty).ToList();
        }
        void CreateBackBones()
        {
            BackBones.Add(new BackBoneConfiguration
            {
                Order = 0,
                Id = Guid.NewGuid(),
                Name = "Triple Accion Blanco",


            });
            BackBones.Add(new BackBoneConfiguration
            {
                Order = 1,
                Id = Guid.NewGuid(),
                Name = "Triple Accion Azul",

            });
            BackBones.Add(new BackBoneConfiguration
            {
                Order = 2,
                Id = Guid.NewGuid(),
                Name = "Triple Accion Verde",

            });
            BackBones.Add(new BackBoneConfiguration
            {
                Order = 3,
                Id = Guid.NewGuid(),
                Name = "Extrablancura blanco",

            });

            BackBones.Add(new BackBoneConfiguration
            {
                Order = 4,
                Id = Guid.NewGuid(),
                Name = "Extrablancura Azul claro",

            });

            BackBones.Add(new BackBoneConfiguration
            {
                Order = 5,
                Id = Guid.NewGuid(),
                Name = "Extrablancura Azul oscuro",

            });

            BackBones.Add(new BackBoneConfiguration
            {
                Order = 6,
                Id = Guid.NewGuid(),
                Name = "Menta",

            });

            BackBones.Add(new BackBoneConfiguration
            {
                Order = 7,
                Id = Guid.NewGuid(),
                Name = "Kolynos",

            });
        }
        void CreateProductBase()
        {
            Products = new List<ProductBaseConfiguration>
            {
                new ProductBaseConfiguration
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Kolynos",
                    Components = new List<ProductConfiguration>
                    {
                        new ProductConfiguration
                        {
                           BackBone = BackBones[7],
                            Percentage = 100.0,

                        }
                    }
                },
                new ProductBaseConfiguration
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Menta",
                    Components = new List<ProductConfiguration>
                    {
                        new ProductConfiguration
                        {
                           BackBone = BackBones[6],
                            Percentage = 100.0,

                        }
                    }
                },
                new ProductBaseConfiguration
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Triple Accion",
                    Components = new List<ProductConfiguration>
                    {
                        new ProductConfiguration
                        {
                            BackBone = BackBones[0],

                            Percentage = 33.33,

                        },
                        new ProductConfiguration
                        {
                             BackBone = BackBones[1],

                            Percentage = 33.33,
                        },
                        new ProductConfiguration
                        {
                              BackBone = BackBones[2],

                            Percentage = 33.33,
                        }
                    }
                },
                new ProductBaseConfiguration
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Extrablancura",
                    Components = new List<ProductConfiguration>
                    {
                        new ProductConfiguration
                        {
                            BackBone = BackBones[3],

                            Percentage = 33.33,
                        },
                        new ProductConfiguration
                        {
                            BackBone = BackBones[4],

                            Percentage = 33.33,
                        },
                        new ProductConfiguration
                        {
                            BackBone = BackBones[5],

                            Percentage = 33.33,
                        }
                    }
                },
                new ProductBaseConfiguration
                {
                    ProductId = Guid.NewGuid(),
                    ProductName = "Doble frescura",
                    Components = new List<ProductConfiguration>
                    {
                        new ProductConfiguration
                        {
                            BackBone = BackBones[0],

                            Percentage = 33.33,

                        },
                        new ProductConfiguration
                        {
                             BackBone = BackBones[1],

                            Percentage = 66.66,
                        }
                    }
                },
            };
        }
        void CreateMixers()
        {
            Mixers = new List<MixerConfiguration>
            {
                new MixerConfiguration
                {
                    MixerId = Guid.NewGuid(),
                    Name = "Brogly",

                    CleaningTime = new Time(8, TimeUnits.Hour),
                    Capabilities = new List<BackboneCapability>
                    {
                        new BackboneCapability
                        {
                            BackBone = BackBones[0],//triple accion blanco  
                              Capacity = new Mass(1350, MassUnits.KiloGram),
                            BatchTime = new Time(61, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {
                            BackBone = BackBones[1],//triple accion azul
                              Capacity = new Mass(1350, MassUnits.KiloGram),
                            BatchTime = new Time(61, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {
                            BackBone = BackBones[2],//triple accion verde
                              Capacity = new Mass(1350, MassUnits.KiloGram),
                            BatchTime = new Time(61, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {
                            BackBone = BackBones[3],//extrablancura blanco
                              Capacity = new Mass(1344, MassUnits.KiloGram),
                            BatchTime = new Time(61, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {  Capacity = new Mass(1344, MassUnits.KiloGram),
                            BackBone= BackBones[4],//extrablancura azul claro
                            BatchTime = new Time(61, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {
                            BackBone = BackBones[5],//extrablancura azul oscuro
                              Capacity = new Mass(1344, MassUnits.KiloGram),
                            BatchTime = new Time(61, TimeUnits.Minute)
                        }

                    }
                },

                new MixerConfiguration
                {
                    MixerId = Guid.NewGuid(),
                    Name = "Li Yuan 2",

                    CleaningTime = new Time(8, TimeUnits.Hour),
                    Capabilities = new List<BackboneCapability>
                    {
                        new BackboneCapability
                        {
                            BackBone = BackBones[6],// menta
                             Capacity = new Mass(2400, MassUnits.KiloGram),
                            BatchTime = new Time(85, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {
                            BackBone = BackBones[7],// kolynos
                             Capacity = new Mass(2400, MassUnits.KiloGram),
                            BatchTime = new Time(85, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {
                            BackBone = BackBones[0],// triple accion blanco
                             Capacity = new Mass(2100, MassUnits.KiloGram),
                            BatchTime = new Time(85, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {
                            BackBone = BackBones[3],// extrablancura blanco
                             Capacity = new Mass(2015, MassUnits.KiloGram),
                            BatchTime = new Time(85, TimeUnits.Minute)
                        }

                    }
                },

                new MixerConfiguration
                {
                    MixerId = Guid.NewGuid(),
                    Name = "Li Yuan 1",

                    CleaningTime = new Time(8, TimeUnits.Hour),
                    Capabilities = new List<BackboneCapability>
                    {
                        new BackboneCapability
                        {
                            Capacity = new Mass(2100, MassUnits.KiloGram),
                            BackBone = BackBones[0],// triple accion blanco
                            BatchTime = new Time(63, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {
                            BackBone = BackBones[3],// extrablancura blanco
                              Capacity = new Mass(2015, MassUnits.KiloGram),
                            BatchTime = new Time(63, TimeUnits.Minute)
                        }

                    }
                },

                new MixerConfiguration
                {
                    MixerId = Guid.NewGuid(),
                    Name = "Fryma",

                    CleaningTime = new Time(8, TimeUnits.Hour),
                    Capabilities = new List<BackboneCapability>
                    {
                        new BackboneCapability
                        {
                            BackBone = BackBones[1],// triple accion azul
                              Capacity = new Mass(2500, MassUnits.KiloGram),
                            BatchTime = new Time(73, TimeUnits.Minute)
                        },
                        new BackboneCapability
                        {
                            BackBone = BackBones[2],// triple accion verde
                              Capacity = new Mass(2500, MassUnits.KiloGram),
                            BatchTime = new Time(73, TimeUnits.Minute)
                        }

                    }
                },
            };
        }

        void CreateBigWIPTanks()
        {
            BigWIPTanks = new List<BigWIPTankConfiguration>
            {
                new BigWIPTankConfiguration
                {
                    TankId = Guid.NewGuid(),
                    InitialLevelKg = 2000, // Nivel inicial en kg
                    Capacity = new Mass(4000, MassUnits.KiloGram),
                    CleaningTime = new(180, TimeUnits.Minute), // 3 horas por defecto
                    InletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    OutletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),

                    BackBone = BackBones[0]
                },
                new BigWIPTankConfiguration
                {
                    TankId = Guid.NewGuid(),
                     InitialLevelKg = 2000, // Nivel inicial en kg
                    Capacity = new Mass(4000, MassUnits.KiloGram),
                     CleaningTime = new(180, TimeUnits.Minute), // 3 horas por defecto
                    InletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    OutletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    BackBone = BackBones[1]
                },
                new BigWIPTankConfiguration
                {
                    TankId = Guid.NewGuid(),
                     InitialLevelKg = 2000, // Nivel inicial en kg
                    Capacity = new Mass(4000, MassUnits.KiloGram),
                     CleaningTime = new(180, TimeUnits.Minute), // 3 horas por defecto
                    InletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    OutletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    BackBone = BackBones[2]
                },
                new BigWIPTankConfiguration
                {
                    TankId = Guid.NewGuid(),
                     InitialLevelKg = 1000, // Nivel inicial en kg
                    Capacity = new Mass(2000, MassUnits.KiloGram),
                     CleaningTime = new(180, TimeUnits.Minute), // 3 horas por defecto
                     InletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    OutletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    BackBone = BackBones[3]
                },
                new BigWIPTankConfiguration
                {
                    TankId = Guid.NewGuid(),
                     InitialLevelKg = 1000, // Nivel inicial en kg
                    Capacity = new Mass(2000, MassUnits.KiloGram),
                     CleaningTime = new(180, TimeUnits.Minute), // 3 horas por defecto
                     InletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    OutletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    BackBone = BackBones[4]
                },
                new BigWIPTankConfiguration
                {
                    TankId = Guid.NewGuid(),
                     InitialLevelKg = 1000, // Nivel inicial en kg
                    Capacity = new Mass(2000, MassUnits.KiloGram),
                     CleaningTime = new(180, TimeUnits.Minute), // 3 horas por defecto
                    InletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    OutletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    BackBone = BackBones[5]
                },
                new BigWIPTankConfiguration
                {
                    TankId = Guid.NewGuid(),
                     InitialLevelKg = 1000, // Nivel inicial en kg
                    Capacity = new Mass(2000, MassUnits.KiloGram),
                     CleaningTime = new(180, TimeUnits.Minute), // 3 horas por defecto
                     InletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    OutletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    BackBone = BackBones[6]
                },
                new BigWIPTankConfiguration
                {
                    TankId = Guid.NewGuid(),
                     InitialLevelKg = 1000, // Nivel inicial en kg
                    Capacity = new Mass(2000, MassUnits.KiloGram),
                     CleaningTime = new(180, TimeUnits.Minute), // 3 horas por defecto
                     InletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    OutletMassFlow= new MassFlow(100, MassFlowUnits.Kg_min),
                    BackBone = BackBones[7]
                },
            };
        }
        void CreateProductionLines()
        {
            ProductionLines = new List<ProductionLineConfiguration>
            {
                new ProductionLineConfiguration
                {
                    LineId = Guid.NewGuid(),
                    Name = "Linea 1",

                    WIPTanks = new List<WIPtankForLineConfiguration>
                    {
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #1 Linea 1",
                            MinimumLevelPercentage= 20 ,// 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[0].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto

                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #2 Linea 1",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #3 Linea 1",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[1].BackBone.Id,
                             CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #4 Linea 1",
                            MinimumLevelPercentage= 20 , // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[2].Components[2].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                    },
                    MassPlannedPerLineProducts=new List<MassPlannedPerLineProductBaseConfiguration>
                     {
                         new MassPlannedPerLineProductBaseConfiguration
                         {
                            TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                            Order = 1,
                            MassPlanned = new Mass(10000, MassUnits.KiloGram),
                            Product = Products[2], //
                            LineVelocity = new LineVelocity(180, LineVelocityUnits.EA_min),
                            FormatMass = new Mass(91.4, MassUnits.Gram)
                         },
                          new MassPlannedPerLineProductBaseConfiguration
                         {
                            TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                            Order = 2,
                            MassPlanned = new Mass(20000, MassUnits.KiloGram),
                            Product = Products[0], //
                            LineVelocity = new LineVelocity(180, LineVelocityUnits.EA_min),
                            FormatMass = new Mass(92, MassUnits.Gram)
                         },
                         //  new MassPlannedPerLineProductBaseConfiguration
                         //{
                         //   TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                         //   Order = 3,
                         //   MassPlanned = new Mass(15000, MassUnits.KiloGram),
                         //   Product = Products[1], //
                         //   LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
                         //   FormatMass = new Mass(92, MassUnits.Gram)
                         //},
                         //   new MassPlannedPerLineProductBaseConfiguration
                         //{
                         //   TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                         //   Order = 4,
                         //   MassPlanned = new Mass(25000, MassUnits.KiloGram),
                         //   Product = Products[3], //
                         //   LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
                         //   FormatMass = new Mass(92, MassUnits.Gram)
                         //},


                     },

                }, new ProductionLineConfiguration
                {
                    LineId = Guid.NewGuid(),
                    Name = "Linea 2",
                    WIPTanks = new List<WIPtankForLineConfiguration>
                    {
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #1 Linea 2",
                            MinimumLevelPercentage= 20 ,// 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[0].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto

                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #2 Linea 2",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #3 Linea 2",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[1].BackBone.Id,
                             CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #4 Linea 2",
                            MinimumLevelPercentage= 20 , // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[2].Components[2].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                    },
                    MassPlannedPerLineProducts=new List<MassPlannedPerLineProductBaseConfiguration>
                     {
                         new MassPlannedPerLineProductBaseConfiguration
                         {
                            TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                            Order = 1,
                            MassPlanned = new Mass(50000, MassUnits.KiloGram),
                            Product = Products[2], //
                            LineVelocity = new LineVelocity(140, LineVelocityUnits.EA_min),
                            FormatMass = new Mass(91.4, MassUnits.Gram)
                         },
               //           new MassPlannedPerLineProductBaseConfiguration
               //          {
               //             TimeToChangeFormat=new Time(1, TimeUnits.Hour),
               //             Order = 2,
               //             MassPlanned = new Mass(20000, MassUnits.KiloGram),
               //             Product = Products[0], //
               //             LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
               //             FormatMass = new Mass(92, MassUnits.Gram)
               //          },
               //            new MassPlannedPerLineProductBaseConfiguration
               //          {
               //             TimeToChangeFormat=new Time(1, TimeUnits.Hour),
               //             Order = 3,
               //             MassPlanned = new Mass(15000, MassUnits.KiloGram),
               //             Product = Products[1], //
               //             LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
               //             FormatMass = new Mass(92, MassUnits.Gram)
               //          },
               //             new MassPlannedPerLineProductBaseConfiguration
               //          {
               //             TimeToChangeFormat=new Time(1, TimeUnits.Hour),
               //             Order = 4,
               //             MassPlanned = new Mass(25000, MassUnits.KiloGram),
               //             Product = Products[3], //
               //             LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
               //             FormatMass = new Mass(92, MassUnits.Gram)
               //          },


                     },

                },
                new ProductionLineConfiguration
                {
                    LineId = Guid.NewGuid(),
                    Name = "Linea 4",

                    WIPTanks = new List<WIPtankForLineConfiguration>
                    {
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #1 Linea 3",
                            MinimumLevelPercentage= 20 ,// 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[0].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto

                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #2 Linea 3",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #3 Linea 3",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[1].BackBone.Id,
                             CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #4 Linea 3",
                            MinimumLevelPercentage= 20 , // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[2].Components[2].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                    },
                    MassPlannedPerLineProducts=new List<MassPlannedPerLineProductBaseConfiguration>
                     {
                         new MassPlannedPerLineProductBaseConfiguration
                         {
                            TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                            Order = 1,
                            MassPlanned = new Mass(50000, MassUnits.KiloGram),
                            Product = Products[2], //
                            LineVelocity = new LineVelocity(150, LineVelocityUnits.EA_min),
                            FormatMass = new Mass(91.4, MassUnits.Gram)
                         },
                         // new MassPlannedPerLineProductBaseConfiguration
                         //{
                         //   TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                         //   Order = 2,
                         //   MassPlanned = new Mass(20000, MassUnits.KiloGram),
                         //   Product = Products[0], //
                         //   LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
                         //   FormatMass = new Mass(92, MassUnits.Gram)
                         //},
                         //  new MassPlannedPerLineProductBaseConfiguration
                         //{
                         //   TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                         //   Order = 3,
                         //   MassPlanned = new Mass(15000, MassUnits.KiloGram),
                         //   Product = Products[1], //
                         //   LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
                         //   FormatMass = new Mass(92, MassUnits.Gram)
                         //},
                         //   new MassPlannedPerLineProductBaseConfiguration
                         //{
                         //   TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                         //   Order = 4,
                         //   MassPlanned = new Mass(25000, MassUnits.KiloGram),
                         //   Product = Products[3], //
                         //   LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
                         //   FormatMass = new Mass(92, MassUnits.Gram)
                         //},


                     },

                }, new ProductionLineConfiguration
                {
                    LineId = Guid.NewGuid(),
                    Name = "Linea 6",
                    WIPTanks = new List<WIPtankForLineConfiguration>
                    {
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #1 Linea 4",
                            MinimumLevelPercentage= 20 ,// 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[0].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto

                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #2 Linea 4",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #3 Linea 4",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[1].BackBone.Id,
                             CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #4 Linea 4",
                            MinimumLevelPercentage= 20 , // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[2].Components[2].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                    },
                    MassPlannedPerLineProducts=new List<MassPlannedPerLineProductBaseConfiguration>
                     {
                         new MassPlannedPerLineProductBaseConfiguration
                         {
                            TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                            Order = 1,
                            MassPlanned = new Mass(50000, MassUnits.KiloGram),
                            Product = Products[2], //
                           LineVelocity = new LineVelocity(146, LineVelocityUnits.EA_min),
                            FormatMass = new Mass(91.4, MassUnits.Gram)
                         },
               //           new MassPlannedPerLineProductBaseConfiguration
               //          {
               //             TimeToChangeFormat=new Time(1, TimeUnits.Hour),
               //             Order = 2,
               //             MassPlanned = new Mass(20000, MassUnits.KiloGram),
               //             Product = Products[0], //
               //             LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
               //             FormatMass = new Mass(92, MassUnits.Gram)
               //          },
               //            new MassPlannedPerLineProductBaseConfiguration
               //          {
               //             TimeToChangeFormat=new Time(1, TimeUnits.Hour),
               //             Order = 3,
               //             MassPlanned = new Mass(15000, MassUnits.KiloGram),
               //             Product = Products[1], //
               //             LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
               //             FormatMass = new Mass(92, MassUnits.Gram)
               //          },
               //             new MassPlannedPerLineProductBaseConfiguration
               //          {
               //             TimeToChangeFormat=new Time(1, TimeUnits.Hour),
               //             Order = 4,
               //             MassPlanned = new Mass(25000, MassUnits.KiloGram),
               //             Product = Products[3], //
               //             LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
               //             FormatMass = new Mass(92, MassUnits.Gram)
               //          },


                     },

                },
                new ProductionLineConfiguration
                {
                    LineId = Guid.NewGuid(),
                    Name = "Linea 9",
                    WIPTanks = new List<WIPtankForLineConfiguration>
                    {
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #1 Linea 5",
                            MinimumLevelPercentage= 20 ,// 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[0].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto

                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #2 Linea 5",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[0].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #3 Linea 5",
                            MinimumLevelPercentage= 20, // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                             Id= Products[2].Components[1].BackBone.Id,
                             CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                        new WIPtankForLineConfiguration
                        {
                            TankId = Guid.NewGuid(),
                            Name = "WIP Tank #4 Linea 5",
                            MinimumLevelPercentage= 20 , // 20% por defecto
                            InitialLevelKg = 1000, // Nivel inicial en kg
                            Id= Products[2].Components[2].BackBone.Id,
                            CleaningTime  = new(180, TimeUnits.Minute)// 3 horas por defecto
                        },
                    },
                    MassPlannedPerLineProducts=new List<MassPlannedPerLineProductBaseConfiguration>
                     {
                         new MassPlannedPerLineProductBaseConfiguration
                         {
                            TimeToChangeFormat=new Time(1, TimeUnits.Hour),
                            Order = 1,
                            MassPlanned = new Mass(50000, MassUnits.KiloGram),
                            Product = Products[2], //
                           LineVelocity = new LineVelocity(270, LineVelocityUnits.EA_min),
                            FormatMass = new Mass(91.4, MassUnits.Gram)
                         },
               //           new MassPlannedPerLineProductBaseConfiguration
               //          {
               //             TimeToChangeFormat=new Time(1, TimeUnits.Hour),
               //             Order = 2,
               //             MassPlanned = new Mass(20000, MassUnits.KiloGram),
               //             Product = Products[0], //
               //             LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
               //             FormatMass = new Mass(92, MassUnits.Gram)
               //          },
               //            new MassPlannedPerLineProductBaseConfiguration
               //          {
               //             TimeToChangeFormat=new Time(1, TimeUnits.Hour),
               //             Order = 3,
               //             MassPlanned = new Mass(15000, MassUnits.KiloGram),
               //             Product = Products[1], //
               //             LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
               //             FormatMass = new Mass(92, MassUnits.Gram)
               //          },
               //             new MassPlannedPerLineProductBaseConfiguration
               //          {
               //             TimeToChangeFormat=new Time(1, TimeUnits.Hour),
               //             Order = 4,
               //             MassPlanned = new Mass(25000, MassUnits.KiloGram),
               //             Product = Products[3], //
               //             LineVelocity = new LineVelocity(100, LineVelocityUnits.EA_min),
               //             FormatMass = new Mass(92, MassUnits.Gram)
               //          },


                     },

                }
            };
        }

    }
}
