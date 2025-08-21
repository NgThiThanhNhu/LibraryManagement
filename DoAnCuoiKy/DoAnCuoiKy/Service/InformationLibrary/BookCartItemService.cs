using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookCartItemService : IBookCartItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public BookCartItemService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
       

        public async Task<BaseResponse<BookCartItemResponse>> AddBookItemToCart(BookCartItemRequest bookCartItemRequest)
        {
            BaseResponse<BookCartItemResponse> response = new BaseResponse<BookCartItemResponse>();
            BookCartItem bookCartItem = new BookCartItem();
            bookCartItem.Id = Guid.NewGuid();
            bookCartItem.UserId = getCurrentUserId();
            Librarian librarian = await _context.librarians.Where(x=>x.isValidate == true && x.Id == bookCartItem.UserId).FirstOrDefaultAsync();
            bookCartItem.BookItemId = bookCartItemRequest.BookItemId;
            bookCartItem.CreateDate = DateTime.Now;
            bookCartItem.CreateUser = librarian.Name;
            _context.bookCartItems.Add(bookCartItem);
            await _context.SaveChangesAsync();
            BookItem bookItem = await _context.bookItems.Include(x=>x.Book).ThenInclude(x=>x.BookAuthor).Include(x=>x.Book).ThenInclude(x=>x.Category).Where(x => x.IsDeleted == false && x.Id == bookCartItem.BookItemId).FirstOrDefaultAsync();
            BookCartItemResponse bookCartItemResponse = new BookCartItemResponse();
            bookCartItemResponse.Id = bookCartItem.Id;
            bookCartItemResponse.BookItemTitle = bookItem.Book.Title;
            bookCartItemResponse.BookItemAuthor = bookItem.Book.BookAuthor.Name;
            bookCartItemResponse.BookItemCategory = bookItem.Book.Category.Name;
            bookCartItemResponse.UserName = librarian.Name;
            response.IsSuccess = true;
            response.message = "Thêm sách vào giỏ hàng thành công";
            response.data = bookCartItemResponse;
            return response;

            
           

        }

        public async Task<BaseResponse<BookCartItemResponse>> DeleteBookCartItem(Guid currentBookItemId)
        {
            BaseResponse<BookCartItemResponse> response = new BaseResponse<BookCartItemResponse>();
            var currentUser = getCurrentUserId();
            BookCartItem bookCartItem = await _context.bookCartItems.Include(x=>x.BookItem).Where(x => x.IsDeleted == false && x.UserId == currentUser && x.BookItemId == currentBookItemId).FirstOrDefaultAsync();
            if(bookCartItem.BookItem.BookStatus == Model.Enum.InformationLibrary.BookStatus.Borrowed && bookCartItem != null)
            {
                bookCartItem.IsDeleted = true;

                _context.bookCartItems.Update(bookCartItem);
                await _context.SaveChangesAsync();
                
                response.IsSuccess = true;
                response.message = "Đã đặt sách thành công";
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.message = "Không tìm thấy sách cần xóa của user này";
                return response;
            }
        }

        public async Task<BaseResponse<List<BookCartItemResponse>>> GetAllBookCartOfUser()
        {
            BaseResponse<List<BookCartItemResponse>> response = new BaseResponse<List<BookCartItemResponse>>();
            var currentUserId = getCurrentUserId();
            List<BookCartItemResponse> bookCartItems = await _context.bookCartItems.Include(x=>x.BookItem.Book).ThenInclude(x=>x.BookAuthor).Include(x => x.BookItem.Book).ThenInclude(x=>x.Category).Where(x=>x.UserId == currentUserId && x.IsDeleted==false).Select(bookcartitem => new BookCartItemResponse{
                Id = bookcartitem.Id,
                BookItemId = bookcartitem.BookItemId,
                BookItemTitle = bookcartitem.BookItem.Book.Title,
                BookItemAuthor = bookcartitem.BookItem.Book.BookAuthor.Name,
                BookItemCategory = bookcartitem.BookItem.Book.Category.Name,
                UserName = bookcartitem.User.Name,

            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Đây là tất cả sách hiện có trong giỏ hàng của bạn";
            response.data = bookCartItems;
            return response;

        }

        private Guid getCurrentUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not Authentiated");
            }
            var userId = user.FindFirst(JwtRegisteredClaimNames.Sub) ?? user.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("userId không tồn tại");
            }
            return Guid.Parse(userId.Value);
        }
    }
}
