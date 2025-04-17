using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;

namespace DoAnCuoiKy.Service.IService.InformationLibrary.Kho
{
    public interface IBookShelfService
    {
        Task<BaseResponse<BookShelfResponse>> AddBookShelf(BookShelfRequest bookShelfRequest);
        Task<BaseResponse<List<BookShelfResponse>>> GetAllBookShelf();
        Task<BaseResponse<BookShelfResponse>> GetBookShelfById(Guid id);
        Task<BaseResponse<BookShelfResponse>> UpdateBookShelf(Guid id, BookShelfRequest bookShelfRequest);
        Task<BaseResponse<BookShelfResponse>> DeleteBookShelf(Guid id);
    }
}
