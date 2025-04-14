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
            //thêm vào bảng rồi mới trả kết quả ra, mà kết quả này được lấy từ bảng
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

            //List<BookItem> bookItems2 = await _context.bookItems.ToListAsync();
            //foreach (var bookItem in bookItems2)
            //{
            //    BookItem item = await _context.bookItems.Include(x => x.Book).FirstOrDefaultAsync(x => x.Id == bookItem.Id);
            //    BookItemResponse bookItemResponse = new BookItemResponse();
            //    bookItemResponse.BookId = bookItem.BookId;
            //    bookItemResponse.Title = item.Book.Title;
            //    bookItemResponse.AuthorName = item.Book.BookAuthor.Name;
            //    bookItemResponse.PublisherName = item.Book.Publisher.PublisherName;


            //    //bookItemResponse.Title = bookItem.Title;
            //    //bookItemResponse.Author = bookItem.Author;
            //    //bookItemResponse.Publisher = bookItem.Publisher;
            //    //bookItemResponse.YearPublished = bookItem.YearPublished;
            //    //bookItemResponse.CategoryName = bookItem.Category.Name;
            //    //bookItemResponse.TitleBookChapter = bookItem.BookChapter.TitleChapter;
            //    bookItemResponse.BookStatus = bookItem.BookStatus;
            //    bookItemResponses.Add(bookItemResponse);
            //}
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
            //List<BookItemResponse> bookItemResponses = new List<BookItemResponse>();
            //List<BookItem> bookItems = await _context.bookItems.Where(x => x.IsDeleted == false).ToListAsync();
            //foreach (var bookItem in bookItems) 
            //{
            //    BookItemResponse bookItemResponse = new BookItemResponse();
            //    bookItemResponse.Id = bookItem.Id.Value;
            //    //bookItemResponse.Title = bookItem.Title;
            //    //bookItemResponse.Author = bookItem.Author;
            //    //bookItemResponse.Publisher = bookItem.Publisher;
            //    //bookItemResponse.YearPublished = bookItem.YearPublished;
            //    //bookItemResponse.bookStatus = bookItem.BookStatus;
            //    //bookItemResponse.Quantity = bookItem.Quantity;
            //    //bookItemResponse.CategoryName = bookItem.Category.Name;
            //    //bookItemResponse.TitleBookChapter = bookItem.BookChapter.TitleChapter;
            //    bookItemResponse.BookId = bookItem.BookId;
            //    bookItemResponses.Add(bookItemResponse);
            //}
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
            BookItem bookItem = await _context.bookItems.Where(x=>x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
            if(bookItem == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại dữ liệu";
                return response;
            }
            bookItemResponse.Id = bookItem.Id.Value;
            //bookItemResponse.Title = bookItem.Title;
            //bookItemResponse.BookId = bookItem.BookId;
            //bookItemResponse.Author = bookItem.Author;
            //bookItemResponse.Publisher = bookItem.Publisher;
            //bookItemResponse.YearPublished = bookItem.YearPublished;
            //bookItemResponse.bookStatus = bookItem.BookStatus;
            //bookItemResponse.Quantity = bookItem.Quantity;
            //BookCategory category = await _context.bookCategories.FirstOrDefaultAsync(x => x.Id == bookItem.BookCategoryId);
            //if(category == null)
            //{
            //    response.IsSuccess = false;
            //    response.message = "Loại sách không tồn tại";
            //    return response;
            //}
            //bookItemResponse.CategoryName = category.Name;
            //BookChapter chapter = await _context.bookChapters.FirstOrDefaultAsync(x => x.Id == bookItem.BookChapterId);
            //if (chapter == null) { 
            //    response.IsSuccess = false;
            //    response.message = "Chapter không tồn tại";
            //    return response;
            //}
            //bookItemResponse.TitleBookChapter = chapter.TitleChapter;
            response.IsSuccess = true;
            response.message = "getBookItemById thành công";
            response.data = bookItemResponse;
            return response;
        }

        public async Task<BaseResponse<BookItemResponse>> UpdateBookItem(Guid id, BookItemRequest bookItemRequest)
        {
            BaseResponse<BookItemResponse> response = new BaseResponse<BookItemResponse>();
            BookItem item = await _context.bookItems.FindAsync(id);
            if(item == null)
            {
                response.IsSuccess=false;
                response.message = "Không tìm thấy bookItemId";
                return response;
            }
            
            
            item.BookStatus = bookItemRequest.bookStatus;
            _context.bookItems.Update(item);
            await _context.SaveChangesAsync();
            
            BookItemResponse bookItemResponse = new BookItemResponse();
            //BookCategory category = await _context.bookCategories.FirstOrDefaultAsync(x=>x.Id == item.BookCategoryId);
            //BookChapter bookChapter = await _context.bookChapters.FirstOrDefaultAsync(x => x.Id == item.BookChapterId);
            bookItemResponse.Id = id;
            //bookItemResponse.Title = item.Title;
            //bookItemResponse.Author = item.Author;
            //bookItemResponse.Publisher = item.Publisher;
            //bookItemResponse.YearPublished = item.YearPublished;
            //bookItemResponse.CategoryName = category.Name;
            //bookItemResponse.TitleBookChapter = bookChapter.TitleChapter;
            //bookItemResponse.Quantity = item.Quantity;
            //bookItemResponse.bookStatus = item.BookStatus;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = bookItemResponse;
            return response;
        }
    }
}
