using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookChapterController :ControllerBase
    {
        private IBookChapterService _bookChapterService;
        public BookChapterController(IBookChapterService bookChapterService)
        {
            _bookChapterService = bookChapterService;
        }
        [HttpPost("AddBookChapter")]
        public async Task<BaseResponse<BookChapterResponse>> addBookChapter(BookChapterRequest chapterRequest)
        {
            //dùng service
            BaseResponse<BookChapterResponse> responses = await _bookChapterService.AddBookChapter(chapterRequest);
            return responses;
        }
        [HttpGet("GetAllBookChapter")]
        public async Task<BaseResponse<List<BookChapterResponse>>> getAllBookChapter()
        {
            //lấy tất cả bookchapter sẽ trả về một kết quả nên
            BaseResponse<List<BookChapterResponse>> responses = await _bookChapterService.GetAllBookChapter();
            return responses;

        }
        [HttpGet("GetBookChapterById/{id}")]
        public async Task<BaseResponse<BookChapterResponse>> getBookChapterById(Guid id) 
        { 
            BaseResponse<BookChapterResponse> responses = await _bookChapterService.GetBookChapterById(id);
            return responses;
        }

        [HttpPost("UpdateBookChapter/{id}")]
        public async Task<BaseResponse<BookChapterResponse>> updateBookChapter(Guid id, BookChapterRequest bookChapterRequest)
        {
            BaseResponse<BookChapterResponse> responses = await _bookChapterService.UpdateBookChapter(id, bookChapterRequest);
            return responses;
        }

        [HttpPost("DeleteBookChapter/{id}")]
        public async Task<BaseResponse<BookChapterResponse>> deleteBookChapter(Guid id)
        {
            BaseResponse<BookChapterResponse> responses = await _bookChapterService.DeleteBookChapter(id);
            return responses;
        }
    }
}
