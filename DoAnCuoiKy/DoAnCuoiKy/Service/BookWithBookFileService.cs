using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service
{
    public class BookWithBookFileService : IBookWithBookFileService
    {
        private readonly ApplicationDbContext _context;
        public BookWithBookFileService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<List<BookWithBookFileResponse>>> GetAllBook()
        {
            BaseResponse<List<BookWithBookFileResponse>> response = new BaseResponse<List<BookWithBookFileResponse>>();
           
            List<BookWithBookFileResponse> bookWithBookFileResponses = await _context.bookFiles.Include(x=>x.book).ThenInclude(x=>x.Category).Include(x=>x.book).ThenInclude(x=>x.BookAuthor).Select(x=> new BookWithBookFileResponse
            {
                BookId = x.BookId,
                Title = x.book.Title,
                AuthorName = x.book.BookAuthor.Name,
                CategoryName = x.book.Category.Name,
                ImageUrl = x.ImageUrl,
                //PdfUrl = x.FileUrl,
                //BookFileType = x.bookFileType

            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy list thành công";
            response.data = bookWithBookFileResponses;
            return response;
        }
    }
}
