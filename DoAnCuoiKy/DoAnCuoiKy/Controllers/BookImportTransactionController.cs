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
        public async Task<BaseResponse<BookImportTransactionResponse>> addBookImportTransaction(BookImportTransactionRequest importTransactionRequest)
        {
            BaseResponse<BookImportTransactionResponse> baseResponse = await _transactionService.AddBookImportTransaction(importTransactionRequest);
            return baseResponse;
        }
    }
}
