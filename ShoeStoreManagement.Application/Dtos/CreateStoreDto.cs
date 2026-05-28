using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class CreateStoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
