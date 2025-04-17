using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.KhoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookShelfController : ControllerBase
    {
        private readonly IBookShelfService _bookShelfService;
        public BookShelfController(IBookShelfService bookShelfService)
        {
            _bookShelfService = bookShelfService;
        }
        [HttpPost("AddBookShelf")]
        public async Task<BaseResponse<BookShelfResponse>> addBookShelf(BookShelfRequest bookShelfRequest)
        {
            BaseResponse<BookShelfResponse> response = await _bookShelfService.AddBookShelf(bookShelfRequest);
            return response;
        }
        [HttpGet("GetAllBookShelf")]
        public async Task<BaseResponse<List<BookShelfResponse>>> getAllBookShelf ()
        {
            BaseResponse<List<BookShelfResponse>> response = await _bookShelfService.GetAllBookShelf();
            return response;
        }
        [HttpGet("GetBookShelfById/{id}")]
        public async Task<BaseResponse<BookShelfResponse>> getBookShelfById(Guid id)
        {
            BaseResponse<BookShelfResponse> response = await _bookShelfService.GetBookShelfById(id);
            return response;
        }
        [HttpPost("UpdateBookShelf/{id}")]
        public async Task<BaseResponse<BookShelfResponse>> updateBookShelf(Guid id, BookShelfRequest bookShelfRequest)
        {
            BaseResponse<BookShelfResponse> response = await _bookShelfService.UpdateBookShelf(id, bookShelfRequest);
            return response;
        }
        [HttpPost("DeleteBookShelf/{id}")]
        public async Task<BaseResponse<BookShelfResponse>> deleteBookShelf(Guid id)
        {
            BaseResponse<BookShelfResponse> response = await _bookShelfService.DeleteBookShelf(id);
            return response;
        }
    }
}
