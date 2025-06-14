using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.NozzleTypes;

namespace Shared.Models.Templates.NozzleTemplates
{
    public class NozzleTemplateResponse:BaseResponse
    {
        public ConnectionTypeEnum ConnectionType { get; set; } = ConnectionTypeEnum.None;
        public NominalDiameterEnum NominalDiameter { get; set; } = NominalDiameterEnum.None;
        public NozzleTypeEnum NozzleType { get; set; } = NozzleTypeEnum.None;
    }
}
