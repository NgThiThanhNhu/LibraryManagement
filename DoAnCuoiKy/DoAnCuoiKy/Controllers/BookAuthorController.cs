using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    public class BookAuthorController : ControllerBase
    {
        private readonly IBookAuthorService _bookAuthorService;
        public BookAuthorController(IBookAuthorService bookAuthorService)
        {
            _bookAuthorService = bookAuthorService;
        }
        public async Task<BaseResponse<BookAuthorResponse>> addBookAuthor(BookAuthorRequest authorRequest)
        {
            BaseResponse<BookAuthorResponse> response = await _bookAuthorService.AddBookAuthor(authorRequest);
            return response;
        }
    }
}
