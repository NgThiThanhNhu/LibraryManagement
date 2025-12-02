using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookCartItemService
    {
        Task<BaseResponse<int>> UpdateQuantity(Guid bookCartItemId, UpdateQuantityRequest updateQuantityRequest);
        Task<BaseResponse<bool>> RemoveItem(Guid bookCartItemId);
        Task<BaseResponse<bool>> ClearAllBookCartItem(Guid bookCartId);

    }
}
