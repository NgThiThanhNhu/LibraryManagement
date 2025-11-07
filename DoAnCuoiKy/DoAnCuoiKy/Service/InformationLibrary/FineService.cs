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
        public async Task<BaseResponse<List<FineResponse>>> CreateFine(Guid borrowingDetailId, List<FineRequest> fineRequests)
        {
            BaseResponse<List<FineResponse>> response = new BaseResponse<List<FineResponse>>();
            BorrowingDetail findBorrowingDetail = await _context.borrowingDetails.Include(x=>x.borrowing).ThenInclude(x=>x.Librarian).FirstOrDefaultAsync(x => x.Id == borrowingDetailId);
            if(findBorrowingDetail == null)
            {
                response.IsSuccess = false;
                response.message = "Không tìm thấy chi tiết phiếu mượn phù hợp";
                return response;
            }
            findBorrowingDetail.ReturnedDate = DateTime.Now;
            findBorrowingDetail.UpdateDate = DateTime.Now;
            if (findBorrowingDetail.IsFined==false)
            {
                findBorrowingDetail.IsFined = true;
            }
            _context.borrowingDetails.Update(findBorrowingDetail);
            _context.SaveChanges();
            int daysLate = 0;
            List<FineResponse> fineResponses = new List<FineResponse>();
            FineResponse fineResponse = new FineResponse();
            foreach (var item in fineRequests)
            {
                Fine newFine = new Fine();
                newFine.Id = Guid.NewGuid();
                newFine.fineReason = item.fineReason;
                if (findBorrowingDetail.ReturnedDate.HasValue && findBorrowingDetail.borrowing?.DueDate != null)
                {
                    daysLate = (findBorrowingDetail.ReturnedDate.Value - findBorrowingDetail.borrowing.DueDate).Days;
                    if (daysLate < 0)
                        daysLate = 0;
                    newFine.DaysLate = daysLate;
                    if (newFine.fineReason == FineReason.LateReturn)
                    {
                        newFine.FineRate = item.FineRate;
                        newFine.Amount = newFine.DaysLate * newFine.FineRate;
                    }
                    if (newFine.fineReason == FineReason.LostBook && item.FineRate == findBorrowingDetail.bookItem.Book.UnitPrice)
                    {
                        item.FineRate = findBorrowingDetail.bookItem.Book.UnitPrice.Value;
                        newFine.FineRate = item.FineRate;
                        newFine.Amount = newFine.FineRate;
                    }
                    if (newFine.fineReason == FineReason.DamagedBook)
                    {
                        newFine.FineRate = item.FineRate;
                        newFine.Amount = newFine.FineRate;
                    }
                }
                newFine.IssuedDate = DateTime.Now;
                newFine.LibrarianId = getCurrentUserId();
                newFine.UserId = findBorrowingDetail.borrowing.UserId.Value;
                newFine.BorrowingDetailId = borrowingDetailId;
                _context.fines.Add(newFine);
                await _context.SaveChangesAsync();
                fineResponse.FineReason = newFine.fineReason;
                fineResponse.FineRate = newFine.FineRate;
                fineResponse.DaysLate = newFine.DaysLate;
                fineResponse.Amount = newFine.Amount;
                fineResponses.Add(fineResponse);
            }
            response.IsSuccess = true;
            response.message = "Tạo nộp phạt thành công";
            response.data = fineResponses;
            return response;
        }

        public async Task<BaseResponse<FineResponse>> GetFineById(Guid borrowingDetailId)
        {
            BaseResponse<FineResponse> response = new BaseResponse<FineResponse>();
            Fine fine = await _context.fines.FirstOrDefaultAsync(x => x.Id == borrowingDetailId);
            FineResponse fineResponse = new FineResponse();
            fineResponse.DaysLate = fine.DaysLate;
            fineResponse.Amount = fine.Amount;
            fineResponse.FineRate = fine.FineRate;
            fineResponse.FineReason = fine.fineReason;
            response.IsSuccess = true;
            response.message = "Hiển thị thông tin tiền phạt thành công";
            response.data = fineResponse;
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
