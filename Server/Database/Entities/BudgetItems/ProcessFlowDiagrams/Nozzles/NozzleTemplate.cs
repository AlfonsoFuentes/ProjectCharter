﻿namespace Server.Database.Entities.BudgetItems.ProcessFlowDiagrams.Nozzles
{
    public class NozzleTemplate : AuditableEntity<Guid>, ITenantCommon
    {
        public int ConnectionType { get; set; }
        public int NominalDiameter { get; set; }
        public int NozzleType { get; set; }
        public Template Template { get; set; } = null!;
        public Guid? TemplateId { get; set; }
        public static NozzleTemplate Create(Guid templateId)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                TemplateId = templateId
            };
        }


    }

}
