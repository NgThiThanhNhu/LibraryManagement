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

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous] 
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
        [AllowAnonymous]
        [HttpPost("ConfirmOtp")]
        public async Task<BaseResponse<OtpResponse>> ConfirmOTP(OtpRequest otpRequest)
        {
            BaseResponse<OtpResponse> baseResponse = await _authenticationService.ConfirmOTP(otpRequest);
            return baseResponse;
        }
        [AllowAnonymous]
        [HttpPost("Logout")]
        public async Task<BaseResponse<LogoutResponse>> Logout()
        {
            BaseResponse<LogoutResponse> baseResponse = await _authenticationService.Logout();
            return baseResponse;

        }
    }
}
