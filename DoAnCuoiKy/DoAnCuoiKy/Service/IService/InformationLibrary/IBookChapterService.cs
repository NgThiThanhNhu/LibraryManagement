using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookChapterService 
    {
        Task<BaseResponse<BookChapterResponse>> AddBookChapter(BookChapterRequest chapterRequest);
        Task<BaseResponse<List<BookChapterResponse>>> GetAllBookChapter();
        Task<BaseResponse<BookChapterResponse>> GetBookChapterById(Guid id);
        Task<BaseResponse<BookChapterResponse>> UpdateBookChapter(Guid id, BookChapterRequest chapterRequest);
        Task<BaseResponse<BookChapterResponse>> DeleteBookChapter(Guid id);

    }
}
