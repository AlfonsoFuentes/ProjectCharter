
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;
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
                var path = host.WebRootPath;

                if (path == null)
                {
                    mesajes.Append("path not found");

                    return;
                }
                Console.WriteLine(path);
                var rutaImagen = Path.Combine(path, "Assets/CPLogo.PNG");
                CPLogo = System.IO.File.ReadAllBytes(rutaImagen);

                mesajes.Append($"CPLogo: created");
                rutaImagen = Path.Combine(path, "Assets/PMLogo.PNG");
                PMLogo = System.IO.File.ReadAllBytes(rutaImagen);
                mesajes.Append($"PMLogo: created");
            }
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Projects.EndPoint.ProjectPlann, async (ProjectGetAllExport request, [FromServices] IWebHostEnvironment host, IQueryRepository Repository) =>
                {
                    Func<IQueryable<Project>, IIncludableQueryable<Project, object>> Includes = x => x
                    .Include(x => x.StakeHolders).ThenInclude(x => x.RoleInsideProject!)
                    .Include(x => x.BackGrounds.OrderBy(x => x.Order))
                    .Include(x => x.Objectives.OrderBy(x => x.Order))
                    .Include(x => x.Requirements.OrderBy(x => x.Order))
                    .Include(x => x.Scopes.OrderBy(x => x.Order))
                    .Include(x => x.AcceptanceCriterias.OrderBy(x => x.Order))
                    .Include(x => x.Bennefits.OrderBy(x => x.Order))
                    .Include(x => x.Constrainsts.OrderBy(x => x.Order))
                    .Include(x => x.Assumptions.OrderBy(x => x.Order))
                    .Include(x => x.LearnedLessons.OrderBy(x => x.Order))
                    .Include(x => x.ExpertJudgements.OrderBy(x => x.Order)).ThenInclude(x => x.Expert!)
            
                    .Include(x => x.Qualitys.OrderBy(x => x.Order))
                    .Include(x => x.KnownRisks.OrderBy(x => x.Order))
                    .Include(x => x.Resources.OrderBy(x => x.Order))
                    .Include(x => x.Acquisitions.OrderBy(x => x.Order))
                 ;

                    Expression<Func<Project, bool>> Criteria = x => x.Id == request.ProjectId;
                    string CacheKey = StaticClass.Projects.Cache.GetById(request.ProjectId);
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
                                col2.Item().Background(Colors.Grey.Lighten2).Text("Project Charter Statement").FontSize(20).AlignCenter();
                            });

                            col1.Item().Element((ele) => ProjectNameContent(ele, response));
                      

                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("A) StakeHolders").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => StakeHoldersContent(ele, response));
                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("B) Scope Management").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => BackGrounds(ele, response));
                            col1.Item().Element((ele) => Objetives(ele, response));
                            col1.Item().Element((ele) => Requirements(ele, response));
                            col1.Item().Element((ele) => Scopes(ele, response));
                            col1.Item().Element((ele) => AcceptanceCriteria(ele, response));
                            col1.Item().Element((ele) => Bennefits(ele, response));
                            col1.Item().Element((ele) => Constrainsts(ele, response));
                            col1.Item().Element((ele) => Assumptions(ele, response));
                            col1.Item().Element((ele) => LearnedLessons(ele, response));
                            col1.Item().Element((ele) => ExpertJudgements(ele, response));
                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("C) Timeline Management").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => Deliverables(ele, response));
                            col1.Item().Element((ele) => Milestones(ele, response));
                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("D) Quality Management").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => Qualitys(ele, response));
                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("E) Risks Management").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => KnownRisks(ele, response));
                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("F) Resources Management").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => Resources(ele, response));
                            col1.Item().Padding(10).Column(col2 =>
                            {
                                col2.Item().Text("G) Acquisition Management").FontSize(10).Bold();
                            });
                            col1.Item().Element((ele) => Acquisitions(ele, response));


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
                            txt.Span("Project Name: ").SemiBold().FontSize(20);
                            txt.Span(response.Name).SemiBold().FontSize(20);
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
                if (response.BackGrounds.Count == 0) return;
                container.Column(col1 =>
                {

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
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
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
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

                    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
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
                            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
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
            void Deliverables(IContainer container, Project response)
            {
                //if (response.Deliverables.Count == 0) return;
                //container.Column(col1 =>
                //{

                //    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                //    {
                //        col2.Item().Text(txt =>
                //        {
                //            txt.Span("Deliverables:").FontSize(10).SemiBold();

                //        });
                //    });


                //    col1.Item().Column(col2 =>
                //    {

                //        foreach (var row in response.Deliverables)
                //        {
                //            col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                //            {
                //                txt.Span($"{row.Name}").FontSize(10);

                //            });

                //        }

                //    });
                //});
            }
            void Milestones(IContainer container, Project response)
            {
                //if (response.Milestones.Count == 0) return;
                //container.Column(col1 =>
                //{

                //    col1.Item().TranslateX(15).PaddingBottom(5).Column(col2 =>
                //    {
                //        col2.Item().Text(txt =>
                //        {
                //            txt.Span("Milestones:").FontSize(10).SemiBold();

                //        });
                //    });


                //    col1.Item().Column(col2 =>
                //    {

                //        //foreach (var row in response.Milestones)
                //        //{
                //        //    col1.Item().TranslateX(20).PaddingBottom(5).ShowEntire().AlignLeft().Text(txt =>
                //        //    {
                //        //        txt.Span($"{row.Name}").FontSize(10);

                //        //    });

                //        //}

                //    });
                //});
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
        }

    }
}
