using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
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
            BookAuthor author = new BookAuthor();
            author.Id = Guid.NewGuid();
            author.Name = authorRequest.Name;
            await _context.bookAuthors.AddAsync(author);
            await _context.SaveChangesAsync();
            if (author.Name == null)
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

        public async Task<BaseResponse<BookAuthorResponse>> DeleteBookAuthor(Guid id)
        {
            BaseResponse<BookAuthorResponse> response = new BaseResponse<BookAuthorResponse>();
            BookAuthor author = await _context.bookAuthors.FirstOrDefaultAsync(author => author.Id == id);
            if(author == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại dữ liệu";
                return response;
            }
            author.IsDeleted = true;
            _context.bookAuthors.Update(author);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa thành công";
            return response;
        }

        public async Task<BaseResponse<List<BookAuthorResponse>>> GetAllBookAuthor()
        {
            BaseResponse<List<BookAuthorResponse>> response = new BaseResponse<List<BookAuthorResponse>>();
            List<BookAuthorResponse> bookAuthorResponse = await _context.bookAuthors.Where(x => x.IsDeleted == false).Select(x => new BookAuthorResponse
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy list thành công";
            response.data = bookAuthorResponse;
            return response;
        }

        public async Task<BaseResponse<BookAuthorResponse>> GetBookAuthorById(Guid id)
        {
            BaseResponse<BookAuthorResponse> response = new BaseResponse<BookAuthorResponse>();
            BookAuthor bookAuthor = await _context.bookAuthors.FirstOrDefaultAsync(x => x.Id == id);
            if(bookAuthor == null)
            {
                response.IsSuccess = false;
                response.message = "Không tìm thấy dữ liệu";
                return response;
            }
            BookAuthorResponse bookAuthorResponse = new BookAuthorResponse();
            bookAuthorResponse.Id = id;
            bookAuthorResponse.Name = bookAuthor.Name;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = bookAuthorResponse;
            return response;
        }

        public async Task<BaseResponse<BookAuthorResponse>> UpdateBookAuthor(Guid id, BookAuthorRequest authorRequest)
        {
            BaseResponse<BookAuthorResponse> response = new BaseResponse<BookAuthorResponse>();
            BookAuthor bookAuthor = await _context.bookAuthors.FirstOrDefaultAsync(x => x.Id == id);
            if (bookAuthor == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại dữ liệu";
                return response;
            }
            bookAuthor.Name = authorRequest.Name;
            _context.bookAuthors.Update(bookAuthor);
            await _context.SaveChangesAsync();
            BookAuthorResponse bookAuthorResponse = new BookAuthorResponse();
            bookAuthorResponse.Id = bookAuthor.Id;
            bookAuthorResponse.Name=bookAuthor.Name;
            response.IsSuccess = true;
            response.message = "Cập nhật dữ liệu thành công";
            response.data= bookAuthorResponse;
            return response;
        }
    }
}
