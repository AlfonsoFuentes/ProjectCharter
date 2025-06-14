namespace Shared.Enums.ProjectNeedTypes
{
    public class ProjectStatusEnum : ValueObject
    {
        public override string ToString()
        {
            return Name;
        }
        public static ProjectStatusEnum None = Create(-1, "None");
        public static ProjectStatusEnum Created = Create(0, "Created");
        public static ProjectStatusEnum Approved = Create(1, "Approved");
        public static ProjectStatusEnum Execution = Create(2, "On Execution");
        public static ProjectStatusEnum Closed = Create(3, "Closed");



        public static List<ProjectStatusEnum> List = new List<ProjectStatusEnum>()
            {
            None, Created, Approved, Execution, Closed
            };
        public static string GetName(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)!.Name : string.Empty;
        public static ProjectStatusEnum GetType(int id) => List.Exists(x => x.Id == id) ? List.FirstOrDefault(x => x.Id == id)! : None;
        public static ProjectStatusEnum Create(int id, string name) => new ProjectStatusEnum() { Id = id, Name = name };
        public static ProjectStatusEnum GetType(string name) => List.Exists(x => x.Name == name) ? List.FirstOrDefault(x => x.Name == name)!
            : None;
    }
}
