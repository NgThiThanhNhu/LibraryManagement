using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IBookItemService
    {
        Task<BaseResponse<List<BookItemResponse>>> AddBookItem(BookItemRequest bookItemRequest);
        Task<BaseResponse<BookItemResponse>> UpdateBookItemStatus(Guid id, BookItemRequest bookItemRequest);
        Task<BaseResponse<List<BookItemResponse>>> GetAllBookItem();
        Task<BaseResponse<BookItemResponse>> ChooseBookItemByBookId(Guid bookId);
        Task<BaseResponse<BookItemResponse>> DeleteBookItem(Guid id);
    }
}
