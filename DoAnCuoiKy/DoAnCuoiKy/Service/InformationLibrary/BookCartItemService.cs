using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookCartItemService : IBookCartItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookCartItemService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<bool>> RemoveItem(Guid bookCartItemId)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                var currentUserId = getCurrentUserId();
                BookCartItem findBookCartItem = await _context.bookCartItems.Include(x => x.BookCart).Include(x=>x.Book).FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == bookCartItemId);
                if (findBookCartItem == null || findBookCartItem.BookCart.UserId != currentUserId)
                {
                    response.IsSuccess = false;
                    response.message = "Không tìm thấy sách";
                    response.data = false;
                    return response;
                }
                var bookTitle = findBookCartItem.Book.Title;
                _context.bookCartItems.Remove(findBookCartItem);
                findBookCartItem.BookCart.UpdateDate = DateTime.Now;
                findBookCartItem.BookCart.UpdateUser = getCurrentName();
                _context.bookCarts.Update(findBookCartItem.BookCart);
                await _context.SaveChangesAsync();
                response.IsSuccess = true;
                response.message = $"Xóa sách {bookTitle} khỏi giỏ thành công";
                response.data = true;
                return response;
            }catch (Exception ex)
            {
                response.IsSuccess= false;
                response.message = ex.Message;
                response.data = false;
                return response;
            }
        }

        public async Task<BaseResponse<int>> UpdateQuantity(Guid bookCartItemId, UpdateQuantityRequest updateQuantityRequest)
        {
            BaseResponse<int> response = new BaseResponse<int>();
            try
            {
                var currentUserId = getCurrentUserId();
                BookCartItem findCartItem = await _context.bookCartItems.Include(x => x.BookCart).Include(x => x.Book).FirstOrDefaultAsync(x => x.Id == bookCartItemId && x.IsDeleted == false && x.BookCart.UserId == currentUserId);
                if (findCartItem == null)
                {
                    response.IsSuccess = false;
                    response.message = "Không tìm thấy sách trong giỏ";
                    return response;
                }
                int newQuantity = findCartItem.Quantity;
                if (updateQuantityRequest.Action == "increase")
                {
                    newQuantity++;
                }
                else if (updateQuantityRequest.Action == "decrease")
                {
                    newQuantity--;
                }
                else
                {
                    response.IsSuccess = false;
                    response.message = "Action không hợp lệ";
                    return response;
                }
                if (newQuantity < 1)
                {
                    response.IsSuccess = false;
                    response.message = "Số lượng không được nhỏ hơn 1";
                    return response;
                }
                if (newQuantity > 5)
                {
                    response.IsSuccess = false;
                    response.message = "Không mượn quá 5 cuốn";
                    return response;
                }
                int checkOrderBookCartItem = await _context.bookCartItems.Where(x => x.CartId == findCartItem.CartId && x.Id != findCartItem.Id && x.IsDeleted == false).SumAsync(x => x.Quantity);
                if (checkOrderBookCartItem + newQuantity > 5)
                {
                    response.IsSuccess = false;
                    response.message = "Số lượng sách trong giỏ không được nhiều hơn 5 cuốn";
                    return response;
                }
                if (newQuantity > findCartItem.Quantity)
                {
                    int availableBookItemCount = await _context.bookItems.CountAsync(x => x.IsDeleted == false && x.BookId == findCartItem.BookId && x.BookStatus == Model.Enum.InformationLibrary.BookStatus.Available);
                    if (availableBookItemCount < newQuantity)
                    {
                        response.IsSuccess = false;
                        response.message = "Không đủ sách";
                        return response;
                    }
                }
                
                findCartItem.Quantity = newQuantity;
                findCartItem.UpdateDate = DateTime.Now;
                findCartItem.UpdateUser = getCurrentName();
                _context.bookCartItems.Update(findCartItem);
                await _context.SaveChangesAsync();
                response.IsSuccess = true;
                response.message = "Thêm sách thành công";
                response.data = newQuantity;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess=false;
                response.message = ex.Message;
                return response;
            }
        }
        private string getCurrentName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
        private Guid getCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext.User;
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
        public async Task<BaseResponse<bool>> ClearAllBookCartItem(Guid bookCartId)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                var currentUserId = getCurrentUserId();
                BookCart findBookCart = await _context.bookCarts.Include(x => x.BookCartItems).FirstOrDefaultAsync(x => x.IsDeleted == false && x.UserId == currentUserId && x.Id == bookCartId && x.CartStatus == CartStatus.Active);
                if (findBookCart == null || !findBookCart.BookCartItems.Any())
                {
                    response.IsSuccess = false;
                    response.message = "Không tìm thấy giỏ sách muốn xóa";
                    return response;
                }
                findBookCart.BookCartItems.Clear();
                findBookCart.UpdateDate = DateTime.Now;
                findBookCart.UpdateUser = getCurrentName();
                _context.bookCarts.Update(findBookCart);
                await _context.SaveChangesAsync();
                response.IsSuccess = true;
                response.message = "Xóa giỏ sách thành công";
                response.data = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.message = ex.Message;
                response.data = false;
                return response;
            }
        }
    }
}
