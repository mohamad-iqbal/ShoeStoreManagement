using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class ReturnItemResponseDto
    {
        public int Id { get; set; }
        public int ReturnId { get; set; }
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal RefundAmount { get; set; }
    }
}
