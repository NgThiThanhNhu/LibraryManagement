using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using DoAnCuoiKy.Utils;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookItemService : IBookItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookCartItemService _bookCartItemService;
        public BookItemService(ApplicationDbContext context, IBookCartItemService bookCartItemService)
        {
            _context = context;
            _bookCartItemService = bookCartItemService;
        }
        //public async Task<List<BookItem>> GenerateMultipleBookItems(Guid bookId, int quantity)
        //{
        //    string shortBookId = bookId.ToString().Substring(0, 6);

        //    // Đếm số lượng BookItem đã có trong DB với BookId này
        //    int existingCount = await _context.bookItems
        //        .CountAsync(b => b.BookId == bookId);

        //    List<BookItem> newBookItems = new List<BookItem>();

        //    for (int i = 1; i <= quantity; i++)
        //    {
        //        int currentSeq = existingCount + i;
        //        string barCode = $"BOOK-{shortBookId}-{currentSeq:D3}";

        //        BookItem bookItem = new BookItem
        //        {
        //            BookId = bookId,
        //            BookStatus = BookStatus.Available,
        //            BarCode = barCode
        //        };

        //        newBookItems.Add(bookItem);
        //    }

        //    return newBookItems;
        //}

        public async Task<BaseResponse<List<BookItem>>> DistributeBookItemsToShelfSections(BookItemRequest bookItemRequest)
        {
            string shortBookId = bookItemRequest.BookId.ToString().Substring(0, 6);
            int existingCount = await _context.bookItems.CountAsync(bi=>bi.BookId== bookItemRequest.BookId && bi.IsDeleted==false);
            var shelfSection = await _context.shelfSections.Include(s=>s.BookItems)
                .Where(s=>!s.BookItems.Any(b=>b.IsDeleted == false && b.BookId == bookItemRequest.BookId)|| s.BookItems.Count(b=>b.IsDeleted==false) < s.Capacity)
                .OrderBy(s=>s.SectionName).ToListAsync();
            if (!shelfSection.Any())
            {
                throw new Exception("Chưa có ô để sách nào khả dụng");
            }
            Book checkBook = await _context.books.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == bookItemRequest.BookId);
            if (checkBook == null)
                throw new Exception("Không tìm thấy sách.");

            if (bookItemRequest.Quantity != checkBook.Quantity)
            {
                throw new Exception("Sai số lượng");
            }
            int remaining = bookItemRequest.Quantity;
            var results = new List<BookItem>();
            int seq = existingCount + 1;
            foreach (var section in shelfSection)
            {
                int usedSection = section.BookItems.Count(bi => bi.IsDeleted == false);
                int available = section.Capacity - usedSection;
                if (available <= 0) continue;
                int toAdd = Math.Min(available, remaining);
                for (int i=0; i<toAdd; i++)
                {
                    var barcode = $"BOOK-{shortBookId}-{seq:D3}";
                    results.Add(new BookItem
                    {
                        BookId = bookItemRequest.BookId,
                        BookStatus = BookStatus.Available,
                        BarCode = barcode,
                        ShelfSectionId = section.Id
                    });
                    seq++;
                }
                remaining -= toAdd;
                if (remaining <= 0) break;
            }
            BaseResponse<List<BookItem>> response = new BaseResponse<List<BookItem>>();
            response.message = remaining > 0 ? $"Đã xếp được {bookItemRequest.Quantity - remaining} cuốn. Còn thiếu {remaining} chỗ chứa sách." : "Tất cả được xếp vào ô thành công.";
            response.data = results;
            return response;

        }

        public async Task<BaseResponse<List<BookItemResponse>>> AddBookItem(BookItemRequest bookItemRequest)
        {
            BaseResponse<List<BookItemResponse>> response = new BaseResponse<List<BookItemResponse>>();
            List<BookItemResponse> bookItemResponses = new List<BookItemResponse>();
            Book book = await _context.books.Include(x=>x.Category).Include(x=>x.BookChapter).Where(b => b.IsDeleted == false).FirstOrDefaultAsync(x=>x.Id == bookItemRequest.BookId);//muốn có được bookitem thì phải biết được bookitem đó thuộc bookId nào
            if(book == null)
                Global.getResponse(false, bookItemResponses, "Không tìm thấy Book để thêm BookItem");
            
            BaseResponse<List<BookItem>> bookItems1 = await DistributeBookItemsToShelfSections(bookItemRequest);
            
            _context.bookItems.AddRange(bookItems1.data);
            await _context.SaveChangesAsync();

            List<BookItemResponse> bookItemResponse = await _context.bookItems.Include(x=>x.ShelfSection).Include(x => x.Book).Where(x => x.IsDeleted == false && x.BookId == bookItemRequest.BookId).Select(i => new BookItemResponse
            {
                Id = i.Id,
               
                Title = i.Book.Title,
                AuthorName = i.Book.BookAuthor.Name,
                PublisherName = i.Book.Publisher.PublisherName,
                YearPublished = i.Book.YearPublished,
                UnitPrice = i.Book.UnitPrice,
                BookStatus = i.BookStatus.Value,
                CategoryName = i.Book.Category.Name,
                TitleBookChapter = i.Book.BookChapter.TitleChapter,
                BarCode = i.BarCode,
                
                ShelfSectionName = i.ShelfSection.SectionName
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
            List<BookItemResponse> bookItems = await _context.bookItems.Include(i=>i.Book).Include(x=>x.ShelfSection).Where(i=>i.IsDeleted == false && i.BookStatus == BookStatus.Borrowed).Select(i=> new BookItemResponse
            {
                Id = i.Id,
             
                Title = i.Book.Title,
                AuthorName = i.Book.BookAuthor.Name,
                PublisherName = i.Book.Publisher.PublisherName,
                YearPublished = i.Book.YearPublished,
                UnitPrice = i.Book.UnitPrice,
                BookStatus = i.BookStatus,
                CategoryName = i.Book.Category.Name,
                TitleBookChapter = i.Book.BookChapter.TitleChapter,
                BarCode = i.BarCode,
               
                ShelfSectionName = i.ShelfSection.SectionName
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

        public async Task<BaseResponse<BookItemResponse>> ChooseBookItemByBookId(Guid bookId)
        {
            BaseResponse<BookItemResponse> response = new BaseResponse<BookItemResponse>();
            BookItemResponse bookItemResponse = new BookItemResponse();
            BookItem bookItem = await _context.bookItems.Include(x=>x.ShelfSection).Include(x=>x.Book.bookFiles).Include(x=>x.Book.BookChapter).Include(x=>x.Book.Category).Include(x=>x.Book.BookAuthor).Include(x=>x.Book.Publisher).Where(x=>x.IsDeleted == false && x.BookStatus == BookStatus.Available).FirstOrDefaultAsync(x => x.BookId == bookId);
            if(bookItem == null)
            {
                response.IsSuccess = false;
                response.message = "Đã hết sách có thể mượn được";
                return response;
            }
            bookItem.BookStatus = BookStatus.Borrowed;
            BookCartItemRequest bookCartItemRequest = new BookCartItemRequest();
            bookCartItemRequest.BookItemId = bookItem.Id.Value;
            _context.bookItems.Update(bookItem);
            BaseResponse<BookCartItemResponse> bookCartItem = await _bookCartItemService.AddBookItemToCart(bookCartItemRequest);
            Book book = await _context.books.FirstOrDefaultAsync(x => x.Id == bookItem.BookId && x.IsDeleted == false);
            if (book != null)
            {
                book.Quantity--;
                _context.books.Update(book);
            }
            _context.SaveChanges();
           
            bookItemResponse.Id = bookItem.Id.Value;
            bookItemResponse.BarCode = bookItem.BarCode;
           
            bookItemResponse.Title = bookItem.Book.Title;
            bookItemResponse.TitleBookChapter = bookItem.Book.BookChapter.TitleChapter;
            bookItemResponse.CategoryName = bookItem.Book.Category.Name;
            bookItemResponse.AuthorName = bookItem.Book.BookAuthor.Name;
            bookItemResponse.PublisherName = bookItem.Book.Publisher.PublisherName;
            bookItemResponse.YearPublished = bookItem.Book.YearPublished;
            bookItemResponse.BookStatus = bookItem.BookStatus.Value;
            bookItemResponse.ShelfSectionName = bookItem.ShelfSection.SectionName;
            bookItemResponse.imageUrl = bookItem.Book.bookFiles.Select(x => x.ImageUrl).ToList();
            response.IsSuccess = true;
            response.message = "thêm vào danh sách thành công";
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
            
            _context.bookItems.UpdateRange(item);
            await _context.SaveChangesAsync();
            
            BookItemResponse bookItemResponse = new BookItemResponse();
            bookItemResponse.Id = id;
            bookItemResponse.BarCode = item.BarCode;
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
