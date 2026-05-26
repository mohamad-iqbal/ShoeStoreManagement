using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public int Price { get; set; }
        public int StoreId { get; set; }
        public List<UpdateProductVariantDto> Variants { get; set; }
            = new();
    }
}
