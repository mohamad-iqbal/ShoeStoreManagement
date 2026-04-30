using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Domain.Enums
{
    public enum Status
    {
        Created = 0,
        Completed = 1,
        Canceled = 2,
        Returned = 3,
        PartiallyReturned = 4
    }
}
