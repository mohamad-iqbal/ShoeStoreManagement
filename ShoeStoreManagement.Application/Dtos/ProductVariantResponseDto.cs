using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class ProductVariantResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Size { get; set; }
        public int StockQty { get; set; }
    }
}
