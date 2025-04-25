using DoAnCuoiKy.Common;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Authorize]//không cần token
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfiguration _configuration;
       

        public AuthenticationController(IAuthenticationService authenticationService, IConfiguration _config)
        {
            _authenticationService = authenticationService;
          _configuration = _config;
        }
     
        [AllowAnonymous] //cho phép truy cập bất kì
        [HttpPost("Login")]
        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {

            BaseResponse<LoginResponse> baseResponse = await _authenticationService.Login(loginRequest);
            HttpContext.Response.Cookies.Append("token", baseResponse.data.Token ?? "", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,//vì fe và be chạy trên 2 domain khác nhau là 3000 và 7260, set có SameSite=Strict, điều này có thể khiến nó không được gửi khi truy cập từ một domain khác.
                Secure = true,
                Expires = DateTime.Now.AddDays(1)
            });
            // kiểm tra đăng nhập
            if (loginRequest.Username == "admin" && loginRequest.Password == "password")
            {
                var tokenRequest = new TokenRequest
                {
                    Id = Guid.NewGuid(),                         // Lấy từ DB hoặc hard-code
                    Name = loginRequest.Username,          // Lấy từ form đăng nhập
                    RoleName = "Admin"              // Lấy từ DB hoặc hard-code
                };

                var token = Encrypt_decrypt.GenerateJwtToken(tokenRequest, _configuration); // _config là IConfiguration đã inject

                Response.Cookies.Append("jwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
                Console.WriteLine("đăng nhập thành công");
            }
                return baseResponse;

           
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<BaseResponse<RegisterResponse>> Register(RegisterRequest registerRequest)
        {
            BaseResponse<RegisterResponse> baseResponse = await _authenticationService.Register(registerRequest);
            return baseResponse;
        }
    }
}
