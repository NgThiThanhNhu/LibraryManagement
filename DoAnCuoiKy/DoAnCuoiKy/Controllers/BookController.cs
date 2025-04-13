using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponse<BookResponse>> addBook(BookRequest bookRequest)
        {
            BaseResponse<BookResponse> response = await _bookService.AddBook(bookRequest);
            return response;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllBook")]
        public async Task<BaseResponse<List<BookResponse>>> getAllBook()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            Console.WriteLine($"User Role: {userRole}");
            BaseResponse<List<BookResponse>> response = await _bookService.GetAllBook();
            return response;
        }
        [HttpGet("GetBookById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponse<BookResponse>> getBookById(Guid id)
        {
            BaseResponse<BookResponse> response = await _bookService.GetBookById(id);
            return response;
        }

        [HttpPost("UpdateBook/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponse<BookResponse>> updateBook(Guid id, BookRequest bookRequest)
        {
            BaseResponse<BookResponse> response = await _bookService.UpdateBook(id, bookRequest);
            return response;
        }
        [HttpPost("DeleteBook/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponse<BookResponse>> deleteBook(Guid id)
        {
            BaseResponse<BookResponse> response = await _bookService.DeleteBook(id);
            return response;
        }
    }
}
