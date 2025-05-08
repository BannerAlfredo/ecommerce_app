using System.Collections.Generic;

namespace Ecommerce.Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }

        // Navegación
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category>? Subcategories { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}