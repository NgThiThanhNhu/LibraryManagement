using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DoAnCuoiKy.Service.InformationLibrary
{
  
    public class BookCategoryService : IBookCategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public BookCategoryService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public async Task<BaseResponse<BookCategoryResponse>> AddBookCategory(BookCategoryRequest categoryRequest)
        {
            BaseResponse<BookCategoryResponse> response = new BaseResponse<BookCategoryResponse>();
            if (categoryRequest.Name == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm không thành công. Tên sách không được để trống";
                return response;
            }
            BookCategory bookCategory = new BookCategory();
            bookCategory.Id = Guid.NewGuid();
            bookCategory.Name = categoryRequest.Name;
            bookCategory.CreateUser = getCurrentName();
            bookCategory.UpdateUser = getCurrentName();
            bookCategory.UpdateDate = DateTime.Now;
            bookCategory.CreateDate = DateTime.Now;
   
            await _context.bookCategories.AddAsync(bookCategory);
            await _context.SaveChangesAsync();

            if(bookCategory == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm không thành công";
               
            }
            response.IsSuccess = true;
            response.message = "Thêm thành công";

            BookCategoryResponse responseData = new BookCategoryResponse();
            responseData.Name = bookCategory.Name;
            response.data = responseData;
            return response;
        }

        public async Task<BaseResponse<BookCategoryResponse>> DeleteBookCategory(Guid id)
        {
            BaseResponse<BookCategoryResponse> baseResponse = new BaseResponse<BookCategoryResponse>();
            BookCategory deleteCategory = await _context.bookCategories.FindAsync(id);
            if(deleteCategory == null)
            {
                baseResponse.IsSuccess = false;
                baseResponse.message = "Không tồn tại loại sách";
                return baseResponse;
            }
            deleteCategory.IsDeleted = true;
            deleteCategory.deleteUser = getCurrentName();
            deleteCategory.DeleteDate = DateTime.Now;
            _context.bookCategories.Update(deleteCategory);
            await _context.SaveChangesAsync();
            baseResponse.IsSuccess = true;
            baseResponse.message = "xóa thành công";
            return baseResponse;
        }

        public async Task<BaseResponse<List<BookCategoryResponse>>> GetAllBookCategory()
        {
            BaseResponse<List<BookCategoryResponse>> responses = new BaseResponse<List<BookCategoryResponse>>();
     
            List<BookCategoryResponse> bookCategoryResponses = new List<BookCategoryResponse>();
            List<BookCategory> bookCategories = await _context.bookCategories.Where(x=>x.IsDeleted == false).ToListAsync();
            foreach(var category in bookCategories)
            {
                BookCategoryResponse bookCategory = new BookCategoryResponse();
                bookCategory.Id = category.Id.Value;
                bookCategory.Name = category.Name;
                bookCategoryResponses.Add(bookCategory);
            }
            if(bookCategoryResponses == null)
            {
                responses.IsSuccess = false;
                responses.message = "Lấy danh sách thất bại";
            }
            responses.IsSuccess = true;
            responses.message = "Lấy danh sách thành công";
            responses.data = bookCategoryResponses;
            return responses;
            
        }

        public async Task<BaseResponse<BookCategoryResponse>> GetBookCategoryById(Guid id)
        {
            BaseResponse<BookCategoryResponse> baseResponse = new BaseResponse<BookCategoryResponse>();
            BookCategoryResponse categoryResponse = new BookCategoryResponse();
            BookCategory bookCategory = await _context.bookCategories.FindAsync(id);
            if (bookCategory != null && bookCategory.IsDeleted == false)
            {
                categoryResponse.Id = bookCategory.Id.Value;
                categoryResponse.Name = bookCategory.Name;
                baseResponse.IsSuccess = true;
                baseResponse.message = "Lấy thành công";
                baseResponse.data = categoryResponse;
            }
            else
            {
                baseResponse.IsSuccess = false;
                baseResponse.message = "Lấy thất bại";
                baseResponse.data = categoryResponse;
            }
            return baseResponse;
            
        }

        public async Task<BaseResponse<BookCategoryResponse>> UpdateBookCategory(Guid id,BookCategoryRequest categoryRequest)
        {
            BaseResponse<BookCategoryResponse> response = new BaseResponse<BookCategoryResponse>();
            BookCategoryResponse bookCategoryResponse = new BookCategoryResponse();
            //BookCategory category = new BookCategory();??? này là tạo một đối tượng mới, chứ không phải cập nhật
            //Guid rId = Guid.Parse(id);
            BookCategory bookCategory = await _context.bookCategories.FindAsync(id);
            if(bookCategory == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại loại sách cần tìm";
                return response;
            }
            bookCategory.Name = categoryRequest.Name;
            bookCategory.UpdateDate = DateTime.Now;
            bookCategory.UpdateUser = "Nguyen Thi Thanh Nhu";
            
            _context.bookCategories.UpdateRange(bookCategory);
            await _context.SaveChangesAsync();
            bookCategoryResponse.Name = bookCategory.Name;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = bookCategoryResponse;
            return response;
        }
        private string getCurrentName()
        {
            return _contextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
