using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;

namespace DoAnCuoiKy.Service.IService.InformationLibrary.Kho
{
    public interface IShelfSectionService
    {
        Task<BaseResponse<ShelfSectionResponse>> AddShelfSection(ShelfSectionRequest shelfSectionRequest);
        Task<BaseResponse<List<ShelfSectionResponse>>> GetAllShelfSection();
        Task<BaseResponse<ShelfSectionResponse>> GetShelfSectionById(Guid id);
        Task<BaseResponse<ShelfSectionResponse>> UpdateShelfSection(Guid id, ShelfSectionRequest shelfSectionRequest);
        Task<BaseResponse<ShelfSectionResponse>> DeleteShelfSection(Guid id);
    }
}
