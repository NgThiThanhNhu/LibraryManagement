using DoAnCuoiKy.Model.Request;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DoAnCuoiKy.Common
{
    public class Encrypt_decrypt
    {
        private static string key { get; set; } = "nhuepdonghai850b7bbsieu405cto8d0fkhong4c4c5lo080nhldc0";
        //readonly chỉ đọc, không cho sửa, giữ nguyên giá trị trong suốt chương trình
        //hàm tạo một salt ngẫu nhiên để mã hóa mật khẩu
        public static string GenerateSalt()
        {
            var buf = new byte[16];
            (new System.Security.Cryptography.RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }
        public static string EncodePassword(string pass, string salt)
        {
            var bytes = Encoding.Unicode.GetBytes(pass);
            var src = Convert.FromBase64String(salt);
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(pass + salt));
            //encoding.default.getbyte dùng để mã hóa 2 chuỗi pass và salt thành một mảng byte, sau đó computeHash sẽ băm theo mã MD5 và trả ra một mảng byte là giá trị hash
            //vì các giao thức http không hỗ trợ truyền tải dữ liệu nhị phân tức là data nhị phân sau khi băm nên ta phải convert data thành base64
            return Convert.ToBase64String(data);//chuỗi base64 có thể lưu trữ được trong cơ sở dữ liệu, json hoặc bất kỳ cấu trúc văn bản nào
        }

        public static string GenerateJwtToken(TokenRequest tokenRequest, IConfiguration config)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, tokenRequest.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, tokenRequest.Name),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, tokenRequest.RoleName) // Phân quyền đúng cách
    };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"], // Đảm bảo Issuer không null
                audience: config["Jwt:Audience"], // Đảm bảo Audience hợp lệ
                claims: claims,
                expires: DateTime.Now.AddHours(Common.CommonConst.ExpireTime),
                signingCredentials: credentials
            );
            return tokenHandler.WriteToken(token);
        }
    }
}
