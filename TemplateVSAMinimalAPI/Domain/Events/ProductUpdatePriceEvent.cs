using TemplateVSAMinimalAPI.Domain.Entities;

namespace TemplateVSAMinimalAPI.Domain.Events
{
    public class ProductUpdatePriceEvent
    {
        public Product Product { get; set; } 
        public ProductUpdatePriceEvent(Product product)
        {
            Product = product;
        }
    }
}
