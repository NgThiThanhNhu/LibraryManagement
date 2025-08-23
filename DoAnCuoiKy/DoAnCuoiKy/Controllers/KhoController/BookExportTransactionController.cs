using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers.KhoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookExportTransactionController : ControllerBase
    {
        private readonly IBookExportTransactionService _bookExportTransactionService;
        public BookExportTransactionController(IBookExportTransactionService bookExportTransactionService)
        {
            _bookExportTransactionService = bookExportTransactionService;
        }
        [HttpPost("createBookExportTransaction")]
        public async Task<BaseResponse<BookExportTransactionResponse>> CreateBookExportTransaction(Guid BorrowingDetailId)
        {
            BaseResponse<BookExportTransactionResponse> response = await _bookExportTransactionService.CreateBookExportTransaction(BorrowingDetailId);
            return response;
        }
    }
}
