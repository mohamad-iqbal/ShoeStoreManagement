using ShoeStoreManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class CreateOrderDto
    {        
        public int CustomerId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public List<CreateOrderItemsDto> OrderItems { get; set; }
            = new ();
    }
}
