using ShoeStoreManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoeStoreManagement.Application.Interfaces
{
    public interface ICurrentUserService
    {
        int Id { get; }
        Role Role { get; }
    }
}
