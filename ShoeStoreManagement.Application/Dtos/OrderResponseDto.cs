using ShoeStoreManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public List<OrderItemsResponseDto> OrderItems { get; set; }
            = new();
    }
}
