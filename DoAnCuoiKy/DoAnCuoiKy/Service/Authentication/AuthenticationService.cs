using System.Net.Mail;
using System.Net;
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
using DoAnCuoiKy.Utils;
using DoAnCuoiKy.Model;


namespace DoAnCuoiKy.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private static Dictionary<string, OtpData> otpStore = new(); 
        

        public AuthenticationService(ApplicationDbContext context, IConfiguration Configuration, IHttpContextAccessor httpContextAccessor )
        {
            _configuration = Configuration;
            _context = context;
           _contextAccessor = httpContextAccessor;
        }


        public string GetJwtSecretKey()
        {
            return _configuration["Jwt:Key"];
        }
       
        public async Task<BaseResponse<LoginResponse>> Login(LoginRequest loginRequest)
        {
            BaseResponse<LoginResponse> response = new BaseResponse<LoginResponse>();
            LoginResponse loginResponse = new LoginResponse();
            Librarian librarian = await _context.librarians.Where(x => x.IsDeleted == false && x.Email == loginRequest.Email).Include(x => x.Role).FirstOrDefaultAsync();
           
            if (librarian == null)
                return Global.getResponse(false, loginResponse, "Tài khoản không tồn tại");

            if (!librarian.isValidate)
                return Global.getResponse(false, loginResponse, "Tài khoản chưa được xác nhận");
        
            string hashInputPassword = Encrypt_decrypt.EncodePassword(loginRequest.Password, librarian.Salt);
            if (hashInputPassword != librarian.Password)
                return Global.getResponse(false, loginResponse, "Mat khau khong chinh xac");
      
            Role role = await _context.roles.Where(x => x.IsDeleted == false && x.Id == librarian.RoleId).FirstOrDefaultAsync();
            TokenRequest tokenRequest = new TokenRequest();
            tokenRequest.Id = librarian.Id.Value;
            tokenRequest.RoleName = role.Name;
            tokenRequest.Name = librarian.Name;

            string jwtToken = Encrypt_decrypt.GenerateJwtToken(tokenRequest, _configuration);
            _contextAccessor.HttpContext.Response.Cookies.Append("jwtToken", jwtToken ?? "", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,//vì fe và be chạy trên 2 domain khác nhau là 3000 và 7260, set có SameSite=Strict, điều này có thể khiến nó không được gửi khi truy cập từ một domain khác.
                Secure = true,
                Expires = DateTime.Now.AddDays(1)
            });
            loginResponse.Token = jwtToken;
            loginResponse.Email = librarian.Email;
            loginResponse.RoleName = role.Name;
            return Global.getResponse(true, loginResponse, "Đăng nhập thành công!");
        }

        public async Task<BaseResponse<RegisterResponse>> Register(RegisterRequest registerRequest)
        {
            BaseResponse<RegisterResponse> response = new BaseResponse<RegisterResponse>();
            RegisterResponse registerResponse = new RegisterResponse();
            Librarian librarian = await _context.librarians.Where(x => x.IsDeleted == false && x.Email == registerRequest.Email).Include(x => x.Role).FirstOrDefaultAsync();
            if (librarian != null)
               return Global.getResponse(false, registerResponse, "Email da ton tai");
            
            string salt = Encrypt_decrypt.GenerateSalt();
            string hashPassword = Encrypt_decrypt.EncodePassword(registerRequest.Password, salt);
            Librarian newLibrarian = new Librarian();
            newLibrarian.Name = registerRequest.Name;
            newLibrarian.Password = hashPassword;
            newLibrarian.Email = registerRequest.Email;
            newLibrarian.Salt = salt;
            Role role = _context.roles.FirstOrDefault(x => x.Name == "User" && x.IsDeleted == false);
            newLibrarian.Role = role;

            registerResponse.Email = newLibrarian.Email;
            string otp = getOTP();
           
            if(!sendEmail(otp, newLibrarian.Email))
            {
                response.IsSuccess = false;
                response.message = "Gmail không hợp lệ";
                response.data = registerResponse;
                return response;
            }
            _context.librarians.Add(newLibrarian);
            _context.SaveChanges();
            OtpData data = new OtpData();
            //otpStore = new Dictionary<string, OtpData>();
            data.Code = otp;
            data.ExpiredAt = DateTime.Now.AddMinutes(5);
            otpStore[newLibrarian.Email] = data;
            response.IsSuccess = true;
            response.message = "Bạn cần kiểm tra email xác nhận OTP";
            response.data = registerResponse;
            return response;
        }
        private string getOTP()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private string htmlEmail(string email, string otp)
        {
            return "Xin chào " + email + ", bạn đã đăng ký tài khoản thư viện điện tử của UTC2." +
            " Đây là mã xác nhận OTP là: " + otp;
        }
        private bool sendEmail(string otp, string email)
        {
            string body = htmlEmail(email, otp);
            string title = "Mã xác nhận OTP....";
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com"; 
                    smtp.Port = 587; 
                    smtp.EnableSsl = true; 
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = "h09052003n@gmail.com",
                        Password = "debskzkkbtkmqdfe"
                    };
                }
                MailAddress fromAddress = new MailAddress("h09052003n@gmail.com", "UTC2Store");
                message.From = fromAddress;
                message.To.Add(email);
                message.Subject = title;
                message.IsBodyHtml = true;
                message.Body = body;
                smtp.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public  Task<BaseResponse<OtpResponse>> ConfirmOTP(OtpRequest otpRequest)
        {
            BaseResponse<OtpResponse> response = new BaseResponse<OtpResponse>();
            var libraryan = _context.librarians.Where(x => x.Email == otpRequest.Email).FirstOrDefault();
            if (!otpStore.ContainsKey(otpRequest.Email))
            {
                response.IsSuccess = false;
                response.message = "Mã Otp không tồn tại";
                response.data = new OtpResponse { isValidate = false };
                return  Task.FromResult(response);
            }
            var storedOtp = otpStore[otpRequest.Email];
            if (DateTime.Now > storedOtp.ExpiredAt) {
                response.IsSuccess = false;
                response.message = "Mã đã hết hiệu lực";
                if(libraryan != null)
                {
                    _context.librarians.Remove(libraryan);
                    _context.SaveChanges();
                }    
                response.data = new OtpResponse { isValidate = false };       
                return  Task.FromResult(response);
            }
            if (storedOtp.Code != otpRequest.ConfirmOtp)
            {
                response.IsSuccess = false;
                response.message = "Mã OTP không chính xác";
                response.data = new OtpResponse { isValidate = false };
                return  Task.FromResult(response);
            }
            
            if(libraryan == null)
            {
                response.IsSuccess = false;
                response.message = "Lỗi hệ thống";
                response.data = new OtpResponse { isValidate = false };
                return  Task.FromResult(response);
            }
           
            libraryan.isValidate = true;
             _context.librarians.Update(libraryan);
             _context.SaveChangesAsync();
            otpStore.Remove(otpRequest.Email);
            response.IsSuccess = true;
            response.message = "Xác minh OTP thành công";
            response.data = new OtpResponse
            {
                isValidate = true,
                ConfirmOtp = otpRequest.ConfirmOtp,
            };
            return  Task.FromResult(response);
        }

        public  Task<BaseResponse<LogoutResponse>> Logout()
        {
           BaseResponse<LogoutResponse> response = new BaseResponse<LogoutResponse>();
            LogoutResponse logoutResponse = new LogoutResponse();
            logoutResponse.Name = getCurrentName();
            logoutResponse.RoleName = getCurrentRole();
            logoutResponse.Token = getCurrentToken();
            _contextAccessor.HttpContext.Response.Cookies.Delete("jwtToken");

            response.IsSuccess = true;
            response.message = "Đăng xuất thành công";
            response.data= logoutResponse;
            return Task.FromResult(response);
            

        }
        private string getCurrentName()
        {
            return _contextAccessor.HttpContext.User.Identity.Name;
        }
        private string getCurrentRole()
        {
            return _contextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == "RoleName")?.Value ?? "Unknown";
        }
        private string getCurrentToken()
        {
            return _contextAccessor.HttpContext?.Request?.Cookies["jwtToken"] ?? string.Empty;
        }
    }
}
