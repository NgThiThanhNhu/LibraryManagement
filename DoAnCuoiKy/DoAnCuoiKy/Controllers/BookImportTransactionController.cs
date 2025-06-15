using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookImportTransactionController : ControllerBase
    {
        private readonly IBookImportTransactionService _transactionService;
        public BookImportTransactionController(IBookImportTransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost("AddBookImportTransaction")]
        public async Task<BaseResponse<BookImportTransactionResponse>> addBookImportTransaction(Guid BookId, BookImportTransactionRequest importTransactionRequest)
        {
            BaseResponse<BookImportTransactionResponse> baseResponse = await _transactionService.AddBookImportTransaction(BookId, importTransactionRequest);
            return baseResponse;
        }
        [HttpGet("GetAllBookImportTransaction")]
        public async Task<BaseResponse<List<BookImportTransactionResponse>>> getAllBookImportTransaction()
        {
            BaseResponse<List<BookImportTransactionResponse>> baseResponse = await _transactionService.GetAllBookImportTransaction();
            return baseResponse;
        }
        [HttpGet("GetBookImportTransactionById/{BookId}")]
        public async Task<BaseResponse<BookImportTransactionResponse>> getBookImportTransactionById(Guid BookId)
        {
            BaseResponse<BookImportTransactionResponse> baseResponse = await _transactionService.GetBookImportTransactionById(BookId);
            return baseResponse;
        }
    }
}
