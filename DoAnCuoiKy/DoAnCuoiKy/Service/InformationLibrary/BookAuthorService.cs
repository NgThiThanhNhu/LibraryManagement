using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookAuthorService : IBookAuthorService
    {
        private readonly ApplicationDbContext _context;
        public BookAuthorService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<BookAuthorResponse>> AddBookAuthor(BookAuthorRequest authorRequest)
        {
            BaseResponse<BookAuthorResponse> response = new BaseResponse<BookAuthorResponse>();
            BookAuthor author = new BookAuthor();
            author.Id = Guid.NewGuid();
            author.Name = authorRequest.Name;
            await _context.bookAuthors.AddAsync(author);
            await _context.SaveChangesAsync();
            if (author == null)
            {
                response.IsSuccess = false;
                response.message = "Không có dữ liệu";
                return response;
            }
            BookAuthorResponse bookAuthorResponse = new BookAuthorResponse();
            bookAuthorResponse.Id = author.Id;
            bookAuthorResponse.Name = author.Name;
            response.IsSuccess = true;
            response.message = "Thêm dữ liệu thành công";
            response.data = bookAuthorResponse;
            return response;
        }
    }
}
