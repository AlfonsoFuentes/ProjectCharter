namespace Server.EndPoint.Projects.Validators
{
    public static class ValidateProjectNumberEndPoint
    {
        public class EndPoint : IEndPoint
        {
            public void MapEndPoint(IEndpointRouteBuilder app)
            {
                app.MapPost(StaticClass.Projects.EndPoint.ValidateProjectNumber, async (ValidateProjectNumberRequest Data, IQueryRepository Repository) =>
                {
                    Expression<Func<Project, bool>> CriteriaId = null!;
                    Func<Project, bool> CriteriaExist = x => Data.Id == null ?
                    x.ProjectNumber.Equals(Data.ProjectNumber) : x.Id != Data.Id.Value && x.ProjectNumber.Equals(Data.ProjectNumber);
                    string CacheKey = StaticClass.Projects.Cache.GetAll;

                    return await Repository.AnyAsync(Cache: CacheKey, CriteriaExist: CriteriaExist, CriteriaId: CriteriaId);
                });


            }
        }



    }
}
