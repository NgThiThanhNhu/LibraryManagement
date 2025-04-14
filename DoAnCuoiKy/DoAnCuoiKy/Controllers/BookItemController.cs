using Azure;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookItemController : ControllerBase
    {
        private readonly IBookItemService _bookItemService;
        public BookItemController(IBookItemService bookItemService)
        {
            _bookItemService = bookItemService;
        }
        [HttpPost("AddBookItem")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponse<List<BookItemResponse>>> addBookItem(BookItemRequest bookItemRequest)
        {
            BaseResponse<List<BookItemResponse>> response = await _bookItemService.AddBookItem(bookItemRequest);
            return response;
        }

        [HttpPost("UpdateBookItem/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponse<BookItemResponse>> updateBookItemStatus(Guid id, BookItemRequest bookItemRequest)
        {
            BaseResponse<BookItemResponse> response = await _bookItemService.UpdateBookItemStatus(id, bookItemRequest);
            return response;
        }
        [HttpGet("GetAllBookItem")]
        [Authorize(Roles = "Admin, User")]
        public async Task<BaseResponse<List<BookItemResponse>>> getAllBookItem()
        {
            BaseResponse<List<BookItemResponse>> response = await _bookItemService.GetAllBookItem();
            return response;
        }
        [HttpGet("GetBookItemById/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<BaseResponse<BookItemResponse>> getBookItemById(Guid id)
        {
            BaseResponse<BookItemResponse> response = await _bookItemService.GetBookItemById(id);
            return response;
        }
        [HttpPost("DeleteBookItem/{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<BaseResponse<BookItemResponse>> deleteBookItem(Guid id)
        {
            BaseResponse<BookItemResponse> response = await _bookItemService.DeleteBookItem(id);
            return response;
        }
    }
}
