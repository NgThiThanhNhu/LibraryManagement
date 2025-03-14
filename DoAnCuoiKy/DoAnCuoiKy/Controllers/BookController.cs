using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpPost("AddBook")]
        public async Task<BaseResponse<BookResponse>> addBook(Guid CategoryId, Guid BookChapterId, BookRequest bookRequest)
        {
            BaseResponse<BookResponse> response = await _bookService.AddBook(CategoryId, BookChapterId, bookRequest);
            return response;
        }
        [HttpGet("GetAllBook")]
        public async Task<BaseResponse<List<BookResponse>>> getAllBook()
        {
            BaseResponse<List<BookResponse>> response = await _bookService.GetAllBook();
            return response;
        }
        [HttpGet("GetBookById/{id}")]
        public async Task<BaseResponse<BookResponse>> getBookById(Guid id)
        {
            BaseResponse<BookResponse> response = await _bookService.GetBookById(id);
            return response;
        }

        [HttpPost("UpdateBook/{id}")]
        public async Task<BaseResponse<BookResponse>> updateBook(Guid id, BookRequest bookRequest)
        {
            BaseResponse<BookResponse> response = await _bookService.UpdateBook(id, bookRequest);
            return response;
        }
        [HttpPost("DeleteBook/{id}")]
        public async Task<BaseResponse<BookResponse>> deleteBook(Guid id)
        {
            BaseResponse<BookResponse> response = await _bookService.DeleteBook(id);
            return response;
        }
    }
}
