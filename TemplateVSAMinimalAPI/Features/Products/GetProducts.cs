using AutoMapper;
using AutoMapper.QueryableExtensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TemplateVSAMinimalAPI.Common.DTOs;
using TemplateVSAMinimalAPI.Domain.Entities;
using TemplateVSAMinimalAPI.Persistence;

namespace TemplateVSAMinimalAPI.Features.Products
{
    public partial class GetProducts : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("products", async (ISender sender) =>
            {
                return Results.Ok(await sender.Send(new GetProductsQuery()));
            });
      
        }

        public sealed record GetProductsQuery : IRequest<List<ProductResponse>> { }

        public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductResponse>>
        {

            private readonly AppDbContext _context;

            private readonly IMapper _mapper;
            public GetProductsHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<List<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
            {
                
                return await _context.Products.ProjectTo<ProductResponse>(_mapper.ConfigurationProvider).ToListAsync();
            }
        }

        public sealed class GetProductsMappingProfile : Profile
        {
            public GetProductsMappingProfile()
            {
                NewMethod();

                void NewMethod()
                {
                    CreateMap<Product, ProductResponse>().ForMember(p => p.CategoryName, opt =>
                    opt.MapFrom(c => c.Category != null ? c.Category.Name : string.Empty));
                }
            }

        }
                
    }
}
