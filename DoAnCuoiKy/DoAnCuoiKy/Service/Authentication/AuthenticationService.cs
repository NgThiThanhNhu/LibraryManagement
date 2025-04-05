using Azure;
using DoAnCuoiKy.Common;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace DoAnCuoiKy.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        

        public AuthenticationService(ApplicationDbContext context, IConfiguration Configuration)
        {
            _configuration = Configuration;
            _context = context;
           
        }
        public string GetJwtSecretKey()
        {
            return _configuration["Jwt:Key"];
        }
       

        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {
            BaseResponse<LoginResponse> response = new BaseResponse<LoginResponse>();
            Librarian librarian = await _context.librarians.Where(x => x.IsDeleted == false && x.Email == loginRequest.Username).Include(x => x.Role).FirstOrDefaultAsync();
            if (librarian == null)
            {
                response.IsSuccess = false;
                response.message = "Chưa có tài khoản";
                return response;
            }
            //mã hóa mật khẩu để kiểm tra tài khoản
            //nếu tìm thấy email trong database thì so sánh mật khẩu
            /* var passwordHasher = new PasswordHasher<Librarian>();
             var result = passwordHasher.VerifyHashedPassword(librarian, librarian.Password, loginRequest.Password);
             if (result == PasswordVerificationResult.Failed)
             {
                 response.IsSuccess = false ;
                 response.message = "Mật khẩu không đúng";
                 return response;
             }*/
            //bước 2: mã hóa mật khẩu nhập vào để so sánh
            string hashInputPassword = Encrypt_decrypt.EncodePassword(loginRequest.Password, librarian.Salt);
            if (hashInputPassword != librarian.Password)
            {
                response.IsSuccess = false;
                response.message = "Mật khẩu không đúng";
                return response;
            }
            Role role = await _context.roles.Where(x => x.IsDeleted == false && x.Id == librarian.RoleId).FirstOrDefaultAsync();
            TokenRequest tokenRequest = new TokenRequest();
            tokenRequest.Id = librarian.Id.Value;
            tokenRequest.RoleName = role.Name;
            tokenRequest.Name = librarian.Name;

            //bước 3: nếu mật khẩu khớp với mật khẩu được lưu, thì trả về token cho người dùng, sử dụng để thực hiện request theo role và các hành động cần xác thực khác
            string jwtToken = Encrypt_decrypt.GenerateJwtToken(tokenRequest, _configuration);
            //sau khi generate token thì lưu token vào cookie vì bảo mật và tránh lộ token từ phía JavaScript
            //// Lưu token vào cookie
            
            response.IsSuccess = true;
            response.message = "Đăng nhập thành công!";
            response.data = new LoginResponse
            {
                Token = jwtToken,
                UserName = librarian.Name,
                RoleName = role.Name,
            };
            return response;


        }

        public async Task<BaseResponse<RegisterResponse>> Register(RegisterRequest registerRequest)
        {
            BaseResponse<RegisterResponse> response = new BaseResponse<RegisterResponse>();
            //khi người dùng đăng ký họ sẽ nhập email của học vào
            //nhiệm vụ là kiểm tra email đó tồn tại trong database chưa
            Librarian librarian = await _context.librarians.Where(x => x.IsDeleted == false && x.Email == registerRequest.Email).Include(x => x.Role).FirstOrDefaultAsync();
            if (librarian != null)
            {
                response.IsSuccess = false;
                response.message = "Email đã tồn tại!";
                return response;
            }

            //tạo một salt ngẫu nhiên 
            string salt = Encrypt_decrypt.GenerateSalt();
            //mã hóa mật khẩu với salt
            string hashPassword = Encrypt_decrypt.EncodePassword(registerRequest.Password, salt);
            // sau khi mã hóa mật khẩu và ng dùng nhập email thì sẽ lưu chúng vào database
            Librarian newLibrarian = new Librarian();
            newLibrarian.Name = registerRequest.Name;
            newLibrarian.Password = hashPassword;
            newLibrarian.Email = registerRequest.Email;
            newLibrarian.Salt = salt;
            Role role = _context.roles.FirstOrDefault(x => x.Name == "User" && x.IsDeleted == false);
            newLibrarian.Role = role;
            _context.librarians.AddAsync(newLibrarian);
            await _context.SaveChangesAsync();

         
            RegisterResponse registerResponse = new RegisterResponse();
            registerResponse.Name = newLibrarian.Name;

            //registerResponse.Token = jwtToken;
            response.IsSuccess = true;
            response.message = "Đăng ký tài khoản thành công";
            response.data = registerResponse;
            return response;

        }
    }
}
