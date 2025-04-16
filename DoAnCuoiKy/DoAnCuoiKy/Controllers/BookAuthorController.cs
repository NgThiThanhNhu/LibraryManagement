using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorController : ControllerBase
    {
        private readonly IBookAuthorService _bookAuthorService;
        public BookAuthorController(IBookAuthorService bookAuthorService)
        {
            _bookAuthorService = bookAuthorService;
        }
        [HttpPost("AddBookAuthor")]
        public async Task<BaseResponse<BookAuthorResponse>> addBookAuthor(BookAuthorRequest authorRequest)
        {
            BaseResponse<BookAuthorResponse> response = await _bookAuthorService.AddBookAuthor(authorRequest);
            return response;
        }
        [HttpGet("GetAllBookAuthor")]
        public async Task<BaseResponse<List<BookAuthorResponse>>> getAllBookAuthor()
        {
            BaseResponse<List<BookAuthorResponse>> response = await _bookAuthorService.GetAllBookAuthor();
            return response;
        }
        [HttpGet("GetBookAuthorById/{id}")]
        public async Task<BaseResponse<BookAuthorResponse>> getBookAuthorById(Guid id)
        {
            BaseResponse<BookAuthorResponse> response = await _bookAuthorService.GetBookAuthorById(id);
            return response;
        }
        [HttpPost("DeleteBookAuthor/{id}")]
        public async Task<BaseResponse<BookAuthorResponse>> deleteBookAuthor(Guid id)
        {
            BaseResponse<BookAuthorResponse> response = await _bookAuthorService.DeleteBookAuthor(id);
            return response;
        }
        [HttpPost("UpdateBookAuthor/{id}")]
        public async Task<BaseResponse<BookAuthorResponse>> updateBookAuthor(Guid id, BookAuthorRequest authorRequest)
        {
            BaseResponse<BookAuthorResponse> response = await _bookAuthorService.UpdateBookAuthor(id, authorRequest);
            return response;
        }
    }
}
