using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService
{
    public interface IBookFileService
    {
        Task<BaseResponse<BookFileResponse>> UploadFileAndImage(BookFileRequest bookFileRequest);
    }
}
