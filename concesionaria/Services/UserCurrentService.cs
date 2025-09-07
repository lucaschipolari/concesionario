using Concesionario.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Data.Services
{
    public class UserCurrentService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserCurrentService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Guid?> GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new Exception("User ID claim is missing or invalid.");
            }
            return userId;
        }
    }
}
