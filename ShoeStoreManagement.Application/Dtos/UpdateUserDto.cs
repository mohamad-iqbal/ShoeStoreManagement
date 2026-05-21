using ShoeStoreManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public Role Role { get; set; }
        public int StoreId { get; set; }
    }
}
