using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController( IPaymentService paymentService, IConfiguration configuration)  
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-vnpayment")]
        public async Task<BaseResponse<PaymentResponse>> CreatePayment(PaymentRequest paymentRequest)
        {
            BaseResponse<PaymentResponse> baseResponse = await _paymentService.CreatePayment(paymentRequest);
            return baseResponse;
        }

        [HttpGet("ReturnUrl")] 
        public async Task<IActionResult> VnPayReturn()
        {
            try
            {
                var result = await _paymentService.VnPayReturn(Request.Query);

                if (result.IsSuccess)
                {
                    var url = $"https://localhost:5173/admin/ReturnUrl" +
                             $"?status=success" +
                             $"&borrowAmount={result.data?.BorrowAmount}" +
                             $"&txnRef={result.data?.TransactionNo}" +
                             $"&bankCode={result.data?.vnpBankCode}" +
                             $"&paymentType={result.data?.PaymentType}" + 
                             $"&vnp_ResponseCode={result.data?.VnpResponseCode}" + 
                             $"&vnpText={result.data?.VnpText}" + 
                             $"&createDate={result.data?.CreateDate}";

                    return Redirect(url);
                }
                else
                {
                    var url = $"https://localhost:5173/admin/ReturnUrl" +
                             $"?status=error" +
                             $"&message={Uri.EscapeDataString(result.message)}";

                    return Redirect(url);
                }
            }
            catch (Exception ex)
            {
                return Redirect($"https://localhost:5173/admin/ReturnUrl?status=error&message=System+error");
            }
        }
        [HttpPost("create-cashpayment")]
        public async Task<BaseResponse<PaymentResponse>> CashPayment(PaymentRequest paymentRequest)
        {
            BaseResponse<PaymentResponse> baseResponse = await _paymentService.CashPayment(paymentRequest);
            return baseResponse;
        }
    }
}