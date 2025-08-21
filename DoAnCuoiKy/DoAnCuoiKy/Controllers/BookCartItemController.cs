using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCartItemController : ControllerBase
    {
        private readonly IBookCartItemService _bookCartItemService;
        public BookCartItemController(IBookCartItemService bookCartItemService)
        {
            _bookCartItemService = bookCartItemService;
        }
        [HttpPost("addBookItemToCart")]
        public async Task<BaseResponse<BookCartItemResponse>> AddBookItemToCart(BookCartItemRequest bookCartItemRequest)
        {
            BaseResponse<BookCartItemResponse> baseResponse = await _bookCartItemService.AddBookItemToCart(bookCartItemRequest);
            return baseResponse;
        }
        [HttpGet("getAllBookCartOfUser")]
        public async Task<BaseResponse<List<BookCartItemResponse>>> GetAllBookCartOfUser()
        {
            BaseResponse<List<BookCartItemResponse>> baseResponse = await _bookCartItemService.GetAllBookCartOfUser();
            return baseResponse;
        }
        [HttpPost("deleteBookCartItem/{currentBookItemid}")]
        public async Task<BaseResponse<BookCartItemResponse>> DeleteBookCartItem(Guid currentBookItemid)
        {
            BaseResponse<BookCartItemResponse> baseResponse = await _bookCartItemService.DeleteBookCartItem(currentBookItemid);
            return baseResponse;
        }
    }
}
