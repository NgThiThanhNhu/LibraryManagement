using DoAnCuoiKy.Common;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.Authentication
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
