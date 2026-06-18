using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class CreateReturnItemDto
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
    }
}
