using Shared.Models.PurchaseOrders.Requests;
using Shared.Models.PurchaseOrders.Responses;

namespace Shared.Models.PurchaseOrders.Mappers
{
    public static class PurchaseOrderMappers
    {
        public static EditPurchaseApprovedOrderRequest ToEditApproved(this PurchaseOrderResponse response)
        {
            return new()
            {

                CostCenter = response.CostCenter,
                CurrencyDate = response.CurrencyDate,
                IsAlteration = response.IsAlteration,
                IsCapitalizedSalary = response.IsCapitalizedSalary,
                IsProductiveAsset = response.IsProductiveAsset,
                MainBudgetItemId = response.MainBudgetItemId,
                Name = response.Name,
                PurchaseRequisition = response.PurchaseRequisition,
                ProjectAccount = response.ProjectAccount,
                ProjectId = response.ProjectId,
                PurchaseOrderCurrency = response.PurchaseOrderCurrency,
                Id = response.Id,
                PurchaseOrderItems = response.PurchaseOrderItems,
                QuoteCurrency = response.QuoteCurrency,
                QuoteNo = response.QuoteNo,
                Supplier = response.Supplier,
                TaxCodeLD = response.Supplier!.TaxCodeLD,
                TaxCodeLP = response.Supplier!.TaxCodeLP,
                USDCOP = response.USDCOP,
                USDEUR = response.USDEUR,
                PONumber = response.PONumber,
                ExpectedDate = response.ExpectedDate,
                ApprovedDate = response.ApprovedDate,
                ClosedDate = response.ClosedDate,
                ReceivingDate = response.ReceivingDate,
                PurchaseOrderStatus = response.PurchaseOrderStatus,
                ReceivingUSDCOP = response.ReceivingUSDCOP,
                ReceivingUSDEUR = response.ReceivingUSDEUR,
                IsTaxEditable = response.IsTaxEditable,


            };
        }
        public static ApprovePurchaseOrderRequest ToApprove(this PurchaseOrderResponse response)
        {
            return new()
            {

                CostCenter = response.CostCenter,
                CurrencyDate = response.CurrencyDate,
                IsAlteration = response.IsAlteration,
                IsCapitalizedSalary = response.IsCapitalizedSalary,
                IsProductiveAsset = response.IsProductiveAsset,
                MainBudgetItemId = response.MainBudgetItemId,
                Name = response.Name,
                PurchaseRequisition = response.PurchaseRequisition,
                ProjectAccount = response.ProjectAccount,
                ProjectId = response.ProjectId,
                PurchaseOrderCurrency = response.PurchaseOrderCurrency,
                Id = response.Id,
                PurchaseOrderItems = response.PurchaseOrderItems,
                QuoteCurrency = response.QuoteCurrency,
                QuoteNo = response.QuoteNo,
                Supplier = response.Supplier,
                TaxCodeLD = response.Supplier!.TaxCodeLD,
                TaxCodeLP = response.Supplier!.TaxCodeLP,
                USDCOP = response.USDCOP,
                USDEUR = response.USDEUR,
                ApprovedDate = response.CurrencyDate,
                PONumber = response.PONumber,
                ExpectedDate = response.ExpectedDate,
                IsTaxEditable = response.IsTaxEditable,
                ReceivingDate = response.ReceivingDate,
                PurchaseOrderStatus = response.PurchaseOrderStatus,
                ReceivingUSDCOP = response.ReceivingUSDCOP,
                ReceivingUSDEUR = response.ReceivingUSDEUR,
                ClosedDate = response.ClosedDate,



            };
        }
        public static EditPurchaseOrderCreatedRequest ToEditCreated(this PurchaseOrderResponse response)
        {
            return new()
            {
                CostCenter = response.CostCenter,
                CurrencyDate = response.CurrencyDate,
                IsAlteration = response.IsAlteration,
                IsCapitalizedSalary = response.IsCapitalizedSalary,
                IsProductiveAsset = response.IsProductiveAsset,
                MainBudgetItemId = response.MainBudgetItemId,
                Name = response.Name,
                PurchaseRequisition = response.PurchaseRequisition,
                ProjectAccount = response.ProjectAccount,
                ProjectId = response.ProjectId,
                PurchaseOrderCurrency = response.PurchaseOrderCurrency,
                Id = response.Id,
                PurchaseOrderItems = response.PurchaseOrderItems,
                QuoteCurrency = response.QuoteCurrency,
                QuoteNo = response.QuoteNo,
                Supplier = response.Supplier,
                TaxCodeLD = response.Supplier!.TaxCodeLD,
                TaxCodeLP = response.Supplier!.TaxCodeLP,
                USDCOP = response.USDCOP,
                USDEUR = response.USDEUR,
                PONumber = response.PONumber,
                ExpectedDate = response.ExpectedDate,
                IsTaxEditable = response.IsTaxEditable,
                ApprovedDate = response.ApprovedDate,
                ClosedDate = response.ClosedDate,
                ReceivingDate = response.ReceivingDate,
                PurchaseOrderStatus = response.PurchaseOrderStatus,
                ReceivingUSDCOP = response.ReceivingUSDCOP,
                ReceivingUSDEUR = response.ReceivingUSDEUR,



            };
        }
        public static ReceivePurchaseOrderApprovedRequest ToReceive(this PurchaseOrderResponse response)
        {
            return new()
            {

                CostCenter = response.CostCenter,
                CurrencyDate = response.CurrencyDate,
                IsAlteration = response.IsAlteration,
                IsCapitalizedSalary = response.IsCapitalizedSalary,
                IsProductiveAsset = response.IsProductiveAsset,
                MainBudgetItemId = response.MainBudgetItemId,
                Name = response.Name,
                PurchaseRequisition = response.PurchaseRequisition,
                ProjectAccount = response.ProjectAccount,
                ProjectId = response.ProjectId,
                PurchaseOrderCurrency = response.PurchaseOrderCurrency,
                Id = response.Id,
                PurchaseOrderItems = response.PurchaseOrderItems,
                QuoteCurrency = response.QuoteCurrency,
                QuoteNo = response.QuoteNo,
                Supplier = response.Supplier,
                TaxCodeLD = response.Supplier!.TaxCodeLD,
                TaxCodeLP = response.Supplier!.TaxCodeLP,
                USDCOP = response.USDCOP,
                USDEUR = response.USDEUR,
                PONumber = response.PONumber,
                ExpectedDate = response.ExpectedDate,
                ReceivingDate = response.ReceivingDate,
                ReceivingUSDCOP = response.ReceivingUSDCOP,
                ReceivingUSDEUR = response.ReceivingUSDEUR,
                ApprovedDate = response.ApprovedDate,
                ClosedDate = response.ClosedDate,
                PurchaseOrderStatus = response.PurchaseOrderStatus,
                IsTaxEditable = response.IsTaxEditable,


            };
        }
        public static EditPurchaseOrderClosedRequest ToEditClosed(this PurchaseOrderResponse response)
        {
            return new()
            {

                CostCenter = response.CostCenter,
                CurrencyDate = response.CurrencyDate,
                IsAlteration = response.IsAlteration,
                IsCapitalizedSalary = response.IsCapitalizedSalary,
                IsProductiveAsset = response.IsProductiveAsset,
                MainBudgetItemId = response.MainBudgetItemId,
                Name = response.Name,
                PurchaseRequisition = response.PurchaseRequisition,
                ProjectAccount = response.ProjectAccount,
                ProjectId = response.ProjectId,
                PurchaseOrderCurrency = response.PurchaseOrderCurrency,
                Id = response.Id,
                PurchaseOrderItems = response.PurchaseOrderItems,
                QuoteCurrency = response.QuoteCurrency,
                QuoteNo = response.QuoteNo,
                Supplier = response.Supplier,
                TaxCodeLD = response.Supplier!.TaxCodeLD,
                TaxCodeLP = response.Supplier!.TaxCodeLP,
                USDCOP = response.USDCOP,
                USDEUR = response.USDEUR,
                PONumber = response.PONumber,
                ExpectedDate = response.ExpectedDate,
                IsTaxEditable = response.IsTaxEditable,
                ApprovedDate = response.ApprovedDate,
                ClosedDate = response.ClosedDate,
                ReceivingDate = response.ReceivingDate,
                PurchaseOrderStatus = response.PurchaseOrderStatus,
                ReceivingUSDCOP = response.ReceivingUSDCOP,
                ReceivingUSDEUR = response.ReceivingUSDEUR,


            };
        }
        public static EditPurchaseOrderSalaryRequest ToEditSalaryClosed(this PurchaseOrderResponse response)
        {
            var result = new EditPurchaseOrderSalaryRequest();


            result.CostCenter = response.CostCenter;
   
       
            result.IsCapitalizedSalary = response.IsCapitalizedSalary;
      
            result.MainBudgetItemId = response.MainBudgetItemId;
            result.Name = response.Name;
           
            result.ProjectAccount = response.ProjectAccount;
            result.ProjectId = response.ProjectId;
            result.PurchaseOrderCurrency = response.PurchaseOrderCurrency;
            result.Id = response.Id;
            result.PurchaseOrderItems = response.PurchaseOrderItems;
          
            result.PONumber = response.PONumber;
        
            result.ValueUSD = response.PurchaseOrderItems.Sum(x => x.ActualPurchaseOrderUSD);
            result.SalaryDate = response.CurrencyDate;


            return result;
        }

    }
}
