using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.BackBones;
using Shared.Models.FinishingLines.BIGWIPTanks;
using Shared.Models.FinishingLines.WIPTankLines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Models.FinishingLines.InitialLevelWips
{
    public class InitialLevelWipResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.InitialLevelWips.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.InitialLevelWips.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public Guid ProductionLineAssignmentId { get; set; }
        [JsonIgnore]
        public Mass InitialLevel=>new Mass(InitialLevelValue, MassUnits.KiloGram);
        public double InitialLevelValue { get; set; }
        public string InitialLevelUnit { get; set; } = MassUnits.KiloGram.Name;
        public WIPTankLineResponse WipTankLine { get; set; } = new WIPTankLineResponse();
        public string BigWipTankname => WipTankLine == null ? string.Empty : WipTankLine.Name;
    }


    public class DeleteInitialLevelWipRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.InitialLevelWips.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.InitialLevelWips.EndPoint.Delete;
    }

    public class GetInitialLevelWipByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.InitialLevelWips.EndPoint.GetById;

        public override string ClassName => StaticClass.InitialLevelWips.ClassName;
    }

    public class InitialLevelWipGetAll : IGetAll
    {
        public string EndPointName => StaticClass.InitialLevelWips.EndPoint.GetAll;
    }

    public class InitialLevelWipResponseList : IResponseAll
    {
        public List<InitialLevelWipResponse> Items { get; set; } = new();
    }

    public class ValidateInitialLevelWipNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.InitialLevelWips.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.InitialLevelWips.ClassName;
    }
    public class DeleteGroupInitialLevelWipRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of InitialLevelWip";

        public override string ClassName => StaticClass.InitialLevelWips.ClassName;

        public HashSet<InitialLevelWipResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.InitialLevelWips.EndPoint.DeleteGroup;
    }
}
