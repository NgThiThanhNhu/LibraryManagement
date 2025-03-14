using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService
{
    public interface IBookCategoryService
    {
        Task<BaseResponse<BookCategoryResponse>> AddBookCategory(BookCategoryRequest categoryRequest);
        Task<BaseResponse<List<BookCategoryResponse>>> GetAllBookCategory();
        Task<BaseResponse<BookCategoryResponse>> GetBookCategoryById(Guid id);
        Task<BaseResponse<BookCategoryResponse>> UpdateBookCategory(Guid id, BookCategoryRequest categoryRequest);
        Task<BaseResponse<BookCategoryResponse>> DeleteBookCategory(Guid id);
    }
}
