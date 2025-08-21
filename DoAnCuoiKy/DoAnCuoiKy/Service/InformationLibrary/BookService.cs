using Azure.Core;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Helper;
using DoAnCuoiKy.Model;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.VisualBasic;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookImportTransactionService _importService;
        //private static Dictionary<Guid, BookInfomation> BookInforStored = new();

        public BookService(ApplicationDbContext context, IBookImportTransactionService importService)
        {
            _context = context;
            _importService = importService;
        }
        public async Task<BaseResponse<BookResponse>> AddBook(BookRequest bookRequest)
        {
            BaseResponse<BookResponse> response = new BaseResponse<BookResponse>();
            Book newbook = new Book();
            newbook.Id = Guid.NewGuid();
           
            newbook.Title = bookRequest.Title;
            BookAuthor bookAuthor = await _context.bookAuthors.FirstOrDefaultAsync(x => x.Id == bookRequest.BookAuthorId);
            if (bookAuthor == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại AuthorId";
                return response;
            }
            newbook.BookAuthorId = bookRequest.BookAuthorId.Value;
            Publisher publisher = await _context.publishers.FirstOrDefaultAsync(x => x.Id == bookRequest.PublisherId);
            if(publisher == null)
            {
                response.IsSuccess = false;
                response.message = "PublisherId không tồn tại";
                return response;
            }
            newbook.PublisherId = bookRequest.PublisherId.Value;
            newbook.YearPublished = bookRequest.YearPublished;
            if(bookRequest.Quantity > 0)
            {
            newbook.Quantity = bookRequest.Quantity;
            }
            else
            {
                response.IsSuccess = false;
                response.message = "Số lượng phải lớn hơn 0";
                return response;
            }

            BookCategory bookCategory = await _context.bookCategories.FindAsync(bookRequest.CategoryId);
            if (bookCategory == null)
            {
                response.message = "Không tồn tại CategoryId trong cơ sở dữ liệu";
                return response;
            }
            newbook.CategoryId = bookRequest.CategoryId;

            BookChapter bookChapter = await _context.bookChapters.FindAsync(bookRequest.BookChapterId);
            if (bookChapter == null)
            {
                response.message = "Không tồn tại BookChapterId trong bảng";
                return response;
            }

            newbook.BookChapterId = bookRequest.BookChapterId.Value;

            newbook.UnitPrice = bookRequest.UnitPrice;
            newbook.TotalPrice = CalculateTotalPrice(newbook.Quantity, newbook.UnitPrice);
            newbook.Description = bookRequest.Description;
            newbook.Slug = SlugHelper.Slugify(bookRequest.Title);
            await _context.books.AddAsync(newbook);
            await _context.SaveChangesAsync();
            if (newbook == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm thất bại";
                return response;
            }
            //BookInfomation bookInfomationData = new BookInfomation();
            //bookInfomationData.Description = bookRequest.Description;
            //BookInforStored[newbook.Id] = bookInfomationData;
            //if (!BookInforStored.ContainsKey(newbook.Id))
            //{
            //    response.IsSuccess = false;
            //    response.message = "Id không tồn tại";
            //    return response;
            //}

            BookImportTransactionRequest importRequest = new BookImportTransactionRequest();
            importRequest.Quantity = newbook.Quantity;
            importRequest.UnitPrice = newbook.UnitPrice.Value;
            importRequest.TotalPrice = newbook.TotalPrice.Value;
            // Gọi import service để ghi nhận lần nhập
            var importResult = await _importService.AddBookImportTransaction(newbook.Id, importRequest);//dto.UserId);
            if (importResult == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm sách thành công, nhưng thêm lịch sử nhập kho thất bại: " + importResult.message;
                return response;
            }

            

            BookResponse bookResponse = new BookResponse();
            bookResponse.Id = newbook.Id;
            bookResponse.Title = newbook.Title;
            bookResponse.Quantity = newbook.Quantity;
            bookResponse.YearPublished = newbook.YearPublished;
            bookResponse.BookAuthorId = newbook.BookAuthorId;
            bookResponse.AuthorName = newbook.BookAuthor.Name;
            bookResponse.PublisherId = newbook.PublisherId;
            bookResponse.PublisherName = newbook.Publisher.PublisherName;
            bookResponse.CategoryId = newbook.CategoryId;
            bookResponse.CategoryName = newbook.Category.Name;
            bookResponse.BookChapterId = newbook.BookChapterId;
            bookResponse.TitleBookChapter = newbook.BookChapter.TitleChapter;
            bookResponse.UnitPrice = newbook.UnitPrice;
            bookResponse.TotalPrice = newbook.TotalPrice;
            bookResponse.Slug = newbook.Slug;
            bookResponse.Description = newbook.Description;
            
            response.IsSuccess = true;
            response.message = "Thêm sách và ghi nhận nhập kho thành công";
            response.data = bookResponse;

            return response;
        }

        public async Task<BaseResponse<BookResponse>> DeleteBook(Guid id)
        {
            //xóa book thì xóa hết BookItem dùng transion

            BaseResponse<BookResponse> response = new BaseResponse<BookResponse>();
            Book book = await _context.books.FirstOrDefaultAsync(x=>x.Id == id);
            if (book == null)
            {
                response.IsSuccess = false;
                response.message = "Không tồn tại dữ liệu muốn xóa";
                return response;
            }
            book.IsDeleted = true;
            //if (!BookInforStored.ContainsKey(id))
            //{
            //    response.message = "Không tồn tại id chứa mô tả trong mảng";
            //    response.IsSuccess = false;
            //    return response;
            //}
            //BookInforStored.Remove(id);
            book.deleteUser = "Nguyen Thi Thanh Nhu";
            book.DeleteDate = DateTime.Now;
            _context.books.Update(book);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa dữ liệu thành công";
            return response;

        }

        public async Task<BaseResponse<List<BookResponse>>> GetAllBook()
        {
            BaseResponse<List<BookResponse>> response = new BaseResponse<List<BookResponse>>();
            List<BookResponse> books = await _context.books.Include(x=>x.bookFiles).Include(b => b.BookAuthor).Include(b=>b.Publisher).Include(b=>b.Category).Include(b=>b.BookChapter).Where(b => b.IsDeleted == false).Select(b => new BookResponse
            {
                Id = b.Id,
                Title = b.Title,
                YearPublished = b.YearPublished,
                Quantity = b.Quantity,
                UnitPrice = b.UnitPrice,
                TotalPrice = b.TotalPrice,
                PublisherName = b.Publisher.PublisherName,
                AuthorName = b.BookAuthor.Name,
                CategoryName = b.Category.Name,
                TitleBookChapter = b.BookChapter.TitleChapter,
                Description = b.Description,
                Slug = b.Slug,
                BookFileId = b.bookFiles.Where(x=>x.IsDeleted == false).Select(x=>x.Id).ToList(),
                ImageUrls = b.bookFiles.Where(x=>x.IsDeleted == false && !string.IsNullOrWhiteSpace(x.ImageUrl)).Select(x=>x.ImageUrl).ToList(),
                FileUrls = b.bookFiles.Where(x=>x.IsDeleted == false && !string.IsNullOrWhiteSpace(x.FileUrl)).Select(x=>x.FileUrl).ToList()
            }).ToListAsync();
            
            if (books == null)
            {
                response.IsSuccess = false;
                response.message = "Lấy dữ liệu thất bại";
                return response;
            }
            //foreach (var book in books)
            //{
            //    if (BookInforStored.ContainsKey(book.Id.Value))
            //    {
            //        book.Description = BookInforStored[book.Id.Value].Description;
            //    }
            //}
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = books;
            return response;
        }

        public async Task<BaseResponse<BookResponse>> GetBookBySlug(string slug)
        {
            BaseResponse<BookResponse> response = new BaseResponse<BookResponse>();
            Book book = await _context.books.Include(x=>x.BookAuthor).Include(x=>x.Publisher).FirstOrDefaultAsync(x=> x.Slug == slug && x.IsDeleted == false);
            
            BookCategory bookCategory = await _context.bookCategories.FirstOrDefaultAsync(x => x.Id == book.CategoryId && x.IsDeleted == false);
            if(bookCategory == null)
            {
                response.message = "Loại sách không tồn tại";
                response.IsSuccess = false;
                return response;
            }
            BookChapter bookChapter = await _context.bookChapters.FirstOrDefaultAsync(x => x.Id == book.BookChapterId && x.IsDeleted ==false);
            if(bookChapter == null)
            {
                response.message = "Chương không tồn tại";
                response.IsSuccess = false;
                return response;
            }

            List<BookFile> bookFile = await _context.bookFiles.Where(x => x.IsDeleted == false && x.BookId == book.Id).ToListAsync();
            if (bookFile==null)
            {
                response.message = "bookFile này không tồn tại";
                response.IsSuccess = false;
                return response;
            }
            BookResponse bookResponse = new BookResponse();
            
            if(book == null)
            {
                response.IsSuccess = false;
                response.message = "Không tìm thấy dữ liệu";
                return response;
            }

            bookResponse.Id = book.Id;
            bookResponse.Title = book.Title;
            bookResponse.Quantity = book.Quantity;
            bookResponse.UnitPrice = book.UnitPrice;
            bookResponse.TotalPrice = book.TotalPrice;
            bookResponse.AuthorName = book.BookAuthor.Name;
            bookResponse.CategoryName = bookCategory.Name;
            bookResponse.TitleBookChapter = bookChapter.TitleChapter;
            bookResponse.Description = book.Description;
            //if (!BookInforStored.ContainsKey(id))
            //{
            //    response.message = "Không tồn tại mô tả này";
            //    response.IsSuccess = false;
            //    return response;
            //}
            //bookResponse.Description = BookInforStored[id].Description;
            foreach (var item in bookFile)
            {
                if(string.IsNullOrEmpty(item.ImageUrl) || string.IsNullOrEmpty(item.FileUrl))
                    continue;
                bookResponse.BookFileId.Add(item.Id);
                bookResponse.ImageUrls.Add(item.ImageUrl);
                bookResponse.FileUrls.Add(item.FileUrl);
            }
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = bookResponse;
            return response;
        }

        public async Task<BaseResponse<BookResponse>> UpdateBook(Guid id, BookRequest bookRequest)
        {
            BaseResponse<BookResponse> response = new BaseResponse<BookResponse>();
            Book book = await _context.books.Include(x=>x.BookAuthor).Include(x=>x.Publisher).FirstOrDefaultAsync(x=>x.Id == id);
            if(book == null)
            {
                response.IsSuccess = false;
                response.message = "Không có dữ liệu hiển thị";
                return response;
            }

            book.Title = bookRequest.Title;
            book.CategoryId = bookRequest.CategoryId;
            book.PublisherId = bookRequest.PublisherId;
            book.BookAuthorId = bookRequest.BookAuthorId;
            book.PublisherId = bookRequest.PublisherId;
            book.YearPublished= bookRequest.YearPublished;
            book.Quantity = bookRequest.Quantity;
            book.UnitPrice = bookRequest.UnitPrice;
            book.TotalPrice = CalculateTotalPrice(book.Quantity, book.UnitPrice);
            book.Description = bookRequest.Description;
            //if (!BookInforStored.ContainsKey(id))
            //{
            //    response.message = "Không tồn tại key id trong mảng";
            //    response.IsSuccess = false;
            //    return response;
            //}
            //BookInfomation updateDescription = new BookInfomation();
            //updateDescription.Description = bookRequest.Description;
            //BookInforStored[id] = updateDescription;


            _context.books.UpdateRange(book);
            await _context.SaveChangesAsync();

            BookResponse bookResponse = new BookResponse();
            
            BookCategory bookCategory = await _context.bookCategories.FirstOrDefaultAsync(c => c.Id == bookRequest.CategoryId);
            BookChapter bookChapter = await _context.bookChapters.FirstOrDefaultAsync(c => c.Id == bookRequest.BookChapterId);
            bookResponse.Id = id;
            bookResponse.Title = book.Title;
            bookResponse.AuthorName = book.BookAuthor.Name;
            bookResponse.PublisherName = book.Publisher.PublisherName;
            bookResponse.CategoryName = bookCategory.Name;
            bookResponse.TitleBookChapter = bookChapter.TitleChapter;
            bookResponse.YearPublished = book.YearPublished;
            bookResponse.Quantity = book.Quantity;
            bookResponse.UnitPrice = book.UnitPrice;
            bookResponse.TotalPrice = book.TotalPrice;
            bookResponse.Description = book.Description;

            //bookResponse.Description = BookInforStored[id].Description;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = bookResponse;
            return response;
        }
        //private BookResponse setBookResponse(Book book)
        //{
        //    BookResponse response = new BookResponse();
        //    response.Id = book.Id;
        //    response.Title = book.Title;
        //    response.YearPublished = book.YearPublished;
        //    response.Quantity = book.Quantity;
        //    return response;
        //}
        private float? CalculateTotalPrice(int? quantity, float? unitPrice)
        {
            if (quantity.HasValue && unitPrice.HasValue)
                return quantity.Value * unitPrice.Value; // => float

            return null;
        }

    }
}
