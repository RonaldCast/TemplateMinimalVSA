using AutoMapper;
using AutoMapper.QueryableExtensions;
using Carter;
using Carter.ModelBinding;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TemplateVSAMinimalAPI.Common.DTOs;
using TemplateVSAMinimalAPI.Common.Exceptions;
using TemplateVSAMinimalAPI.Common.Filters;
using TemplateVSAMinimalAPI.Domain.Entities;
using TemplateVSAMinimalAPI.Persistence;
using static TemplateVSAMinimalAPI.Common.Filters.CommonResponseFilter;

namespace TemplateVSAMinimalAPI.Features.Products
{
    public class CreateProduct : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("product", async ([FromBody] CreateProductCommand command, ISender sender) =>
            {
                return await sender.Send(command);

            }).WithTags(nameof(Product))
            .AddEndpointFilter<CommonResponseFilter>();
        }

        public sealed record CreateProductCommand(string Name, string Description, decimal Price, Guid? CategoryId) : IRequest<CommonResponse<ProductResponse>>;

        public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, CommonResponse<ProductResponse>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;
            public CreateProductHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CommonResponse<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {

                var cartegory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken) ??
                    throw new ResponseException("Category doesn't exist");

                var product = _mapper.Map<Product>(request);
                _context.Products.Add(product);

                await _context.SaveChangesAsync(cancellationToken);

                return new CommonResponse<ProductResponse>(StatusCodes.Status201Created, _mapper.Map<Product, ProductResponse>(product), 
                    "Product was correctly created");

            }
        }

        public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
        {

            public CreateProductValidator()
            {
                RuleFor(prop => prop.Name).NotEmpty();
                RuleFor(prop => prop.Description).NotEmpty();
                RuleFor(prop => prop.Price)
                    .Must(x => x > 0).WithMessage("Must be higher than 0");
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
