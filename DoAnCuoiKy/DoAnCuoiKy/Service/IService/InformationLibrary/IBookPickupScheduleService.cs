using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookPickupScheduleService
    {
        Task<BaseResponse<BookPickupScheduleResponse>> CreateSchedule(BookPickupScheduleRequest bookPickupScheduleRequest);
        Task<BaseResponse<BookPickupScheduleResponse>> GetScheduledByBorrowingId(Guid borrowingId);
        Task<BaseResponse<BookPickupScheduleResponse>> DeleteScheduled(Guid borrowingId);

    }
}
