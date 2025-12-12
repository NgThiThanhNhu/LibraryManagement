using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.UserBehavior;
using DoAnCuoiKy.Model.Request.UserBookInteractionRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.UserBehaviorRequest;
using DoAnCuoiKy.Service.IService.IUserBehavior;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.UserBehaviorService
{
    public class UserBookInteractionService : IUserBookInteractionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserBookInteractionService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _contextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<UserBookInteractionResponse>> CreateRating(UserBookInteractionRequest userBookInteractionRequest)
        {
            BaseResponse<UserBookInteractionResponse> response = new BaseResponse<UserBookInteractionResponse>();
            Book bookWantRate = await _context.books.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == userBookInteractionRequest.BookId);
            if (bookWantRate == null)
            {
                response.IsSuccess = false;
                response.message = "Sách không còn tồn tại nữa!";
                return response;
            }
            Borrowing existed = await _context.borrowings.FirstOrDefaultAsync(x => x.IsDeleted == false && x.BorrowingStatus == Model.Enum.InformationLibrary.BorrowingStatus.Returned && x.UserId == getCurrentUserId());
            if (existed == null)
            {
                response.IsSuccess = false;
                response.message = "Người này chưa mượn sách, không thể đánh giá";
                return response;
            }
            if (userBookInteractionRequest.Rating > 5 || userBookInteractionRequest.Rating < 0)
            {
                response.IsSuccess = false;
                response.message = "Chỉ đánh giá từ 1 đến 5 sao";
                return response;
            }
            UserBookInteraction newInteraction = new UserBookInteraction();
            newInteraction.Id = Guid.NewGuid();
            newInteraction.CreateDate = DateTime.Now;
            newInteraction.UserId = getCurrentUserId();
            newInteraction.BookId = userBookInteractionRequest.BookId;
            newInteraction.InteractionType = userBookInteractionRequest.InteractionType;
            newInteraction.Rating = userBookInteractionRequest.Rating;
            newInteraction.ReviewText = userBookInteractionRequest.ReviewText;
            _context.userBookInteractions.Add(newInteraction);
            await _context.SaveChangesAsync();
            UserBookInteractionResponse userBookInteractionResponse = new UserBookInteractionResponse();
            userBookInteractionResponse.Id = newInteraction.Id;
            userBookInteractionResponse.UserName = getCurrentName();
            userBookInteractionResponse.BookTitle = bookWantRate.Title;
            userBookInteractionResponse.CreateAt = newInteraction.CreateDate;
            userBookInteractionResponse.Rating = newInteraction.Rating;
            userBookInteractionResponse.ReviewText = newInteraction.ReviewText;
            response.IsSuccess = true;
            response.message = "Đánh giá thành công";
            response.data = userBookInteractionResponse;
            return response;
        }

        public async Task<BaseResponse<List<UserBookInteractionResponse>>> GetAllRating(Guid BookId)
        {
            BaseResponse<List<UserBookInteractionResponse>> response = new BaseResponse<List<UserBookInteractionResponse>>();
            List<UserBookInteractionResponse> userBookInteractionResponses = await _context.userBookInteractions.Include(x => x.Librarian).Include(x => x.Book).Where(x=>x.IsDeleted == false && x.BookId == BookId).Select(x => new UserBookInteractionResponse
            {
                Id = x.Id,
                CreateAt = x.CreateDate,
                Rating = x.Rating,
                ReviewText = x.ReviewText,
                UserName = x.Librarian.Name,
                BookTitle = x.Book.Title,
            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy danh sách bình luận thành công";
            response.data = userBookInteractionResponses;
            return response;
        }
        private string getCurrentName()
        {
            return _contextAccessor.HttpContext.User.Identity.Name;
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
