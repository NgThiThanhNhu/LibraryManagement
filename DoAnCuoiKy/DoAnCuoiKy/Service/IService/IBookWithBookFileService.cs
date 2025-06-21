using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService
{
    public interface IBookWithBookFileService
    {
        Task<BaseResponse<List<BookWithBookFileResponse>>> GetAllBook();
    }
}
