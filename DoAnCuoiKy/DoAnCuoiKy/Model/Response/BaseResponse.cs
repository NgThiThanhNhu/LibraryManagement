namespace DoAnCuoiKy.Model.Response
{
    public class BaseResponse <T> //generic
    {
        public bool IsSuccess { get; set; }
        public string message { get; set; }
        public T? data { get; set; }
    }
}
