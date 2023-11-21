using AutoMapper;
using AutoMapper.QueryableExtensions;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TemplateVSAMinimalAPI.Common.DTOs;
using TemplateVSAMinimalAPI.Common.Filters;
using TemplateVSAMinimalAPI.Domain.Entities;
using TemplateVSAMinimalAPI.Persistence;
using static TemplateVSAMinimalAPI.Common.Filters.CommonResponseFilter;

namespace TemplateVSAMinimalAPI.Features.Products
{
    public partial class GetProducts : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("products", async (ISender sender) =>
            {
               return await sender.Send(new GetProductsQuery());

            }).WithTags(nameof(Product))
            .AddEndpointFilter<CommonResponseFilter>();
        }

        public sealed record GetProductsQuery : IRequest<CommonResponse<List<ProductResponse>>> { }

        public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, CommonResponse<List<ProductResponse>>>
        {

            private readonly AppDbContext _context;

            private readonly IMapper _mapper;
            public GetProductsHandler(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<CommonResponse<List<ProductResponse>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
            {
                var products =  await _context.Products.ProjectTo<ProductResponse>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken: cancellationToken);

                return new CommonResponse<List<ProductResponse>>(statusCode: StatusCodes.Status200OK, result: products );
            }
        }

        public sealed class GetProductsMappingProfile : Profile
        {
            public GetProductsMappingProfile()
            {
                CreateMap<Product, ProductResponse>().ForMember(p => p.CategoryName, opt =>
                opt.MapFrom(c => c.Category != null ? c.Category.Name : string.Empty));
            }

        }
                
    }
}
