using System;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCartController : ControllerBase
    {
        private readonly IBookCartService _bookCartService;
        public BookCartController(IBookCartService bookCartService)
        {
            _bookCartService = bookCartService;
        }
        [HttpPost("createCart")]
        public async Task<BaseResponse<BookCartResponse>> createBookCart(BookCartRequest bookCartRequest)
        {
            BaseResponse<BookCartResponse> baseResponse = await _bookCartService.CreateBookCart(bookCartRequest);
            return baseResponse;
        }
        [HttpGet("GetBookCartActive")]
        public async Task<BaseResponse<BookCartResponse>> getBookCartActive()
        {
            BaseResponse<BookCartResponse> baseResponse = await _bookCartService.GetBookCartActive();
            return baseResponse;
        }
        [HttpPost("checkout")]
        public async Task<BaseResponse<CheckoutBookCartResponse>> CheckoutBookCart(CheckoutBookCartResquest checkoutBookCartResquest)
        {
            BaseResponse<CheckoutBookCartResponse> baseResponse = await _bookCartService.CheckoutBookCart(checkoutBookCartResquest); ;
            return baseResponse;
        }
    }
}
