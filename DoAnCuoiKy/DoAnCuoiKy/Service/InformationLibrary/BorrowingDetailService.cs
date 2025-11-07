using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BorrowingDetailService : IBorrowingDetailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFineService _fineService;
        public BorrowingDetailService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IFineService fineService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _fineService = fineService;
        }
        

        public async Task<BaseResponse<List<BorrowingDetailResponse>>> GetBorrowingDetailsForUser(Guid borrowingId)
        {
            BaseResponse<List<BorrowingDetailResponse>> response = new BaseResponse<List<BorrowingDetailResponse>>();
            var currentUser = getCurrentUserId();
            List<BorrowingDetailResponse> borrowingDetailResponses = await _context.borrowingDetails.Include(x=>x.borrowing).Include(x=>x.bookItem.Book.BookAuthor).Include(x=>x.bookItem.Book.Category).Include(x=>x.bookItem.Book).ThenInclude(b=>b.bookFiles).Where(x=>x.IsDeleted == false && x.BorrowingId == borrowingId &&
                x.borrowing.UserId == currentUser).Select(x => new BorrowingDetailResponse 
                {
                BorrowingCode = x.borrowing.Code,
                BookItemTitle = x.bookItem.Book.Title,
                AuthorBookItem = x.bookItem.Book.BookAuthor.Name,
                CategoryName = x.bookItem.Book.Category.Name,
                QuantityStorage = x.bookItem.Book.Quantity,
                UrlImage = x.bookItem.Book.bookFiles.FirstOrDefault().ImageUrl,
                ReturnedDate = x.ReturnedDate.Value,
                IsScheduled = x.borrowing.isScheduled
                }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Danh sách chi tiết phiếu mượn";
            response.data = borrowingDetailResponses;
            return response;
        }

        public async Task<BaseResponse<List<BorrowingDetailForFineResponse>>> GetBorrowingDetailsForFine(Guid borrowingId)
        {
            BaseResponse<List<BorrowingDetailForFineResponse>> response = new BaseResponse<List<BorrowingDetailForFineResponse>>();
            List<BorrowingDetailForFineResponse> borrowingDetailResponses = await _context.borrowingDetails.Include(x => x.fines).Include(x => x.borrowing).Include(x => x.bookItem.Book).Where(x => x.IsDeleted == false && x.BorrowingId == borrowingId).Select(x => new BorrowingDetailForFineResponse
            {
                BorrowingDetailId = x.Id.Value,
                BookTitle = x.bookItem.Book.Title,
                ReturnedDate = x.ReturnedDate.Value,
                UnitPrice = x.bookItem.Book.UnitPrice.Value,
                fineResponses = x.fines.Where(y => y.BorrowingDetailId == x.Id.Value).Select(y => new FineResponse
                {
                    FineReason = y.fineReason,
                    Amount = y.Amount,
                    FineRate = y.FineRate,
                    DaysLate = y.DaysLate
                }).ToList(),
                IsFined = x.IsFined,
            }).ToListAsync();
            if (!borrowingDetailResponses.Any())
            {
                response.IsSuccess = false;
                response.message = " borrowingId này không tồn tại";
                return response;
            }
            response.IsSuccess = true;
            response.message = "Danh sách chi tiết phiếu mượn";
            response.data = borrowingDetailResponses;
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
    }
}
