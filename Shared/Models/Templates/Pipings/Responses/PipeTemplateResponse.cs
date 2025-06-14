using Shared.Enums.DiameterEnum;
using Shared.Enums.Materials;
using Shared.Models.Brands.Responses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.Templates.NozzleTemplates;
using System.Reflection;

namespace Shared.Models.Templates.Pipings.Responses
{
    public class PipeTemplateResponse : BaseResponse, IMessageResponse, IRequest
    {

     
        public double EquivalentLenghPrice { get; set; }
        public MaterialEnum Material { get; set; } = MaterialEnum.None;
        public double LaborDayPrice { get; set; }
        public NominalDiameterEnum Diameter { get; set; } = NominalDiameterEnum.None;
        public PipeClassEnum Class { get; set; } = PipeClassEnum.None;
        public bool Insulation { get; set; }

       
        public string Legend => $"{Diameter.Name} {Material.Name} {Class.Name}";
        public string EndPointName => StaticClass.PipeTemplates.EndPoint.CreateUpdate;
        public string ActionType => Id == Guid.Empty ? "created" : "updated";
        public string ClassName => StaticClass.PipeTemplates.ClassName;
        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);
        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);
        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);
    }
}
