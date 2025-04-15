using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookAuthorService
    {
        Task<BaseResponse<BookAuthorResponse>> AddBookAuthor(BookAuthorRequest authorRequest);
        Task<BaseResponse<List<BookAuthorResponse>>> GetAllBookAuthor();
        Task<BaseResponse<BookAuthorResponse>> GetBookAuthorById(Guid id);
        Task<BaseResponse<BookAuthorResponse>> UpdateBookAuthor(Guid Id, BookAuthorRequest authorRequest);
        Task<BaseResponse<BookAuthorResponse>> DeleteBookAuthor(Guid Id);
        
    }
}
