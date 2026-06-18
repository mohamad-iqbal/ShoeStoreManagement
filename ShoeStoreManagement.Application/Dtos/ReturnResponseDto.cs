using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Dtos
{
    public class ReturnResponseDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalRefund { get; set; }
        public List<ReturnItemResponseDto> ReturnItems { get; set; } = new();
    }
}
