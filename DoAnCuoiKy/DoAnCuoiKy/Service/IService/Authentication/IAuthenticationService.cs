using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.Authentication
{
    public interface IAuthenticationService
    {
        Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest);
    }
}
