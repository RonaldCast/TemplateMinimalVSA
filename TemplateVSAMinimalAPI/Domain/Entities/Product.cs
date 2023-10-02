using System.ComponentModel.DataAnnotations.Schema;

namespace TemplateVSAMinimalAPI.Domain.Entities
{
    public class Product : IHasDomainEvent
    {
        
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public Guid? CategoryId { get; private set; } 
        public virtual Category? Category { get; private set; }

        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

        /*public void UpdateInfo() 
        {
            if (Price != command.Price)
            {
                DomainEvents.Add(new ProductUpdatePriceEvent(this));
            }

            Name = command.Name!;
            Description = command.Description!;
            Price = command.Price;
            CategoryId = command.CategoryId;
        }*/

    }
}
