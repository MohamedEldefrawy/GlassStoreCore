using GlassStoreCore.BL;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlassStoreCore.Services.RolesService
{
    public class RolesService : Service<IdentityRole>, IRolesService
    {
        private readonly ObjectMapper _mapper = new ObjectMapper();
        private readonly GlassStoreContext _context;

        public RolesService(GlassStoreContext context) :
            base(context)
        {
            _context = context;
        }

        public  void Update(UpdateRoleDto roleDto, string id)
        {
            var selectedRole = _context.Roles.FindAsync(id).Result;
            selectedRole.Name = roleDto.Name;
            _context.Entry(selectedRole).State = EntityState.Modified;
        }
    }
}
