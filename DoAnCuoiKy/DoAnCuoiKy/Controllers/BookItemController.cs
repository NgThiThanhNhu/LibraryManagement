using Azure;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
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
        public async Task<BaseResponse<List<BookItemResponse>>> addBookItem(BookItemRequest bookItemRequest)
        {
            BaseResponse<List<BookItemResponse>> response = await _bookItemService.AddBookItem(bookItemRequest);
            return response;
        }

        [HttpPost("UpdateBookItem/{id}")]
        public async Task<BaseResponse<BookItemResponse>> updateBookItem(Guid id, BookItemRequest bookItemRequest)
        {
            BaseResponse<BookItemResponse> response = await _bookItemService.UpdateBookItem(id, bookItemRequest);
            return response;
        }
        [HttpGet("GetAllBookItem")]
        public async Task<BaseResponse<List<BookItemResponse>>> getAllBookItem()
        {
            BaseResponse<List<BookItemResponse>> response = await _bookItemService.GetAllBookItem();
            return response;
        }
        [HttpGet("GetBookItemById/{id}")]
        public async Task<BaseResponse<BookItemResponse>> getBookItemById(Guid id) 
        { 
            BaseResponse<BookItemResponse> response = await _bookItemService.GetBookItemById(id);
            return response;
        }
        [HttpPost("DeleteBookItem/{id}")]
        public async Task<BaseResponse<BookItemResponse>> deleteBookItem(Guid id)
        {
            BaseResponse<BookItemResponse> response = await _bookItemService.DeleteBookItem(id);
            return response;
        }
    }
}
