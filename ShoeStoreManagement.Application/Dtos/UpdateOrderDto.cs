using ShoeStoreManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class UpdateOrderDto
    {
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public List<UpdateOrderItemsDto> OrderItems { get; set; }
            = new();
    }
}
