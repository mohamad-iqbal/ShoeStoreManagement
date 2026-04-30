using ShoeStoreManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Entities
{
    public class InventoryTransaction
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public TypeTransaction Type { get; set; }
        public int Quantity { get; set; }
        public DateTime Date {  get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }

        // Navigation
        public ProductVariant ProductVariant { get; set; } = null!;
        public Order Order { get; set; } = null!;
        public User User { get; set; } = null!;

    }
}
