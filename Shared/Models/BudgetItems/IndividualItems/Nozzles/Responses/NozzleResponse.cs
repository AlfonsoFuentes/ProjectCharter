using Shared.Enums.ConnectionTypes;
using Shared.Enums.DiameterEnum;
using Shared.Enums.NozzleTypes;

namespace Shared.Models.BudgetItems.IndividualItems.Nozzles.Responses
{
    public class NozzleResponse : BaseResponse
    {
        public string UpadtePageName { get; set; } = StaticClass.Nozzles.PageName.Update;

        public ConnectionTypeEnum ConnectionType { get; set; } = ConnectionTypeEnum.None;
        public NominalDiameterEnum NominalDiameter { get; set; } = NominalDiameterEnum.None;
        public NozzleTypeEnum NozzleType { get; set; } = NozzleTypeEnum.None;


        public Length OuterDiameter = new Length(2, LengthUnits.Inch);
        public Length Thickness = new Length(0, LengthUnits.Inch);
        public Length InnerDiameter = new Length(0, LengthUnits.Inch);
        public Length Height = new Length(0, LengthUnits.MilliMeter);



    }
}
