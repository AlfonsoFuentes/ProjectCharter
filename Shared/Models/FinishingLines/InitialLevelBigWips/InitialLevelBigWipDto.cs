using Shared.Models.FileResults.Generics.Records;
using Shared.Models.FileResults.Generics.Reponses;
using Shared.Models.FileResults.Generics.Request;
using Shared.Models.FinishingLines.BackBones;
using Shared.Models.FinishingLines.BIGWIPTanks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Models.FinishingLines.InitialLevelBigWips
{
    public class InitialLevelBigWipResponse : BaseResponse, IMessageResponse, IRequest
    {
        public string EndPointName => StaticClass.InitialLevelBigWips.EndPoint.CreateUpdate;

        public string Legend => Name;

        public string ActionType => Id == Guid.Empty ? "created" : "updated";

        public string ClassName => StaticClass.InitialLevelBigWips.ClassName;

        public string Succesfully => StaticClass.ResponseMessages.ReponseSuccesfullyMessage(Legend, ClassName, ActionType);

        public string Fail => StaticClass.ResponseMessages.ReponseFailMessage(Legend, ClassName, ActionType);

        public string NotFound => StaticClass.ResponseMessages.ReponseNotFound(ClassName);

        public Guid ProductionPlanId { get; set; }
        [JsonIgnore]
        public Mass InitialLevel=>new Mass(InitialLevelValue, MassUnits.KiloGram);
        public double InitialLevelValue { get; set; }
        public string InitialLevelUnit { get; set; } = MassUnits.KiloGram.Name;
        public BIGWIPTankResponse BigWipTank { get; set; } = new BIGWIPTankResponse();
        public string BigWipTankname => BigWipTank == null ? string.Empty : BigWipTank.Name;
    }


    public class DeleteInitialLevelBigWipRequest : DeleteMessageResponse, IRequest
    {
        public string Name { get; set; } = string.Empty;

        public override string Legend => Name;

        public override string ClassName => StaticClass.InitialLevelBigWips.ClassName;

        public Guid Id { get; set; }

        public string EndPointName => StaticClass.InitialLevelBigWips.EndPoint.Delete;
    }

    public class GetInitialLevelBigWipByIdRequest : GetByIdMessageResponse, IGetById
    {
        public Guid Id { get; set; }

        public string EndPointName => StaticClass.InitialLevelBigWips.EndPoint.GetById;

        public override string ClassName => StaticClass.InitialLevelBigWips.ClassName;
    }

    public class InitialLevelBigWipGetAll : IGetAll
    {
        public string EndPointName => StaticClass.InitialLevelBigWips.EndPoint.GetAll;
    }

    public class InitialLevelBigWipResponseList : IResponseAll
    {
        public List<InitialLevelBigWipResponse> Items { get; set; } = new();
    }

    public class ValidateInitialLevelBigWipNameRequest : ValidateMessageResponse, IRequest
    {
        public Guid? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EndPointName => StaticClass.InitialLevelBigWips.EndPoint.Validate;

        public override string Legend => Name;

        public override string ClassName => StaticClass.InitialLevelBigWips.ClassName;
    }
    public class DeleteGroupInitialLevelBigWipRequest : DeleteMessageResponse, IRequest
    {

        public override string Legend => "Group of InitialLevelBigWip";

        public override string ClassName => StaticClass.InitialLevelBigWips.ClassName;

        public HashSet<InitialLevelBigWipResponse> SelecteItems { get; set; } = null!;

        public string EndPointName => StaticClass.InitialLevelBigWips.EndPoint.DeleteGroup;
    }
}
