using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary.Kho
{
    public interface IShelfService
    {
        Task<BaseResponse<ShelfResponse>> AddShelf(ShelfRequest shelfRequest);
        Task<BaseResponse<List<ShelfResponse>>> GetAllShelf();
        Task<BaseResponse<ShelfResponse>> GetShelfById(Guid id);
        Task<BaseResponse<ShelfResponse>> UpdateShelf(Guid id, ShelfRequest shelfRequest);
        Task<BaseResponse<ShelfResponse>> DeleteShelf(Guid id);
    }
}
