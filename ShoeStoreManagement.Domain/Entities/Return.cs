using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Entities
{
    public class Return
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalRefund { get; set; }

        // Navigation
        public Order Order { get; set; } = null!;
        public ICollection<ReturnItem> ReturnItems { get; set; }
            = new List<ReturnItem>();

    }
}
