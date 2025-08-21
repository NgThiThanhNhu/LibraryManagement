using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookService
    {
        Task<BaseResponse<BookResponse>> AddBook(BookRequest bookRequest);
        Task<BaseResponse<List<BookResponse>>> GetAllBook();
        Task<BaseResponse<BookResponse>> GetBookBySlug(string slug);
        Task<BaseResponse<BookResponse>> UpdateBook(Guid id, BookRequest bookRequest);
        Task<BaseResponse<BookResponse>> DeleteBook(Guid id);
    }
}
