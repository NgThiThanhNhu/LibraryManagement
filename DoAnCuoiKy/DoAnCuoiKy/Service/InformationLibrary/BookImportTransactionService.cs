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
        public async Task<BaseResponse<BookImportTransactionResponse>> AddBookImportTransaction(BookImportTransactionRequest importTransactionRequest)
        {
            BaseResponse<BookImportTransactionResponse> response = new BaseResponse<BookImportTransactionResponse>();
            
            BookImportTransaction bookImportTransaction = new BookImportTransaction();
            Book book = await _context.books.FirstOrDefaultAsync(x=>x.Id == importTransactionRequest.BookId);
            if (book == null)
            {
                response.IsSuccess = false;
                response.message = "Lỗi không thấy BookId";
                return response;
            }
            bookImportTransaction.Id = Guid.NewGuid();
            bookImportTransaction.BookId = importTransactionRequest.BookId.Value;
            bookImportTransaction.TransactionType = TransactionType.Import;
            bookImportTransaction.CreateDate = DateTime.Now;
            await _context.bookImportTransactions.AddAsync(bookImportTransaction);
            await _context.SaveChangesAsync();
            BookImportTransactionResponse bookImportTransactionResponse = new BookImportTransactionResponse();
            bookImportTransactionResponse.Id = bookImportTransaction.Id;
            bookImportTransactionResponse.BookId = bookImportTransaction.BookId;
            bookImportTransactionResponse.BookTitle = bookImportTransaction.book.Title;
            bookImportTransactionResponse.Quantity = bookImportTransaction.book.Quantity;
            bookImportTransactionResponse.UnitPrice = bookImportTransaction.book.UnitPrice;
            bookImportTransactionResponse.TotalPrice = bookImportTransaction.book.TotalPrice;
            bookImportTransactionResponse.TransactionType = bookImportTransaction.TransactionType;
            bookImportTransactionResponse.Created = bookImportTransaction.CreateDate;
            response.IsSuccess = true;
            response.message = "Ghi log thành công";
            response.data = bookImportTransactionResponse;
            return response;
        }
    }
}
