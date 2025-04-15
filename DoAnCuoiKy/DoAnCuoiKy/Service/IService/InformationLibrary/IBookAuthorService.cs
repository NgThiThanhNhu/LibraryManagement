using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookAuthorService
    {
        Task<BaseResponse<BookAuthorResponse>> AddBookAuthor(BookAuthorRequest authorRequest);
        
    }
}
