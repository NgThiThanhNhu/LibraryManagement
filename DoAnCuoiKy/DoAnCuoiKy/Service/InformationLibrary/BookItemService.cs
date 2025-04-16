using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookItemService : IBookItemService
    {
        private readonly ApplicationDbContext _context;
        public BookItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<BookItemResponse>>> AddBookItem(BookItemRequest bookItemRequest)
        {
            //đang thêm bookitem theo bookId dựa vào số lượng
            BaseResponse<List<BookItemResponse>> response = new BaseResponse<List<BookItemResponse>>();
            List<BookItemResponse> bookItemResponses = new List<BookItemResponse>();
            Book book = await _context.books.Include(x=>x.Category).Include(x=>x.BookChapter).Where(b => b.IsDeleted == false).SingleOrDefaultAsync(x=>x.Id == bookItemRequest.BookId);//muốn có được bookitem thì phải biết được bookitem đó thuộc bookId nào
            if(book == null)
            {
                response.IsSuccess = false;
                response.message = "Không có dữ liệu để thêm BookItem";
                return response;
            }
            List<BookItem> bookItems1 = new List<BookItem>();
            for(int i = 0; i<book.Quantity; i++)
            {
                BookItem bookItem = new BookItem();
                bookItem.BookId = book.Id;
                bookItem.BookStatus = BookStatus.Available;
                bookItems1.Add(bookItem);
            }
                _context.bookItems.AddRange(bookItems1);
                await _context.SaveChangesAsync();

            List<BookItemResponse> bookItemResponse = await _context.bookItems.Include(x => x.Book).Where(x => x.IsDeleted == false && x.BookId == bookItemRequest.BookId).Select(i => new BookItemResponse
            {
                Id = i.Id,
                Title = i.Book.Title,
                AuthorName = i.Book.BookAuthor.Name,
                PublisherName = i.Book.Publisher.PublisherName,
                YearPublished = i.Book.YearPublished,
                UnitPrice = i.Book.UnitPrice,
                BookStatus = i.BookStatus.Value,
                CategoryName = i.Book.Category.Name,
                TitleBookChapter = i.Book.BookChapter.TitleChapter
            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Thêm dữ liệu thành công";
            response.data = bookItemResponse;
            return response;

        }
        //xong addBookitem
        public async Task<BaseResponse<BookItemResponse>> DeleteBookItem(Guid id)
        {
            //xóa rồi thì số lượng bookquantity bên bảng book cũng phải giảm
            BaseResponse<BookItemResponse> response = new BaseResponse<BookItemResponse>(); 
            BookItem bookItem = await _context.bookItems.Where(x=>x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
            if (bookItem == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            bookItem.IsDeleted = true;
            Book book = await _context.books.Where(x=>x.IsDeleted== false).FirstOrDefaultAsync(x => x.Id == bookItem.BookId);
           
            if(book == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại bookId";
                return response;
            }
            book.Quantity -= 1;
            _context.bookItems.Update(bookItem);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa thành công";
            return response;
        }
        //sửa xong delete
        public async Task<BaseResponse<List<BookItemResponse>>> GetAllBookItem()
        {
            BaseResponse<List<BookItemResponse>> response = new BaseResponse<List<BookItemResponse>>();
            List<BookItemResponse> bookItems = await _context.bookItems.Include(i=>i.Book).Where(i=>i.IsDeleted == false).Select(i=> new BookItemResponse
            {
                Id = i.Id,
                BookId = i.BookId,
                Title = i.Book.Title,
                AuthorName = i.Book.BookAuthor.Name,
                PublisherName = i.Book.Publisher.PublisherName,
                YearPublished = i.Book.YearPublished,
                UnitPrice = i.Book.UnitPrice,
                BookStatus = i.BookStatus,
                CategoryName = i.Book.Category.Name,
                TitleBookChapter = i.Book.BookChapter.TitleChapter
            }).ToListAsync();
            if(bookItems == null)
            {
                response.IsSuccess = false;
                response.message = "getList thất bại";
                return response;
            }
            response.IsSuccess = true;
            response.message = "getList thành công";
            response.data = bookItems;
            return response;
        }

        public async Task<BaseResponse<BookItemResponse>> GetBookItemById(Guid id)
        {
            BaseResponse<BookItemResponse> response = new BaseResponse<BookItemResponse>();
            BookItemResponse bookItemResponse = new BookItemResponse();
            BookItem bookItem = await _context.bookItems.Include(x=>x.Book).Where(x=>x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
            if(bookItem == null)
            {
                response.IsSuccess = false;
                response.message = "dữ liệu không tồn tại";
                return response;
            }
            bookItemResponse.Id = bookItem.Id.Value;
            bookItemResponse.BookId = bookItem.BookId.Value;
            bookItemResponse.Title = bookItem.Book.Title;
            bookItemResponse.TitleBookChapter = bookItem.Book.BookChapter.TitleChapter;
            bookItemResponse.CategoryName = bookItem.Book.Category.Name;
            bookItemResponse.AuthorName = bookItem.Book.BookAuthor.Name;
            bookItemResponse.PublisherName = bookItem.Book.Publisher.PublisherName;
            bookItemResponse.YearPublished = bookItem.Book.YearPublished;
            bookItemResponse.BookStatus = bookItem.BookStatus.Value;
            response.IsSuccess = true;
            response.message = "getBookItemById thành công";
            response.data = bookItemResponse;
            return response;
        }

        public async Task<BaseResponse<BookItemResponse>> UpdateBookItemStatus(Guid id, BookItemRequest bookItemRequest)
        {
            BaseResponse<BookItemResponse> response = new BaseResponse<BookItemResponse>();
            BookItem item = await _context.bookItems.Include(x=>x.Book).FirstOrDefaultAsync(x=>x.Id == id);
            if(item == null)
            {
                response.IsSuccess=false;
                response.message = "Không tìm thấy bookItemId";
                return response;
            }
            
            if(Enum.IsDefined(typeof(BookStatus), bookItemRequest.bookStatus)){
                item.BookStatus = (BookStatus)bookItemRequest.bookStatus;
            }
            else
            {
                response.IsSuccess = false;
                response.message = "BookSatus không hợp lệ";
                return response;
            }
            
            _context.bookItems.Update(item);
            await _context.SaveChangesAsync();
            
            BookItemResponse bookItemResponse = new BookItemResponse();
            bookItemResponse.Id = id;
            bookItemResponse.BookId = item.BookId;
            bookItemResponse.Title = item.Book.Title;
            bookItemResponse.AuthorName = item.Book.BookAuthor.Name;
            bookItemResponse.PublisherName = item.Book.Publisher.PublisherName;
            bookItemResponse.YearPublished = item.Book.YearPublished;
            bookItemResponse.CategoryName = item.Book.Category.Name;
            bookItemResponse.TitleBookChapter = item.Book.BookChapter.TitleChapter;
            bookItemResponse.BookStatus = item.BookStatus;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = bookItemResponse;
            return response;
        }
    }
}
