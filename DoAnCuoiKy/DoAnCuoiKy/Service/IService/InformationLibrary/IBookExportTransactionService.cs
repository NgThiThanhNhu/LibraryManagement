using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookExportTransactionService
    {
        Task<BaseResponse<BookExportTransactionResponse>> CreateBookExportTransaction(Guid BorrowingDetailId);
    }
}
