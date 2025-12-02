using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IPaymentService
    {
        Task<BaseResponse<PaymentResponse>> CreatePayment(PaymentRequest paymentRequest);
        Task<BaseResponse<PaymentResponse>> VnPayReturn(IQueryCollection query);
        Task<BaseResponse<PaymentResponse>> CashPayment(PaymentRequest paymentRequest);
    }
}
