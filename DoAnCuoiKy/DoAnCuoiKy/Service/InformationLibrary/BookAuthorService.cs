using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using DoAnCuoiKy.Utils;
using Microsoft.EntityFrameworkCore;

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
            BookAuthorResponse bookAuthorResponse = new BookAuthorResponse();
            BookAuthor findBookAuthor = _context.bookAuthors.FirstOrDefault(x => x.Name == authorRequest.Name);
            if (findBookAuthor != null)
            {
                response.IsSuccess = false;
                response.message = "Tên đã tồn tại, vui lòng nhập tên khác. Lưu ý không được trùng tên.";
                return response;
            }
            BookAuthor author = new BookAuthor();
            author.Id = Guid.NewGuid();
            author.Name = authorRequest.Name;
            await _context.bookAuthors.AddAsync(author);
            await _context.SaveChangesAsync();
            if (author.Name == null)
                return Global.getResponse(false, bookAuthorResponse, "Không có dữ liệu");

            bookAuthorResponse.Id = author.Id;
            bookAuthorResponse.Name = author.Name;
            return Global.getResponse(true, bookAuthorResponse, "Thêm dữ liệu thành công");
        }

        public async Task<BaseResponse<BookAuthorResponse>> DeleteBookAuthor(Guid id)
        {
            BaseResponse<BookAuthorResponse> response = new BaseResponse<BookAuthorResponse>();
            BookAuthorResponse bookAuthorResponse = new BookAuthorResponse();
            BookAuthor author = await _context.bookAuthors.FirstOrDefaultAsync(author => author.Id == id);
            if(author == null)
                return Global.getResponse(false, bookAuthorResponse, "Thêm dữ liệu thành công");
            author.IsDeleted = true;
            _context.bookAuthors.Update(author);
            await _context.SaveChangesAsync();
            return Global.getResponse(true, bookAuthorResponse, "Xóa thành công");
        }

        public async Task<BaseResponse<List<BookAuthorResponse>>> GetAllBookAuthor()
        {
            BaseResponse<List<BookAuthorResponse>> response = new BaseResponse<List<BookAuthorResponse>>();
            List<BookAuthorResponse> bookAuthorResponse = await _context.bookAuthors.Where(x => x.IsDeleted == false).Select(x => new BookAuthorResponse
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return Global.getResponse(true, bookAuthorResponse, "Lấy list thành công");
        }

        public async Task<BaseResponse<BookAuthorResponse>> GetBookAuthorById(Guid id)
        {
            BaseResponse<BookAuthorResponse> response = new BaseResponse<BookAuthorResponse>();
            BookAuthorResponse bookAuthorResponse = new BookAuthorResponse();
            BookAuthor bookAuthor = await _context.bookAuthors.FirstOrDefaultAsync(x => x.Id == id);
            if(bookAuthor == null)
                return Global.getResponse(false, bookAuthorResponse, "Không tìm thấy dữ liệu");
            
            bookAuthorResponse.Id = id;
            bookAuthorResponse.Name = bookAuthor.Name;
            return Global.getResponse(true, bookAuthorResponse, "Lấy dữ liệu thành công");
        }

        public async Task<BaseResponse<BookAuthorResponse>> UpdateBookAuthor(Guid id, BookAuthorRequest authorRequest)
        {
            BaseResponse<BookAuthorResponse> response = new BaseResponse<BookAuthorResponse>();
            BookAuthorResponse bookAuthorResponse = new BookAuthorResponse();
            BookAuthor bookAuthor = await _context.bookAuthors.FirstOrDefaultAsync(x => x.Id == id);
            if (bookAuthor == null)
                return Global.getResponse(false, bookAuthorResponse, "Không tồn tại dữ liệu");

            bookAuthor.Name = authorRequest.Name;
            _context.bookAuthors.Update(bookAuthor);
            await _context.SaveChangesAsync();
            bookAuthorResponse.Id = bookAuthor.Id;
            bookAuthorResponse.Name=bookAuthor.Name;
            return Global.getResponse(true, bookAuthorResponse, "Cập nhật dữ liệu thành công");
        }
    }
}
