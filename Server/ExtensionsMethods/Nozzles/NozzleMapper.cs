using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.NozzleTypes;
using Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses;
using Shared.Models.Templates.NozzleTemplates;
using Shared.Models.Templates.Valves.Responses;

namespace Server.ExtensionsMethods.Nozzles
{
    public static class NozzleMapper
    {
        public static NozzleTemplateResponse Map(this NozzleTemplate row)
        {
            return new()
            {
                Id = row.Id,
                NominalDiameter = NominalDiameterEnum.GetType(row.NominalDiameter),
                ConnectionType = ConnectionTypeEnum.GetType(row.ConnectionType),
                NozzleType = NozzleTypeEnum.GetType(row.NozzleType),

            };
        }
        public static NozzleTemplate Map(this NozzleTemplateResponse response, NozzleTemplate row)
        {
            row.NozzleType = response.NozzleType.Id;
            row.NominalDiameter = response.NominalDiameter.Id;
            row.ConnectionType = response.ConnectionType.Id;
            return row;

        }
        public static NozzleTemplate Map(this NozzleResponse response, NozzleTemplate row)
        {
            row.NozzleType = response.NozzleType.Id;
            row.NominalDiameter = response.NominalDiameter.Id;
            row.ConnectionType = response.ConnectionType.Id;
            return row;

        }
        public static Nozzle Map(this NozzleResponse request, Nozzle row)
        {
            row.Order = request.Order;
            row.ConnectionType = request.ConnectionType.Id;
            row.NominalDiameter = request.NominalDiameter.Id;
            row.InnerDiameter = request.InnerDiameter.Value;
            row.OuterDiameter = request.OuterDiameter.Value;
            row.Thickness = request.Thickness.Value;
            row.Height = request.Height.Value;
            row.OuterDiameterUnit = request.OuterDiameter.Unit.Name;
            row.ThicknessUnit = request.Thickness.Unit.Name;
            row.InnerDiameterUnit = request.InnerDiameter.Unit.Name;
            row.HeightUnit = request.Height.Unit.Name;
            row.NozzleType = request.NozzleType.Id;
            return row;
        }
        public static NozzleResponse Map(this Nozzle row)
        {
            return new()
            {
                Id = row.Id,
                Name = row.Name,
                ConnectionType = ConnectionTypeEnum.GetType(row.ConnectionType),
                NozzleType = NozzleTypeEnum.GetType(row.NozzleType),
                NominalDiameter = NominalDiameterEnum.GetType(row.NominalDiameter),
                OuterDiameter = string.IsNullOrEmpty(row.OuterDiameterUnit) ? new() : new Length(row.OuterDiameter, row.OuterDiameterUnit),
                InnerDiameter = string.IsNullOrEmpty(row.InnerDiameterUnit) ? new() : new Length(row.InnerDiameter, row.InnerDiameterUnit),
                Thickness = string.IsNullOrEmpty(row.ThicknessUnit) ? new() : new Length(row.Thickness, row.ThicknessUnit),
                Height = string.IsNullOrEmpty(row.HeightUnit) ? new() : new Length(row.Height, row.HeightUnit),
                Order = row.Order,


            };
        }
        public static bool ValidateNozzles(this IEnumerable<NozzleTemplate> existingNozzles, IEnumerable<NozzleTemplateResponse> incomingNozzles)
        {
            // Crear un diccionario para acceder rápidamente a las boquillas existentes
            var existingNozzleLookup = existingNozzles.ToLookup(nt =>
                (nt.NominalDiameter, nt.NozzleType, nt.ConnectionType));

            // Verificar que todas las boquillas entrantes existan en las boquillas existentes
            foreach (var nozzle in incomingNozzles)
            {
                if (!existingNozzleLookup.Contains((nozzle.NominalDiameter.Id, nozzle.NozzleType.Id, nozzle.ConnectionType.Id)))
                {
                    return false;
                }
            }

            return true;
        }
        public static bool ValidateNozzles(this IEnumerable<NozzleTemplate> existingNozzles, IEnumerable<NozzleResponse> incomingNozzles)
        {
            // Crear un diccionario para acceder rápidamente a las boquillas existentes
            var existingNozzleLookup = existingNozzles.ToLookup(nt =>
                (nt.NominalDiameter, nt.NozzleType, nt.ConnectionType));

            // Verificar que todas las boquillas entrantes existan en las boquillas existentes
            foreach (var nozzle in incomingNozzles)
            {
                if (!existingNozzleLookup.Contains((nozzle.NominalDiameter.Id, nozzle.NozzleType.Id, nozzle.ConnectionType.Id)))
                {
                    return false;
                }
            }

            return true;
        }
        public static async Task CreateNozzles(IRepository Repository, Guid ParentId, List<NozzleResponse> Nozzles)
        {
            int order = 0;
            foreach (var nozzleTempalte in Nozzles)
            {
                order++;
                var nozzle = Nozzle.Create(ParentId);
                nozzleTempalte.Map(nozzle);

                nozzle.Order = order;
                await Repository.AddAsync(nozzle);
            }
        }
        public static async Task CreateNozzleTemplates(IRepository Repository, Guid ParentId, List<NozzleTemplateResponse> Nozzles)
        {
            int order = 0;
            foreach (var nozzleTempalte in Nozzles)
            {
                order++;
                var nozzle = NozzleTemplate.Create(ParentId);
                nozzleTempalte.Map(nozzle);

                nozzle.Order = order;
                await Repository.AddAsync(nozzle);
            }
        }
        public static async Task CreateNozzleTemplates(IRepository Repository, Guid ParentId, List<NozzleResponse> Nozzles)
        {
            int order = 0;
            foreach (var nozzleTempalte in Nozzles)
            {
                order++;
                var nozzle = NozzleTemplate.Create(ParentId);
                nozzleTempalte.Map(nozzle);

                nozzle.Order = order;
                await Repository.AddAsync(nozzle);
            }
        }
        public static async Task UpdateNozzles(IRepository Repository, List<Nozzle> nozzles, List<NozzleResponse> nozzleRespomses, Guid ParentId)
        {

            foreach (var item in nozzles)
            {
                if (!nozzleRespomses.Any(x => x.Id == item.Id))
                {
                    await Repository.RemoveAsync(item);
                }
            }

            foreach (var item in nozzleRespomses)
            {
                var nozzle = nozzles.SingleOrDefault(x => x.Id == item.Id);
                if (nozzle != null)
                {
                    await Repository.UpdateAsync(nozzle);
                }
                else
                {
                    nozzle = Nozzle.Create(ParentId);
                    await Repository.AddAsync(nozzle);

                }
                item.Map(nozzle);
            }

        }
        public static async Task UpdateNozzlesTemplate(IRepository Repository, List<NozzleTemplate> nozzles, List<NozzleTemplateResponse> nozzleRespomses, Guid ParentId)
        {

            foreach (var item in nozzles)
            {
                if (!nozzleRespomses.Any(x => x.Id == item.Id))
                {
                    await Repository.RemoveAsync(item);
                }
            }

            foreach (var item in nozzleRespomses)
            {
                var nozzle = nozzles.SingleOrDefault(x => x.Id == item.Id);
                if (nozzle != null)
                {
                    await Repository.UpdateAsync(nozzle);
                }
                else
                {
                    nozzle = NozzleTemplate.Create(ParentId);
                    await Repository.AddAsync(nozzle);

                }
                item.Map(nozzle);
            }

        }
        

        
    }
}
