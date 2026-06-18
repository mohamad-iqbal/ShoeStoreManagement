using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Entities
{
    public class ReturnItem
    {
        public int Id { get; set; }
        public int ReturnId { get; set; }
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal RefundAmount { get; set; }

        // Navigation
        public Return Return { get; set; } = null!;
        public OrderItem OrderItem { get; set; } = null!;

    }
}
