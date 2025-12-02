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
        [HttpPut("{bookCartItemId}/quantity")]
        public async Task<BaseResponse<int>> UpdateQuantity(Guid bookCartItemId, UpdateQuantityRequest updateQuantityRequest)
        {
            BaseResponse<int> baseResponse = await _bookCartItemService.UpdateQuantity(bookCartItemId, updateQuantityRequest);
            return baseResponse;
        }
        [HttpDelete("bookCartItem/{bookCartItemId}")]
        public async Task<BaseResponse<bool>> RemoveItem(Guid bookCartItemId)
        {
            BaseResponse<bool> baseResponse = await _bookCartItemService.RemoveItem(bookCartItemId);
            return baseResponse;
        }
        [HttpDelete("clear/{bookCartId}")]
        public async Task<BaseResponse<bool>> ClearAllBookCartItem(Guid bookCartId) 
        {
            BaseResponse<bool> baseResponse = await _bookCartItemService.ClearAllBookCartItem(bookCartId);
            return baseResponse;
        }
    }
}
