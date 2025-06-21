using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookWithBookFileController : ControllerBase
    {
        private readonly IBookWithBookFileService _bookWithBookFileService;
        public BookWithBookFileController(IBookWithBookFileService bookWithBookFileService)
        {
            _bookWithBookFileService = bookWithBookFileService;
        }
        [HttpGet("GetAllBook")]
        public async Task<BaseResponse<List<BookWithBookFileResponse>>> getAllBook()
        {
            BaseResponse<List<BookWithBookFileResponse>> baseResponse = await _bookWithBookFileService.GetAllBook();
            return baseResponse;
        }
    }
}
