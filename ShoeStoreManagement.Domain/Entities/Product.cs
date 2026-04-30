using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Sku {  get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }

        // Navigation
        public Store Store { get; set; } = null!;
        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}
