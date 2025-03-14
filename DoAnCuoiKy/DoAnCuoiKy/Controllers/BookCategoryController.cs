using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCategoryController : ControllerBase
    {
        private IBookCategoryService _bookCategoryService;

        public BookCategoryController(IBookCategoryService bookCategoryService)
        {
            _bookCategoryService = bookCategoryService;
        }

        [HttpPost("AddBookCategory")]
        public  async Task<BaseResponse<BookCategoryResponse>> addBookCategory(BookCategoryRequest bookCategoryRequest)
        {
            //gọi lại
            BaseResponse<BookCategoryResponse> response = await  _bookCategoryService.AddBookCategory(bookCategoryRequest);
            return response;
        }

        [HttpGet("GetAllBookCategory")]
        public async Task<BaseResponse<List<BookCategoryResponse>>> getAllBookCategory()
        {
            BaseResponse<List<BookCategoryResponse>> responses = await _bookCategoryService.GetAllBookCategory();
            return responses;
        }

        [HttpGet("GetBookCategoryById/{id}")]
        public async Task<BaseResponse<BookCategoryResponse>> getBookCategoryById(Guid id)
        {
            BaseResponse<BookCategoryResponse> response = await _bookCategoryService.GetBookCategoryById(id);
            return response;
        }

        [HttpPost("UpdateBookCategory/{id}")]
        public async Task<BaseResponse<BookCategoryResponse>> updateBookCategory(Guid id, BookCategoryRequest categoryRequest)
        {
            BaseResponse<BookCategoryResponse> response = await _bookCategoryService.UpdateBookCategory(id, categoryRequest);
            return response;
        }

        [HttpPost("DeleteBookCategory/{id}")]
        public async Task<BaseResponse<BookCategoryResponse>> deleteBookCategory(Guid id)
        {
            BaseResponse<BookCategoryResponse> response = await _bookCategoryService.DeleteBookCategory(id);
            return response;
        }
    }
}
