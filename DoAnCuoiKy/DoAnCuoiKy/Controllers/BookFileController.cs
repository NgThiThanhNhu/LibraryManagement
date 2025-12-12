using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service;
using DoAnCuoiKy.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookFileController : ControllerBase
    {
        private readonly IBookFileService _bookFileService;
        public BookFileController(IBookFileService bookFileService)
        {
            _bookFileService = bookFileService;
        }
        [HttpPost("UploadFileAndImage")]
        public async Task<BaseResponse<BookFileResponse>> uploadFileAndImage( [FromForm] BookFileRequest bookFileRequest)
        {
            BaseResponse<BookFileResponse> baseResponse = await _bookFileService.UploadFileAndImage(bookFileRequest);
            return baseResponse;
        }
        [HttpGet("pdf/{bookFileId}")]
        public async Task<BaseResponse<ReadFileResponse>> GetPdfFile(Guid bookFileId)
        {
            BaseResponse < ReadFileResponse > response = await _bookFileService.GetPdfFileStream(bookFileId);
            return response;
        }
    }
}
