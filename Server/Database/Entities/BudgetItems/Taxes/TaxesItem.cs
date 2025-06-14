﻿using Server.Database.Contracts;

namespace Server.Database.Entities.BudgetItems.Taxes
{
    public class TaxesItem : AuditableEntity<Guid>, ITenantEntity
    {

        public Guid TaxItemId { get; set; }
        public Tax TaxItem { get; set; } = null!;

        public Guid? SelectedId { get; set; }
        public BudgetItem? Selected { get; set; } = null!;
        public string TenantId { get; set; } = string.Empty;

        public static TaxesItem Create(Guid TaxId, Guid BudgetItemId)
        {
            return new()
            {
                TaxItemId = TaxId,
                SelectedId = BudgetItemId,
            };
        }
    }

}
