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

        public Task<BaseResponse<BookAuthorResponse>> DeleteBookAuthor(Guid Id)
        {
            throw new NotImplementedException();
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

        public Task<BaseResponse<BookAuthorResponse>> GetBookAuthorById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<BookAuthorResponse>> UpdateBookAuthor(Guid Id, BookAuthorRequest authorRequest)
        {
            throw new NotImplementedException();
        }
    }
}
