using System.Linq;
using GlassStoreCore.BL.APIs.Filters;
using GlassStoreCore.BL.DTOs;
using GlassStoreCore.Data.UnitOfWork;
using GlassStoreCore.Helpers;
using GlassStoreCore.Services.UriService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GlassStoreCore.BL.APIs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService _uriService;
        private readonly ObjectMapper _mapper = new ObjectMapper();

        public RolesController(IUnitOfWork unitOfWork, IUriService uriService)
        {
            _unitOfWork = unitOfWork;
            _uriService = uriService;
        }

        [HttpGet]
        public ActionResult<IdentityRole> GetRoles([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var (roles, totalRecords) = _unitOfWork.RolesService.GetAll(filter.PageNumber, filter.PageSize).Result;

            if (roles == null)
            {
                return NotFound();
            }

            var rolesDtos = roles.Select(_mapper.Mapper.Map<IdentityRole, RoleDto>).ToList();
            var pageResponse = PaginationHelper.CreatePagedResponse(rolesDtos, filter, totalRecords, _uriService, route);

            return Ok(pageResponse);
        }

        [HttpGet("{id}")]
        public ActionResult<IdentityRole> GetRole(string id)
        {
            var role = _unitOfWork.RolesService.Get(id).Result;

            if (role == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Mapper.Map<IdentityRole, RoleDto>(role));
        }

        [HttpPost]
        public ActionResult<IdentityRole> CreateRole(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please Enter a valid role data");
            }
            var role = _mapper.Mapper.Map<RoleDto, IdentityRole>(roleDto);
            _unitOfWork.RolesService.Add(role);
            _unitOfWork.Complete();
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult<IdentityRole> UpdateRole(UpdateRoleDto roleDto, string id)
        {
            var selectedRole = _mapper.Mapper.Map<UpdateRoleDto, IdentityRole>(roleDto);
            selectedRole.Id = id;

            _unitOfWork.RolesService.Update(roleDto, id);

            var result = _unitOfWork.Complete().Result;

            if (result == 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<IdentityRole> DeleteRole(string id)
        {
            if (_unitOfWork.RolesService.Get(id).Result == null)
            {
                return NotFound("please select a valid role");
            }

            _unitOfWork.RolesService.Delete(id);
            var result = _unitOfWork.Complete().Result;
            if (result == 0)
            {
                return BadRequest("Something wrong");
            }

            return Ok();
        }
    }
}
