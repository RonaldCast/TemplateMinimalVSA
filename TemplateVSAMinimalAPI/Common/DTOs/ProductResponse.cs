namespace TemplateVSAMinimalAPI.Common.DTOs
{
    public sealed record ProductResponse(Guid Id, string Name, string Description, decimal Price, string CategoryName);

}
