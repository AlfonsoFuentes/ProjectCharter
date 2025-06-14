namespace Shared.Models.Projects.Reponses
{
    public class ProjectTreeViewResponse 
    {
        public string Name { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public List<CaseTreeView> Cases { get; set; } = new();
        public class CaseTreeView
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;

            public List<ScopeTreeView> Scopes { get; set; } = new();
            public class ScopeTreeView
            {
                public Guid Id { get; set; }
                public string Name { get; set; } = string.Empty;
                public List<DeliverableTreeView> Deliverables { get; set; } = new();
                public class DeliverableTreeView
                {
                    public Guid Id { get; set; }
                    public string Name { get; set; } = string.Empty;
                }
            }
        }

    }
}
