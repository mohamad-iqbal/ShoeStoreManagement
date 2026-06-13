using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation
        public Order Order { get; set; } = null!;
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
