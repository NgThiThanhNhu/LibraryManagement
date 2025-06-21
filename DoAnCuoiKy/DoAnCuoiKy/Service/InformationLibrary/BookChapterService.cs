using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookChapterService : IBookChapterService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public BookChapterService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public async Task<BaseResponse<BookChapterResponse>> AddBookChapter(BookChapterRequest chapterRequest)
        {
            BaseResponse<BookChapterResponse> response = new BaseResponse<BookChapterResponse>();
            BookChapterResponse chapterResponse = new BookChapterResponse();
            BookChapter bookChapter = new BookChapter();
            bookChapter.TitleChapter = chapterRequest.TitleChapter;
            bookChapter.CreateUser = getCurrentName();
            bookChapter.CreateDate = DateTime.Now;
            _context.bookChapters.AddAsync(bookChapter);
            await _context.SaveChangesAsync();
            if(bookChapter.TitleChapter == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm thất bại";
                return response;
            }
            chapterResponse.TitleChapter = bookChapter.TitleChapter;

            response.IsSuccess = true;
            response.message = "Thêm thành công";
            response.data = chapterResponse;
            return response;
        }

        public async Task<BaseResponse<BookChapterResponse>> DeleteBookChapter(Guid id)
        {
            BaseResponse<BookChapterResponse> response = new BaseResponse<BookChapterResponse>();
            BookChapter bookChapter =await _context.bookChapters.FindAsync(id);
            if(bookChapter !=null && bookChapter.IsDeleted == false)
            {
                bookChapter.IsDeleted = true;
                bookChapter.deleteUser = getCurrentName();
                bookChapter.UpdateDate = DateTime.Now;
                _context.bookChapters.Update(bookChapter);
                await _context.SaveChangesAsync();
                response.IsSuccess = true;
                response.message = "Xóa dữ liệu thành công";
                //đổi isdelete xong thì cần phải cập nhật giá trị vào table thì mới trả về kết quả

            }
            else
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại dữ liệu cần xóa";
            }
            return response;
        }

        public async Task<BaseResponse<List<BookChapterResponse>>> GetAllBookChapter()
        {
            BaseResponse<List<BookChapterResponse>> response = new BaseResponse<List<BookChapterResponse>>();
            List<BookChapterResponse> bookChapterResponses = new List<BookChapterResponse>();
            List<BookChapter> chapters = await _context.bookChapters.Where(x=>x.IsDeleted == false).ToListAsync();
            foreach(var chapter in chapters)
            {
                BookChapterResponse bookChapterResponse = new BookChapterResponse();
                bookChapterResponse.Id = chapter.Id;
                bookChapterResponse.TitleChapter = chapter.TitleChapter;
                bookChapterResponses.Add(bookChapterResponse);
            }
            if(bookChapterResponses == null)
            {
                response.IsSuccess = false;
                response.message = "không tồn tại dữ liệu để hiển thị";
            }
            response.IsSuccess = true;
            response.message = "Hiển thị dữ liệu thành công";
            response.data = bookChapterResponses;
            return response;
        }

        public async Task<BaseResponse<BookChapterResponse>> GetBookChapterById(Guid id)
        {
            BaseResponse<BookChapterResponse> response = new BaseResponse<BookChapterResponse>();
            BookChapterResponse bookChapterResponse = new BookChapterResponse();
            BookChapter bookChapter = await _context.bookChapters.FindAsync(id);
            if(bookChapter == null && bookChapter.IsDeleted == true)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            bookChapterResponse.Id = bookChapter.Id;
            bookChapterResponse.TitleChapter = bookChapter.TitleChapter;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = bookChapterResponse;
            return response;
        }

        public async Task<BaseResponse<BookChapterResponse>> UpdateBookChapter(Guid id, BookChapterRequest chapterRequest)
        {
            BaseResponse<BookChapterResponse> response = new BaseResponse<BookChapterResponse>();
            BookChapterResponse bookChapterResponse = new BookChapterResponse();
            BookChapter bookChapter = await _context.bookChapters.FindAsync(id);
            _context.Entry(bookChapter).State = EntityState.Detached; //xóa đối tượng khỏi tracking trước khi cập nhật
            if (bookChapter == null)
            {
                response.IsSuccess = false;
                response.message = "Không tìm thấy chapter";
                return response;
            }
            //BookChapter chapter = new BookChapter();
            bookChapter.TitleChapter = chapterRequest.TitleChapter;
            bookChapter.UpdateUser = getCurrentName();
            bookChapter.UpdateDate = DateTime.Now;

                _context.bookChapters.UpdateRange(bookChapter);
                await _context.SaveChangesAsync();
            //lưu xong rồi trả ra kết quả cho người biết:)
                //bookChapterResponse.Id = chapter.Id;
                bookChapterResponse.TitleChapter = bookChapter.TitleChapter;
                response.IsSuccess = true;
                response.message = "update thành công";
                response.data= bookChapterResponse; 
                return response;
        }
        private string getCurrentName()
        {
            return _contextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
