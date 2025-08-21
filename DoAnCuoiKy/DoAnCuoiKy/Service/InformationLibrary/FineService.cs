using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class FineService : IFineService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FineService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<FineResponse>> CreateFine(Guid BorrowingDetailId, FineReason fineReason)
        {
            BaseResponse<FineResponse> response = new BaseResponse<FineResponse>();
            Fine fine = new Fine();
            fine.Id = Guid.NewGuid();
            BorrowingDetail find = await _context.borrowingDetails.Include(x=>x.borrowing).Include(x=>x.bookItem.Book).Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == BorrowingDetailId);

            if (find == null || find.ReturnedDate == null) 
            { 
                response.IsSuccess = true;
                response.message = "Không tìm thấy chi tiết mượn sách hoặc chưa trả sách";
                return response;
            }

            if (fineReason == Model.Enum.InformationLibrary.FineReason.LateReturn && fine.IsPaid == false)
            {
                fine.Amount = getCalculateFine(find.ReturnedDate.Value, find.borrowing.DueDate);
                fine.fineReason = fineReason;

            }else if(fineReason == Model.Enum.InformationLibrary.FineReason.DamagedBook || fineReason == Model.Enum.InformationLibrary.FineReason.LostBook && fine.IsPaid == false)
            {
                fine.Amount = find.bookItem.Book.UnitPrice;
                fine.fineReason = fineReason;
            }
            fine.BorrowingDetailId = BorrowingDetailId;
            var currentUser = getCurrentUserId();
            fine.LibrarianId = currentUser;
            fine.UserId = find.borrowing.UserId.Value;
            _context.fines.Add(fine);
            await _context.SaveChangesAsync();

            FineResponse fineResponse = new FineResponse();
            fineResponse.Id = fine.Id.Value;
            fineResponse.BorrowingDetailId = fine.BorrowingDetailId;
            fineResponse.Amount = fine.Amount;
            fineResponse.fineReason = fine.fineReason;
            fineResponse.IsPaid = fine.IsPaid;
            Librarian librarian = await _context.librarians.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x=>x.Id == fine.UserId);
            fineResponse.userName = librarian.Name;
            Librarian librarian1 = await _context.librarians.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == fine.LibrarianId);
            fineResponse.librarianName = librarian1.Name;
            response.IsSuccess = true;
            response.message = "Tạo tiền phạt thành công";
            response.data = fineResponse;
            return response;
        }

        private float getCalculateFine(DateTime returnedDate, DateTime dueDate) 
        {
            if (returnedDate == null || returnedDate <= dueDate) return 0;
            TimeSpan delay = returnedDate.Date - dueDate.Date;
            int daysLate = delay.Days;
            const float perDay = 5000f;
            return daysLate * perDay;
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
