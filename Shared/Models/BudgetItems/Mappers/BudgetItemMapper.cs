using Shared.Models.BudgetItems.BasicEngineeringItems;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicEquipments.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicInstruments.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicPipes.Responses;
using Shared.Models.BudgetItems.BasicEngineeringItems.BasicValves.Responses;
using Shared.Models.BudgetItems.Exports;
using Shared.Models.BudgetItems.IndividualItems.Equipments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Instruments.Responses;
using Shared.Models.BudgetItems.IndividualItems.Pipes.Responses;
using Shared.Models.BudgetItems.IndividualItems.Valves.Responses;
using Shared.Models.BudgetItems.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.StaticClasses.StaticClass;

namespace Shared.Models.BudgetItems.Mappers
{
    public static class BudgetItemMapper
    {
        public static BudgetItemWithPurchaseOrdersResponse Map(this BudgetItemWithPurchaseOrdersResponse budgetItem)
        {
            return new BudgetItemWithPurchaseOrdersResponse
            {
                Id = budgetItem.Id,
                Name = budgetItem.Name,

                Nomenclatore = budgetItem.Nomenclatore,
                ProjectId = budgetItem.ProjectId,
                ActualUSD = budgetItem.ActualUSD,
                CommitmentUSD = budgetItem.CommitmentUSD,
                PotentialUSD = budgetItem.PotentialUSD,
                BudgetUSD = budgetItem.BudgetUSD,
                IsAlteration = budgetItem.IsAlteration,
                IsTaxes = budgetItem.IsTaxes,

            };
        }
        public static BudgetItemExport MapToExport(this IBudgetItemResponse budgetItem)
        {


            return new BudgetItemExport(budgetItem.Nomenclatore, budgetItem.Name, budgetItem.BudgetUSD);
        }
        public static BudgetItemExport MapToExportBudgetZeroUSD(this IBudgetItemResponse budgetItem)
        {


            return new BudgetItemExport(budgetItem.Nomenclatore, budgetItem.Name, 0);
        }
        public static List<BudgetItemExport> GetExportedList(this List<BudgetItemResponse> ordereditems)
        {
            var result = new List<BudgetItemExport>();

            foreach (var item in ordereditems)
            {
                IEnumerable<BudgetItemExport>? subItems = null;

                switch (item)
                {
                    case EquipmentResponse { Items: not null } e:

                        subItems = e.Items.Select(x => ((BasicEquipmentResponse)x).MapToExportBasic(e));
                        break;

                    case InstrumentResponse { Items: not null } i:

                        subItems = i.Items.Select(x => ((BasicInstrumentResponse)x).MapToExportBasic(i));
                        break;

                    case PipeResponse { Items: not null } p:

                        subItems = p.Items.Select(x => ((BasicPipeResponse)x).MapToExportBasic(p));
                        break;

                    case ValveResponse { Items: not null } v:

                        subItems = v.Items.Select(x => ((BasicValveResponse)x).MapToExportBasic(v));
                        break;


                }

                if (subItems is not null)
                {
                    if (subItems.Sum(x => x.BudgetUSD) == 0)
                    {
                        result.Add(item.MapToExport());
                    }
                    else
                    {
                        result.Add(item.MapToExportBudgetZeroUSD());
                    }

                    result.AddRange(subItems);
                }
                else
                {
                    result.Add(item.MapToExport());
                }

            }

            return result;
        }


        public static BudgetItemExport MapToExportBasic(this BasicResponse basic, BudgetItemResponse budgetitem)
        {


            return new BudgetItemExport($"{budgetitem.Nomenclatore} - {budgetitem.Name}", $"{basic.Tag} - {basic.Name}", basic.BudgetUSD);
        }
        public static BudgetItemWithPurchaseordersExport MapToExportPO(this BudgetItemWithPurchaseOrdersResponse budgetItem)
        {
            return new BudgetItemWithPurchaseordersExport(budgetItem.Nomenclatore, budgetItem.Name, budgetItem.BudgetUSD, budgetItem.ActualUSD, budgetItem.CommitmentUSD,
                budgetItem.PotentialUSD, budgetItem.ToCommitUSD);
        }



    }
}
