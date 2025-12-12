using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Azure;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.UserBehavior;
using DoAnCuoiKy.Model.Request.UserBookInteractionRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.UserBehaviorRequest;
using DoAnCuoiKy.Model.Response.UserBehaviorResponse;
using DoAnCuoiKy.Service.IService.IUserBehavior;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.UserBehaviorService
{
    public class UserReadingSessionService : IUserReadingSessionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserReadingSessionService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<UserReadingSessionResponse>> CreateReadingSession(UserReadingSessionRequest readingSessionRequest)
        {
            BaseResponse<UserReadingSessionResponse> response = new BaseResponse<UserReadingSessionResponse>();
            try
            {
                var currentUserId = getCurrentUserId();
                UserReadingSession sessionExisted = await _context.userReadingSessions.FirstOrDefaultAsync(x => x.IsDeleted == false && x.UserId == getCurrentUserId() && x.BookId == readingSessionRequest.BookId);
                if (sessionExisted == null)
                {
                    if (readingSessionRequest.LastPageNumber < 1 || readingSessionRequest.TotalPages < 1)
                    {
                        response.IsSuccess = false;
                        response.message = "Số trang không được bé hơn 1 trang";
                        return response;
                    }
                    if (readingSessionRequest.LastPageNumber > readingSessionRequest.TotalPages)
                    {
                        response.IsSuccess = false;
                        response.message = "Số trang đã đọc không lớn hơn tổng trang sách đang có";
                        return response;
                    }
                    UserReadingSession newUserReadingSession = new UserReadingSession();
                    newUserReadingSession.Id = Guid.NewGuid();
                    newUserReadingSession.UserId = currentUserId;
                    newUserReadingSession.BookId = readingSessionRequest.BookId;
                    newUserReadingSession.StartTime = DateTime.Now;
                    newUserReadingSession.DurationSeconds = 0;
                    newUserReadingSession.ReadingProgress = 0;
                    newUserReadingSession.LastPageNumber = readingSessionRequest.LastPageNumber;
                    newUserReadingSession.TotalPages = readingSessionRequest.TotalPages;
                    newUserReadingSession.CreateDate = DateTime.Now;
                    newUserReadingSession.CreateUser = getCurrentName();
                    await _context.userReadingSessions.AddAsync(newUserReadingSession);
                    await _context.SaveChangesAsync();

                    UserReadingSessionResponse userReadingSessionResponse = new UserReadingSessionResponse();
                    userReadingSessionResponse.Id = newUserReadingSession.Id;
                    userReadingSessionResponse.UserId = newUserReadingSession.UserId;
                    userReadingSessionResponse.BookId = newUserReadingSession.BookId;
                    userReadingSessionResponse.StartTime = newUserReadingSession.StartTime;
                    userReadingSessionResponse.LastPageNumber = newUserReadingSession.LastPageNumber;
                    userReadingSessionResponse.TotalPages = newUserReadingSession.TotalPages;
                    userReadingSessionResponse.ReadingProgress = newUserReadingSession.ReadingProgress;
                    userReadingSessionResponse.IsCompleted = newUserReadingSession.IsCompleted.Value;
                    response.IsSuccess = true;
                    response.message = "Tạo phiên theo dõi đọc sách thành công";
                    response.data = userReadingSessionResponse;
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.message = "Đã có phiên đọc của cuốn sách này";
                    response.data = null;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.message = ex.Message;
                return response;
            }
            
        }

        public async Task<BaseResponse<UserReadingSessionResponse>> GetLastPosition(Guid bookId)
        {
            BaseResponse<UserReadingSessionResponse> response = new BaseResponse<UserReadingSessionResponse>();
            var currentUserId = getCurrentUserId();

            UserReadingSession lastSession = await _context.userReadingSessions.Where(x => x.IsDeleted == false && x.UserId == currentUserId && x.BookId == bookId).OrderByDescending(x => x.StartTime).FirstOrDefaultAsync();
            if (lastSession == null)
            {
                response.IsSuccess = false;
                response.message = "Chưa có lịch sử đọc sách này";
                return response;
            }
            UserReadingSessionResponse readingResponse = new UserReadingSessionResponse();
            readingResponse.Id = lastSession.Id;
            readingResponse.UserId = lastSession.UserId;
            readingResponse.BookId = lastSession.BookId;
            readingResponse.StartTime = lastSession.StartTime;
            readingResponse.EndTime = lastSession.EndTime.Value;
            readingResponse.LastPageNumber = lastSession.LastPageNumber;
            readingResponse.TotalPages = lastSession.TotalPages;
            readingResponse.DurationSeconds = lastSession.DurationSeconds;
            readingResponse.ReadingProgress = lastSession.ReadingProgress;
            readingResponse.IsCompleted = lastSession.IsCompleted.Value;

            response.IsSuccess = true;
            response.message = "Lấy vị trí đọc cuối cùng thành công";
            response.data = readingResponse;

            return response;
        }

        public async Task<BaseResponse<UserReadingSessionResponse>> UpdateReadingSession(Guid sessionId, UserReadingSessionUpdateRequest updateRequest)
        {
            BaseResponse<UserReadingSessionResponse> response = new BaseResponse<UserReadingSessionResponse>();
            var currentUserId = getCurrentUserId();
            UserReadingSession find = await _context.userReadingSessions.FirstOrDefaultAsync(x => x.IsDeleted == false && x.IsCompleted == false && x.Id == sessionId && x.UserId == currentUserId);
            if (find == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại phiên đọc";
                return response;
            }
            if (updateRequest.LastPageNumber < 1)
            {
                response.IsSuccess = false;
                response.message = "Trang cuối cùng đã đọc không được bé hơn 1";
                return response;
            }
            if (updateRequest.LastPageNumber > find.TotalPages)
            {
                response.IsSuccess = false;
                response.message = "Số trang cuối không được lớn hơn tổng số trang của sách";
                return response;
            }
            if (updateRequest.LastPageNumber == find.TotalPages)
            {
                find.IsCompleted = true;
                find.EndTime = DateTime.Now;
            }
            find.LastPageNumber = updateRequest.LastPageNumber;
            find.DurationSeconds = updateRequest.DurationSeconds;
            find.ReadingProgress = find.TotalPages > 0 ? (decimal)updateRequest.LastPageNumber / find.TotalPages * 100 : 0;
            find.UpdateDate = DateTime.Now;
            find.UpdateUser = getCurrentName();
            find.EndTime = DateTime.Now;
            _context.userReadingSessions.Update(find);
            await _context.SaveChangesAsync();
            UserReadingSessionResponse readingResponse = new UserReadingSessionResponse();
            readingResponse.Id = sessionId;
            readingResponse.StartTime = find.StartTime;
            readingResponse.UserId = currentUserId;
            readingResponse.BookId = find.BookId;
            readingResponse.DurationSeconds = updateRequest.DurationSeconds;
            readingResponse.LastPageNumber = updateRequest.LastPageNumber;
            readingResponse.ReadingProgress = find.ReadingProgress;
            response.IsSuccess = true;
            response.message = "Đã cập nhật thành công";
            response.data = readingResponse;
            return response;
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
        private string getCurrentName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
