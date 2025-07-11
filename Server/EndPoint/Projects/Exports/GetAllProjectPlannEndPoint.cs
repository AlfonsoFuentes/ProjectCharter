
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Server.Database.Entities.BudgetItems;
using Shared.ExtensionsMetods;
using System.Text;
using static MudBlazorWeb.Pages.ProjectDependant.ExecutionPlan.ExecutionPlanPage;
using static Shared.StaticClasses.StaticClass;
namespace Server.EndPoint.Projects.Exports
{
    public static class GetAllProjectExecutionPlanEndPoint
    {

        public class EndPoint : IEndPoint
        {

            byte[] CPLogo = null!;
            byte[] PMLogo = null!;

            StringBuilder mesajes = new StringBuilder();
            void GetImageData(IWebHostEnvironment host)
            {
                var path = host.ContentRootPath;

                if (path == null)
                {
                    mesajes.Append("Server path not found");
                    path = host.WebRootPath;
                    if (path == null)
                    {
                        mesajes.Append("Web path not found");
                        Console.WriteLine(mesajes.ToString());
                        return;
                    }
                    else
                    {
                        mesajes.Append("Web path  found");
                        Console.WriteLine(mesajes.ToString());
                    }
                }
                else
                {
                    mesajes.Append("Server path found");
                    Console.WriteLine(path);
                }

                try
                {
                    var rutaImagen = Path.Combine(path, "Assets/CPLogo.PNG");
                    CPLogo = System.IO.File.ReadAllBytes(rutaImagen);

                    mesajes.Append($"CPLogo: created");
                    rutaImagen = Path.Combine(path, "Assets/PMLogo.PNG");
                    PMLogo = System.IO.File.ReadAllBytes(rutaImagen);
                    mesajes.Append($"PMLogo: created");
                    Console.WriteLine(mesajes.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Projects.EndPoint.ProjectPlann, async (ProjectGetAllExport request, [FromServices] IWebHostEnvironment host, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> Includes = x => x

                    .Include(x => x.StakeHolders).ThenInclude(x => x.RoleInsideProject!)
                    .Include(x => x.Deliverables.OrderBy(x => x.Order)).ThenInclude(x => x.NewGanttTasks).ThenInclude(x => x.BudgetItemNewGanttTasks)
                    .Include(x => x.Qualitys.OrderBy(x => x.Order))
                    .Include(x => x.Objectives.OrderBy(x => x.Order))
                    .Include(x => x.Scopes.OrderBy(x => x.Order))
                    .Include(x => x.KnownRisks.OrderBy(x => x.Order))
                    .Include(x => x.Resources.OrderBy(x => x.Order))
                    .Include(x => x.Acquisitions.OrderBy(x => x.Order))
                    .Include(x => x.Communications.OrderBy(x => x.Order))

                     .Include(p => p.BudgetItems).ThenInclude(x => (x as Instrument)!.InstrumentItems!).ThenInclude(x => x.InstrumentTemplate!).ThenInclude(x => x.BrandTemplate!)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.FluidCode)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Pipe)!.PipeItems!).ThenInclude(x => x.PipeTemplate!)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Valve)!.ValveItems!).ThenInclude(x => x.ValveTemplate!).ThenInclude(x => x.BrandTemplate!)
                .Include(p => p.BudgetItems).ThenInclude(x => (x as Equipment)!.EquipmentItems!).ThenInclude(x => x.EquipmentTemplate!).ThenInclude(x => x.BrandTemplate!)
               .Include(x => x.BudgetItems).ThenInclude(x => x.BudgetItemNewGanttTasks)/*.ThenInclude(x => x.SelectedBasicEngineeringItem!)*/
                 ;

                    Expression<Func<Project, bool>> Criteria = x => x.Id == request.ProjectId;
                    string CacheKey = StaticClass.Projects.Cache.GetByIdExecutionPlan(request.ProjectId);
                    var project = await Repository.GetAsync(Cache: CacheKey, Criteria: Criteria, Includes: Includes);

                    if (project == null) return Result<Shared.Models.FileResults.FileResult>.Fail();
                    mesajes.Append("Ingresado a exportar");
                    GetImageData(host);


