using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.Usermanage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpPost("AddRole")]
        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponse<RoleResponse>> addRole(RoleRequest roleRequest)
        {
            BaseResponse<RoleResponse> response = await _roleService.AddRole(roleRequest);
            return response;
        }
        [HttpGet("GetAllRole")]
        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponse<List<RoleResponse>>> getAllRole()
        {
            BaseResponse<List<RoleResponse>> response = await _roleService.GetAllRole();
            return response;
        }
    }
}
