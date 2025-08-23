using AutoMapper;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookExportTransactionService : IBookExportTransactionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public BookExportTransactionService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<BaseResponse<BookExportTransactionResponse>> CreateBookExportTransaction(Guid BorrowingDetailId)
        {
            BaseResponse<BookExportTransactionResponse> response = new BaseResponse<BookExportTransactionResponse>();
            BorrowingDetail findBorrowingDetail = await _context.borrowingDetails.Include(x => x.borrowing).Include(x=>x.bookItem).Include(x=>x.bookItem.ShelfSection).FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == BorrowingDetailId);
            if (findBorrowingDetail == null)
            {
                response.IsSuccess = false;
                response.message = "Chi tiết phiếu mượn này không tồn tại";
                return response;
            }
            BookExportTransaction newExportTransaction = new BookExportTransaction();
            newExportTransaction.Id = Guid.NewGuid();
            newExportTransaction.BorrowingDetailId = BorrowingDetailId;
            newExportTransaction.CreateUser = getCurrentName();
            newExportTransaction.CreateDate = DateTime.Now;
            if (findBorrowingDetail.borrowing.BorrowingStatus == Model.Enum.InformationLibrary.BorrowingStatus.Borrowing)
            {
                newExportTransaction.ExportReason = Model.Enum.InformationLibrary.Kho.ExportReason.Borrow;
                newExportTransaction.TransactionType = Model.Enum.InformationLibrary.Kho.TransactionType.Export;
            }
            else if (findBorrowingDetail.borrowing.BorrowingStatus == Model.Enum.InformationLibrary.BorrowingStatus.Returned)
            {
                newExportTransaction.ExportReason = null;
                newExportTransaction.TransactionType = Model.Enum.InformationLibrary.Kho.TransactionType.ReturnToStock;
            }
            _context.bookExportTransactions.Add(newExportTransaction);
            await _context.SaveChangesAsync();
            
            BookExportTransactionResponse bookExportTransactionResponse = _mapper.Map<BookExportTransactionResponse>(newExportTransaction);
            response.IsSuccess = true;
            response.message = "Tạo lịch sử export thành công";
            response.data = bookExportTransactionResponse;
            return response;

        }
        private string getCurrentName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