                    var responsePDF = CreatePDF(project);


                    return Result<Shared.Models.FileResults.FileResult>.Success(responsePDF, mesajes.ToString());
                });
            }
            Shared.Models.FileResults.FileResult CreatePDF(Project response)
            {
                byte[] pdfBytes = GenerateReportBytes(response);

                Shared.Models.FileResults.FileResult newresult = new()
                {
                    Data = pdfBytes,
                    ExportFileName = $"Project Execution Plan of {response.Name}.pdf",
                    ContentType = Shared.Models.FileResults.FileResult.pdfContentType,
                };

                return newresult;
            }
            byte[] GenerateReportBytes(Project response)
            {
                byte[] reportBytes;
                Document document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);

                        page.Size(PageSizes.Letter.Portrait());
                        page.MarginLeft(2f, QuestPDF.Infrastructure.Unit.Centimetre);
                        page.MarginRight(2f, QuestPDF.Infrastructure.Unit.Centimetre); // Ajustar el margen derecho
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));
                        page.Header().Row(row =>
                        {
                            if (CPLogo == null)
                            {
                                row.ConstantItem(100).Column(col =>
                                {
                                    col.Item().AlignCenter().Text("Colgate Palmolive").FontColor("#422ef2").Bold().FontSize(16).Italic();

                                });
                            }
                            else
                            {
                                row.ConstantItem(100).Image(CPLogo);
                            }

                            row.RelativeItem().Column(col =>
                            {
                                col.Item().AlignCenter().Text("Confidential").FontSize(14);

                            });
                            if (PMLogo == null)
                            {

                                row.ConstantItem(100).Column(col =>
                                {
                                    col.Item().AlignCenter().Text("Project Management").FontColor("#422ef2").Bold().FontSize(16).Italic();

                                });
                            }
                            else
                            {
                                row.ConstantItem(100).Image(PMLogo);
                            }

                        });
                        page.Footer().AlignRight().Text(txt =>
                        {
                            txt.Span("Page ").FontSize(10);
                            txt.CurrentPageNumber().FontSize(10);
                            txt.Span(" of ").FontSize(10);
                            txt.TotalPages().FontSize(10);
                        });
                        page.Content().PaddingVertical(10).Column(col1 =>
                        {
                            col1.Item().PaddingBottom(15).Column(col2 =>
                            {
                                col2.Item().Background(Colors.Grey.Lighten2).Text("Project execution plann").FontSize(20).AlignCenter();
                            });

                            col1.Item().Element((ele) => ProjectNameContent(ele, response));

                            col1.Item().Element((ele) => Objetives(ele, response));
                            col1.Item().Element((ele) => Scopes(ele, response));


                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("A) Timeline Management").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => GetMilestones(ele, response));
                            col1.Item().Element((ele) => GetGanttTask(ele, response));


                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("B) Budget").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => GetBudget(ele, response));
                            col1.Item().Element((ele) => GetExpendingtool(ele, response));


                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("C) Resources Management").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => Resources(ele, response));
                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("D) Acquisition Management").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => Acquisitions(ele, response));

                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("E) Communication Plan").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => Communications(ele, response));

                            col1.Item().LineHorizontal(0.5f);
                            col1.Item().Element((ele) => SignContent(ele, response));
                        });

                    });
                });

                reportBytes = document.GeneratePdf();

                return reportBytes;
            }

            void ProjectNameContent(IContainer container, Project response)
            {
                container.Column(col1 =>
                {

                    col1.Item().PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Project Name: ").SemiBold().FontSize(10);
                            txt.Span(response.Name).SemiBold().FontSize(10);
                        });
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Project Code: ").SemiBold().FontSize(10);
                            txt.Span($"CEC0000{response.ProjectNumber}").SemiBold().FontSize(10);
                        });
                    });


                });
            }




            void StakeHoldersContent(IContainer container, Project response)
            {
                if (response.StakeHolders.Count == 0) return;
                container.Column(col1 =>
                {
                    col1.Item().TranslateX(15).PaddingBottom(5).Table(table => GetStakeHoldersTable(table, response));
                });
            }
            TableDescriptor GetStakeHoldersTable(TableDescriptor tabla, Project response)
            {
                tabla.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(2);


                });

                tabla.Header(header =>
                {
                    header.Cell()
                    .Padding(4).Text("Name").Bold();

                    header.Cell()
                   .Padding(4).Text("Role").Bold();




                });

                foreach (var expert in response.ExpertJudgements)
                {
                    if (expert.Expert != null)
                    {
                        tabla.Cell()
                            .Padding(4).Text(expert.Expert!.Name).FontSize(10);

                        tabla.Cell()
                        .Padding(4).Text("Expert").FontSize(10);
                    }

                }
                foreach (var stakeholder in response.StakeHolders)
                {
                    tabla.Cell()
                        .Padding(4).Text(stakeholder.Name).FontSize(10);

                    tabla.Cell()
                    .Padding(4).Text(stakeholder.RoleInsideProject!.Name).FontSize(10);


                }
                return tabla;
            }



            void SignContent(IContainer container, Project response)
            {

                container.Column(col1 =>
                {

                    col1.Item().Padding(30).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Approvals").FontSize(10).SemiBold();

                        });
                    });
                    col1.Item().Table(table => GetSign(table, response));
                });
            }
            TableDescriptor GetSign(TableDescriptor tabla, Project response)
            {
                tabla.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);

                });

                tabla.Header(header =>
                {
                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text("Name").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text("Role").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text("Sign").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text("Sign date").Bold();


                });


                foreach (var expert in response.StakeHolders)
                {
                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text(expert.Name).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text(expert.Area).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text(string.Empty).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text(string.Empty).FontSize(10);
                }

                return tabla;
            }

            void BackGrounds(IContainer container, Project response)
            {
                if (response.BackGrounds.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("BackGrounds:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.BackGrounds)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void Objetives(IContainer container, Project response)
            {
                if (response.Objectives.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Objectives:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Objectives)
                        {
                            col1.Item().TranslateX(15).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }

            void Requirements(IContainer container, Project response)
            {
                if (response.Requirements.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Requirements:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Requirements)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void Scopes(IContainer container, Project response)
            {
                if (response.Scopes.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Scopes:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Scopes)
                        {
                            col1.Item().TranslateX(15).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void AcceptanceCriteria(IContainer container, Project response)
            {
                if (response.AcceptanceCriterias.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Acceptance Criterias:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.AcceptanceCriterias)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void Bennefits(IContainer container, Project response)
            {
                if (response.Bennefits.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Bennefits:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Bennefits)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }

            void Constrainsts(IContainer container, Project response)
            {
                if (response.Constrainsts.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Constrainsts:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Constrainsts)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void Assumptions(IContainer container, Project response)
            {
                if (response.Assumptions.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Assumptions:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Assumptions)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void LearnedLessons(IContainer container, Project response)
            {
                if (response.LearnedLessons.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Learned Lessons:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.LearnedLessons)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void ExpertJudgements(IContainer container, Project response)
            {
                if (response.ExpertJudgements.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().TranslateX(15).Text(txt =>
                        {
                            txt.Span("Expert Judgements:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.ExpertJudgements)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void GetGanttTask(IContainer container, Project response)
            {

                container.Column(col1 =>
                {

                    col1.Item().Padding(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Gantt Diagram").FontSize(10).SemiBold();

                        });
                    });
                    col1.Item().Table(table => GetGanntTaskTable(table, response));
                });
            }
            TableDescriptor GetGanntTaskTable(TableDescriptor tabla, Project response)
            {
                var milestones = response.Deliverables.SelectMany(x => x.NewGanttTasks).OrderBy(x => x.MainOrder).ToList();
                tabla.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);

                });

                tabla.Header(header =>
                {
                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text("Order").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text("WBS").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text("Name").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                 .Padding(4).Text("Start date").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                 .Padding(4).Text("End date").Bold();

                });

                foreach (var deliverable in response.Deliverables)
                {
                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text(deliverable.Order.ToString()).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text(deliverable.WBS).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text(deliverable.Name).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text(deliverable.StartDate!.Value.ToString("d")).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text(deliverable.EndDate!.Value.ToString("d")).FontSize(10);
                    foreach (var gantt in deliverable.NewGanttTasks.OrderBy(x => x.MainOrder))
                    {
                        tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                       .Padding(4).Text(gantt.MainOrder.ToString()).FontSize(10);

                        tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                        .Padding(4).Text(gantt.WBS.ToString()).FontSize(10);

                        tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                        .Padding(4).Text(gantt.Name).FontSize(10);

                        tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                        .Padding(4).Text(gantt.StartDate.ToString("d")).FontSize(10);

                        tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                       .Padding(4).Text(gantt.EndDate.ToString("d")).FontSize(10);


                    }
                }



                return tabla;
            }

            void GetBudget(IContainer container, Project response)
            {

                container.Column(col1 =>
                {

                    col1.Item().Padding(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Budget Items").FontSize(10).SemiBold();

                        });
                    });
                    col1.Item().Table(table => GetBudgetTable(table, response));
                });
            }
            TableDescriptor GetBudgetTable(TableDescriptor tabla, Project response)
            {
                var budgetitems = response.BudgetItems.OrderBy(x => x.Nomenclatore).ToList();
                tabla.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);

                    columns.RelativeColumn(3);
                    columns.RelativeColumn(1);


                });

                tabla.Header(header =>
                {


                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text("#").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text("Name").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                 .Padding(4).Text("Budget, USD").Bold();


                });

                foreach (var budgetitem in budgetitems)
                {
                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text(budgetitem.Nomenclatore).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text(budgetitem.Name).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text(budgetitem.BudgetUSD.ToCurrencyCulture()).FontSize(10);


                }
                tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                  .Padding(4).Text(string.Empty).FontSize(10);

                tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
               .Padding(4).Text("Totals").FontSize(10);

                tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
               .Padding(4).Text(budgetitems.Sum(x => x.BudgetUSD).ToCurrencyCulture()).SemiBold().FontSize(10);


                return tabla;
            }

            void GetExpendingtool(IContainer container, Project response)
            {

                container.Column(col1 =>
                {

                    col1.Item().Padding(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Expending tool").FontSize(10).SemiBold();

                        });
                    });
                    col1.Item().Table(table => GetExpendingtoolTable(table, response));
                });
            }
            TableDescriptor GetExpendingtoolTable(TableDescriptor tabla, Project response)
            {
                var budgetitems = response.BudgetItems.OrderBy(x => x.Nomenclatore).ToList();
                var ganttTasks = response.Deliverables.SelectMany(x => x.NewGanttTasks).ToList();

                var mindate = ganttTasks.Min(x => x.StartDate);
                var maxdate = ganttTasks.Max(x => x.EndDate);

                // Aseguramos que los trimestres empiecen desde Enero del año de 'mindate'
                var startDateAligned = new DateTime(mindate.Year, 1, 1);

                // Calcular número total de meses entre Enero del primer año y maxdate
                var numberofmonths = (maxdate.Year - startDateAligned.Year) * 12 +
                                     (maxdate.Month - startDateAligned.Month) + 1;

                var numberofquarter = (int)Math.Ceiling(numberofmonths / 3.0); // Asegurar todos los trimestres

                // Definir columnas dinámicas por trimestre
                tabla.ColumnsDefinition(columns =>
                {
                    foreach (var i in Enumerable.Range(1, numberofquarter))
                    {
                        columns.RelativeColumn(1);
                    }
                });

                // Definir encabezados de la tabla
                tabla.Header(header =>
                {
                    var currentDate = new DateTime(startDateAligned.Year, 1, 1);

                    for (int i = 0; i < numberofquarter; i++)
                    {
                        var quarterNumber = ((currentDate.Month - 1) / 3) + 1;
                        header.Cell()
                              .Border(0.5f)
                              .BorderColor("#D9D9D9")
                              .Padding(4)
                              .Text($"Q{quarterNumber}-{currentDate.Year}")
                              .Bold();

                        currentDate = currentDate.AddMonths(3);
                    }
                });
                var percentagecontingency = response.PercentageContingency;
                var percentageengineering = response.PercentageEngineering;
                var totalpercentage = percentagecontingency + percentageengineering;
                // Rellenar datos por trimestre
                var currentDateForLoop = new DateTime(startDateAligned.Year, 1, 1);

                for (int i = 0; i < numberofquarter; i++)
                {
                    var quarterStart = new DateTime(currentDateForLoop.Year, currentDateForLoop.Month, 1);
                    var quarterEnd = quarterStart.AddMonths(3).AddDays(-1); // último día del trimestre

                    // Ajustamos para cubrir todo el día
                    var startOfDay = new DateTime(quarterStart.Year, quarterStart.Month, quarterStart.Day);
                    var endOfDay = new DateTime(quarterEnd.Year, quarterEnd.Month, quarterEnd.Day)
                                   .AddDays(1).AddTicks(-1); // hasta las 23:59:59.9999999 del último día

                    var ganttThisQuarter = ganttTasks
                        .Where(x => x.EndDate >= startOfDay && x.EndDate <= endOfDay)
                        .ToList();

                    var taskIdsInQuarter = ganttThisQuarter.Select(t => t.Id).ToList();

                    // Filtrar ítems de presupuesto asociados a esas tareas
                    var budgetitemsthisQuarter = budgetitems
                        .Where(b => b.BudgetItemNewGanttTasks.Any(y => taskIdsInQuarter.Contains(y.NewGanttTaskId)))
                        .ToList();

                    // Sumar el presupuesto USD
                    var sumBudgetUSD = budgetitemsthisQuarter.Sum(b => b.BudgetUSD);
                    sumBudgetUSD = Math.Round(sumBudgetUSD * 100 / (100-totalpercentage));
                    // Agregar celda a la tabla
                    tabla.Cell()
                         .Border(0.5f)
                         .BorderColor("#D9D9D9")
                         .Padding(4)
                         .Text(sumBudgetUSD.ToCurrencyCulture())
                         .FontSize(10);

                    // Avanzar al siguiente trimestre
                    currentDateForLoop = currentDateForLoop.AddMonths(3);
                }

                return tabla;
            }
          
            void Deliverables(IContainer container, Project response)
            {
                if (response.Deliverables.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Deliverables:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Deliverables)
                        {
                            col1.Item().TranslateX(15).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void GetMilestones(IContainer container, Project response)
            {

                container.Column(col1 =>
                {

                    col1.Item().Padding(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Milestones").FontSize(10).SemiBold();

                        });
                    });
                    col1.Item().Table(table => GetMilestonesTable(table, response));
                });
            }
            TableDescriptor GetMilestonesTable(TableDescriptor tabla, Project response)
            {
                var milestones = response.Deliverables.SelectMany(x => x.NewGanttTasks).Where(x => x.IsMilestone).OrderBy(x => x.EndDate).ToList();
                tabla.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3);
                    columns.RelativeColumn(1);


                });

                tabla.Header(header =>
                {
                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text("Name").Bold();

                    header.Cell().Border(0.5f).BorderColor("#D9D9D9")
                   .Padding(4).Text("Date").Bold();



                });


                foreach (var expert in milestones)
                {
                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text(expert.Name).FontSize(10);

                    tabla.Cell().Border(0.5f).BorderColor("#D9D9D9")
                    .Padding(4).Text(expert.EndDate.ToString("d")).FontSize(10);


                }

                return tabla;
            }

            void Qualitys(IContainer container, Project response)
            {
                if (response.Qualitys.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Qualitys:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Qualitys)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void KnownRisks(IContainer container, Project response)
            {
                if (response.KnownRisks.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Known Risks:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.KnownRisks)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void Resources(IContainer container, Project response)
            {
                if (response.Resources.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Resources:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Resources)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void Acquisitions(IContainer container, Project response)
            {
                if (response.Acquisitions.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Acquisitions:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Acquisitions)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
            void Communications(IContainer container, Project response)
            {
                if (response.Communications.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                    {
                        col2.Item().Text(txt =>
                        {
                            txt.Span("Communications:").FontSize(10).SemiBold();

                        });
                    });


                    col1.Item().Column(col2 =>
                    {

                        foreach (var row in response.Communications)
                        {
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                            {
                                txt.Span($"{row.Name}").FontSize(10);

                            });

                        }

                    });
                });
            }
        }

    }
}
