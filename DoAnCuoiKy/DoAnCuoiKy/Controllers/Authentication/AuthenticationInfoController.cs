using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.Authentication
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationInfoController : ControllerBase
    {
        //[Authorize(Roles = "admin, user")] //phải phân quyền cho nó mới lấy được name và role
        [HttpGet("authenticationInfor")]
        public IActionResult Me()
        {
            //bước 1: phải xác định chính xác là người dùng đó
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //bước 2: sau khi xác định được chính xác người dùng thì lấy thông tin từ token ra
            var name = identity?.FindFirst(ClaimTypes.Name)?.Value;
            var role = identity?.FindFirst(ClaimTypes.Role)?.Value;
            return Ok(new
            {
                name = name,
                role = role
            });
        }
    }
}
