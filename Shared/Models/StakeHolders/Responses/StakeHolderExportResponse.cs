using Shared.Enums.StakeHolderTypes;

namespace Shared.Models.StakeHolders.Responses
{
    public class StakeHolderExportResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public StakeHolderRoleEnum Role { get; set; } = StakeHolderRoleEnum.None;
    }
}
