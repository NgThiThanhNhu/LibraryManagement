using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookCartItemService
    {
        Task<BaseResponse<BookCartItemResponse>> AddBookItemToCart(BookCartItemRequest bookCartItemRequest);
        Task<BaseResponse<List<BookCartItemResponse>>> GetAllBookCartOfUser();
        Task<BaseResponse<BookCartItemResponse>> DeleteBookCartItem(Guid currentBookItemid);
    }
}
