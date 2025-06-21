using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Utils
{
    public  static class Global
    {
        public static BaseResponse<T> getResponse<T>(bool isSuccess, T? data, string mess)
        {
            return new BaseResponse<T>
            {
                IsSuccess = isSuccess,
                message = mess,
                data = data
            };
        }
    }
}
