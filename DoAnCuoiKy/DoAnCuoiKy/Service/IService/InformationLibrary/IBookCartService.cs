using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookCartService
    {
        Task<BaseResponse<BookCartResponse>> CreateBookCart(BookCartRequest bookCartRequest);
        Task<BaseResponse<BookCartResponse>> GetBookCartActive();
        Task<BaseResponse<CheckoutBookCartResponse>> CheckoutBookCart(CheckoutBookCartResquest checkoutBookCartResquest);
    }
}
