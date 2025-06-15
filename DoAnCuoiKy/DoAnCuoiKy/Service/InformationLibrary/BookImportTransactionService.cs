using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Enum.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookImportTransactionService : IBookImportTransactionService
    {
        private readonly ApplicationDbContext _context;
        public BookImportTransactionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<BookImportTransactionResponse>> AddBookImportTransaction(Guid BookId, BookImportTransactionRequest importTransactionRequest)
        {
            BaseResponse<BookImportTransactionResponse> response = new BaseResponse<BookImportTransactionResponse>();
            
            BookImportTransaction bookImportTransaction = new BookImportTransaction();
            Book book = await _context.books.FirstOrDefaultAsync(x=>x.Id == BookId);
            if (book == null)
            {
                response.IsSuccess = false;
                response.message = "Lỗi không thấy BookId";
                return response;
            }
            bookImportTransaction.Id = Guid.NewGuid();
            bookImportTransaction.BookId = BookId;
            bookImportTransaction.Quantity = importTransactionRequest.Quantity;
            bookImportTransaction.UnitPrice = importTransactionRequest.UnitPrice;
            bookImportTransaction.TotalPrice = importTransactionRequest.UnitPrice * importTransactionRequest.Quantity;

            bookImportTransaction.TransactionType = TransactionType.Import;
            bookImportTransaction.CreateDate = DateTime.Now;
            await _context.bookImportTransactions.AddAsync(bookImportTransaction);
            await _context.SaveChangesAsync();
            BookImportTransactionResponse bookImportTransactionResponse = new BookImportTransactionResponse();
            bookImportTransactionResponse.Id = bookImportTransaction.Id;
            bookImportTransactionResponse.BookId = bookImportTransaction.BookId;
            bookImportTransactionResponse.BookTitle = bookImportTransaction.book.Title;
            bookImportTransactionResponse.Quantity = bookImportTransaction.Quantity;
            bookImportTransactionResponse.UnitPrice = bookImportTransaction.UnitPrice;
            bookImportTransactionResponse.TotalPrice = bookImportTransaction.TotalPrice;
            bookImportTransactionResponse.TransactionType = bookImportTransaction.TransactionType;
            bookImportTransactionResponse.CreatedDate = bookImportTransaction.CreateDate;
            response.IsSuccess = true;
            response.message = "Tạo lịch sử nhập thành công";
            response.data = bookImportTransactionResponse;
            return response;
        }

        public async Task<BaseResponse<List<BookImportTransactionResponse>>> GetAllBookImportTransaction()
        {
            BaseResponse<List<BookImportTransactionResponse>> response = new BaseResponse<List<BookImportTransactionResponse>>();
            List<BookImportTransactionResponse> importTransactions = await _context.bookImportTransactions.Include(x=>x.book).Where(x=>x.IsDeleted == false).Select(bookImportTransaction => new BookImportTransactionResponse
            {
                Id = bookImportTransaction.Id,
                BookId = bookImportTransaction.BookId,
                BookTitle = bookImportTransaction.book.Title,
                Quantity = bookImportTransaction.Quantity,
                UnitPrice = bookImportTransaction.UnitPrice,
                TotalPrice = bookImportTransaction.TotalPrice,
                TransactionType = bookImportTransaction.TransactionType,
                CreatedDate = bookImportTransaction.CreateDate

            }).ToListAsync();
            if (importTransactions == null)
            {
                response.IsSuccess = false;
                response.message = "Lấy danh sách thất bại";
                return response;
            }
            response.IsSuccess = true;
            response.message = "Lấy danh sách thành công";
            response.data = importTransactions;
            return response;
        }

        public async Task<BaseResponse<BookImportTransactionResponse>> GetBookImportTransactionById(Guid BookId)
        {
            BaseResponse<BookImportTransactionResponse> response = new BaseResponse<BookImportTransactionResponse>();
            BookImportTransaction findTransaction = await _context.bookImportTransactions.Include(x=>x.book).Where(x=>x.IsDeleted==false).FirstOrDefaultAsync(x=>x.BookId == BookId);
            if (findTransaction == null)
            {
                response.IsSuccess = false;
                response.message = "Sách không tồn tại";
                return response;
            }
            BookImportTransactionResponse bookImportTransaction = new BookImportTransactionResponse();
            bookImportTransaction.Id = findTransaction.Id;
            bookImportTransaction.BookId = findTransaction.BookId;
            bookImportTransaction.BookTitle = findTransaction.book.Title;
            bookImportTransaction.Quantity = findTransaction.Quantity;
            bookImportTransaction.UnitPrice = findTransaction.UnitPrice;
            bookImportTransaction.TotalPrice = findTransaction.TotalPrice;
            bookImportTransaction.TransactionType = findTransaction.TransactionType;
            bookImportTransaction.CreatedDate = findTransaction.CreateDate;
            response.IsSuccess = true;
            response.message = "Lấy giao dịch thành công";
            response.data= bookImportTransaction;
            return response;

        }
    }
}
