using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookImportTransactionService
    {
        Task<BaseResponse<BookImportTransactionResponse>> AddBookImportTransaction(BookImportTransactionRequest importTransactionRequest);
    }
}
