using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class CreateReturnDto
    {
        public int OrderId { get; set; }
        public List<CreateReturnItemDto> ReturnItems { get; set; } = new ();
    }
}
