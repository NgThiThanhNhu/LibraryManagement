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
        

        public async Task<BaseResponse<List<BorrowingDetailResponse>>> GetBorrowingDetails(Guid borrowingId)
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

        public async Task<BaseResponse<List<BorrowingDetailResponse>>> GetBorrowingDetailsManaged()
        {
            BaseResponse<List<BorrowingDetailResponse>> response = new BaseResponse<List<BorrowingDetailResponse>>();
            List<BorrowingDetailResponse> borrowingDetailResponses = await _context.borrowingDetails.Include(x => x.borrowing).Include(x => x.bookItem.Book.BookAuthor).Include(x => x.bookItem.Book.Category).Include(x => x.bookItem.Book).ThenInclude(b => b.bookFiles).Where(x => x.IsDeleted == false ).Select(x => new BorrowingDetailResponse
                {               
                    BorrowingCode = x.borrowing.Code,
                    BookItemTitle = x.bookItem.Book.Title,
                    AuthorBookItem = x.bookItem.Book.BookAuthor.Name,
                    CategoryName = x.bookItem.Book.Category.Name,
                    QuantityStorage = x.bookItem.Book.Quantity,
                    UrlImage = x.bookItem.Book.bookFiles.FirstOrDefault().ImageUrl,
                    ReturnedDate = x.ReturnedDate.Value
                }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Danh sách chi tiết phiếu mượn";
            response.data = borrowingDetailResponses;
            return response;
        }

        public async Task<BaseResponse<BorrowingDetailResponse>> UpdateBorrowingDetail(Guid id, BorrowingDetailRequest borrowingDetailRequest)
        {
            BaseResponse<BorrowingDetailResponse> response = new BaseResponse<BorrowingDetailResponse>();
            BorrowingDetail borrowingDetail = await _context.borrowingDetails.Include(x => x.borrowing).Include(x => x.bookItem.Book.BookAuthor).Include(x => x.bookItem.Book.Category).Include(x => x.bookItem.Book).ThenInclude(b => b.bookFiles).Where(x => x.IsDeleted == false ).FirstOrDefaultAsync(x => x.BorrowingId == id);
            if(borrowingDetail == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại borrowingdetail theo borrowingId này";
                return response;
            }
            borrowingDetail.ReturnedDate = borrowingDetailRequest.ReturnedDate;
            borrowingDetail.bookStatus = borrowingDetailRequest.BookStatusBorrowingDetail;
            BookItem bookItem = await _context.bookItems.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == borrowingDetail.BookItemId);
            if (borrowingDetail.bookStatus == Model.Enum.InformationLibrary.BookStatus.Available)
                bookItem.BookStatus = borrowingDetail.bookStatus;
            else if (borrowingDetail.bookStatus == Model.Enum.InformationLibrary.BookStatus.Lost)
            {
                BaseResponse<FineResponse> fine = await _fineService.CreateFine(borrowingDetail.Id.Value, Model.Enum.InformationLibrary.FineReason.LostBook);
                bookItem.BookStatus = borrowingDetail.bookStatus.Value;
            }else if (borrowingDetail.bookStatus == Model.Enum.InformationLibrary.BookStatus.Damaged)
            {
                BaseResponse<FineResponse> fineDamaged = await _fineService.CreateFine(borrowingDetail.Id.Value, Model.Enum.InformationLibrary.FineReason.DamagedBook);
                bookItem.BookStatus = borrowingDetail.bookStatus.Value;
            }
            
            _context.borrowingDetails.Update(borrowingDetail);
            await _context.SaveChangesAsync();
            BorrowingDetailResponse borrowingDetailResponse = new BorrowingDetailResponse();
            borrowingDetailResponse.BorrowingCode = borrowingDetail.borrowing.Code;
            borrowingDetailResponse.BookItemTitle = borrowingDetail.bookItem.Book.Title;
            borrowingDetailResponse.UrlImage = borrowingDetail?.bookItem?.Book?.bookFiles?.FirstOrDefault()?.ImageUrl;
            borrowingDetailResponse.AuthorBookItem = borrowingDetail.bookItem.Book.BookAuthor.Name;
            borrowingDetailResponse.CategoryName = borrowingDetail.bookItem.Book.Category.Name;
            borrowingDetailResponse.QuantityStorage = borrowingDetail.bookItem.Book.Quantity;
            borrowingDetailResponse.ReturnedDate = borrowingDetail.ReturnedDate.Value;
            response.IsSuccess = true;
            response.message = "Cập nhật ngày trả sách thành công";
            response.data = borrowingDetailResponse;
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
