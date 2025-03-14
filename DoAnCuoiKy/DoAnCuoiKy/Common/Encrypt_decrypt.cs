using System.Text;

namespace DoAnCuoiKy.Common
{
    public class Encrypt_decrypt
    {
        private static  string key { get; set; } = "nhuepdonghai850b7bbsieu405cto8d0fkhong4c4c5lo080nhldc0";
        //readonly chỉ đọc, không cho sửa, giữ nguyên giá trị trong suốt chương trình
       public static string EncodePassword(string pass, string salt)
        {
            var bytes = Encoding.Unicode.GetBytes(pass);
            var src = Convert.FromBase64String(salt);

        }
    }
}
