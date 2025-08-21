using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.Authentication
{
    public interface IAuthenticationService
    {
        Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest);
        Task<BaseResponse<RegisterResponse>> Register(RegisterRequest registerRequest);
        Task<BaseResponse<OtpResponse>> ConfirmOTP(OtpRequest otpRequest);
        Task<BaseResponse<LogoutResponse>> Logout();
    }
}
