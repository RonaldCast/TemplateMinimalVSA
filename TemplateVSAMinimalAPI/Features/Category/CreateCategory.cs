using Carter;

namespace TemplateVSAMinimalAPI.Features.Category
{
    public class CreateCategory : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/category", async () =>
            {
                    throw new NotImplementedException();
            });
        }
    }
}
