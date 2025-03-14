using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        public AuthenticationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {
            BaseResponse<LoginResponse> response = new BaseResponse<LoginResponse>();
            Librarian librarian = await _context.librarians.Where(x=>x.IsDeleted == false && x.Email == loginRequest.Username).FirstOrDefaultAsync();
            if(librarian == null)
            {
                response.IsSuccess = false;
                response.message = "Chưa có tài khoản";
                return response;
            }
            //mã hóa mật khẩu để kiểm tra tài khoản
        }
    }
}
