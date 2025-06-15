using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookImportTransactionService
    {
        Task<BaseResponse<BookImportTransactionResponse>> AddBookImportTransaction(Guid BookId,BookImportTransactionRequest importTransactionRequest);
        Task<BaseResponse<List<BookImportTransactionResponse>>> GetAllBookImportTransaction();
        Task<BaseResponse<BookImportTransactionResponse>> GetBookImportTransactionById(Guid BookId);
    }
}
