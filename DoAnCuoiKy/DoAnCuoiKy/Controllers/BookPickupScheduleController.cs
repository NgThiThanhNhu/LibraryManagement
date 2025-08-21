using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.InformationLibrary;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookPickupScheduleController : ControllerBase
    {
        private readonly IBookPickupScheduleService _bookPickupScheduleService;
        public BookPickupScheduleController(IBookPickupScheduleService bookPickupScheduleService)
        {
            _bookPickupScheduleService = bookPickupScheduleService;
        }
        [HttpPost("createSchedule")]
        public async Task<BaseResponse<BookPickupScheduleResponse>> CreateSchedule(BookPickupScheduleRequest bookPickupScheduleRequest)
        {
            BaseResponse<BookPickupScheduleResponse> response = await _bookPickupScheduleService.CreateSchedule(bookPickupScheduleRequest);
            return response;
        }
        [HttpGet("getScheduledByBorrowingId/{borrowingId}")]
        public async Task<BaseResponse<BookPickupScheduleResponse>> GetScheduledByBorrowingId(Guid borrowingId)
        {
            BaseResponse<BookPickupScheduleResponse> response = await _bookPickupScheduleService.GetScheduledByBorrowingId(borrowingId);
            return response;
        }
        [HttpPost("deleteScheduled/{borrowingId}")]
        public async Task<BaseResponse<BookPickupScheduleResponse>> DeleteScheduled(Guid borrowingId)
        {
            BaseResponse<BookPickupScheduleResponse> response = await _bookPickupScheduleService.DeleteScheduled(borrowingId);
            return response;
        }
    }
}
