using ShoeStoreManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShoeStoreManagement.Domain.Enums;

namespace ShoeStoreManagement.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int Id
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.Parse(userId);
            }

        }

        public Role Role
        {
            get
            {
                var role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
                return Enum.Parse<Role>(role!);
            }
        }

        public int StoreId
        {
            get
            {
                var storeId = _httpContextAccessor.HttpContext?.User.FindFirst("StoreId")?.Value;
                return int.Parse(storeId!);
            }
        }
    }
}
