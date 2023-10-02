using AutoMapper;
using AutoMapper.QueryableExtensions;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TemplateVSAMinimalAPI.Common.DTOs;
using TemplateVSAMinimalAPI.Domain.Entities;
using TemplateVSAMinimalAPI.Persistence;

namespace TemplateVSAMinimalAPI.Features.Products
{
    public class CreateProduct : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("product", async ([FromBody] CreateProductCommand command, ISender sender) =>
            {
                return Results.Ok(await sender.Send(command));
            }).WithTags(nameof(Product));
        }

        public sealed record CreateProductCommand(string Name, string Description, decimal Price, Guid? CategoryId) : IRequest<ProductResponse>;

        public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            public CreateProductHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                var product = _mapper.Map<Product>(request);
                _context.Products.Add(product);

                await _context.SaveChangesAsync();

                return _mapper.Map<Product,ProductResponse>(product);

            }
        }

        public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
        {

            public CreateProductValidator()
            {
                RuleFor(prop => prop.Name).NotEmpty();
                RuleFor(prop => prop.Description).NotEmpty();
                RuleFor(prop => prop.Price).NotEmpty();
                RuleFor(prop => prop.CategoryId).NotEmpty();
            }

        }

        public sealed class CreateProductMappingProfile : Profile
        {
           public  CreateProductMappingProfile()
           {
                CreateMap<CreateProductCommand, Product>();
           }
        }
    }
}
