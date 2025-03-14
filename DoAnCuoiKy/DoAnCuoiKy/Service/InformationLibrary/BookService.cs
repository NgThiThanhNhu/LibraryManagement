using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<BookResponse>> AddBook(Guid categoryId, Guid bookChapterId, BookRequest bookRequest)
        {
            BaseResponse<BookResponse> response = new BaseResponse<BookResponse>();
            Book newbook = new Book();
            newbook.Title = bookRequest.Title;
            newbook.Author = bookRequest.Author;
            newbook.Publisher = bookRequest.Publisher;
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

            BookCategory bookCategory = await _context.bookCategories.FindAsync(categoryId);
            if (bookCategory == null)
            {
                response.message = "Không tồn tại CategoryId trong cơ sở dữ liệu";
                return response;
            }
            newbook.CategoryId = bookRequest.CategoryId;

            BookChapter bookChapter = await _context.bookChapters.FindAsync(bookChapterId);
            if (bookChapter == null)
            {
                response.message = "Không tồn tại BookChapterId trong bảng";
                return response;
            }
            newbook.BookChapterId = bookRequest.BookChapterId.Value;

            _context.books.AddAsync(newbook);
            await _context.SaveChangesAsync();

            if (newbook == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm thất bại";
                return response;
            }

            BookResponse bookResponse = new BookResponse();
            bookResponse.Id = newbook.Id;
            bookResponse.Title = newbook.Title;
            bookResponse.Author = newbook.Author;
            bookResponse.Publisher = newbook.Publisher;
            bookResponse.YearPublished = newbook.YearPublished;
            bookResponse.Quantity = newbook.Quantity;
            bookResponse.CategoryName = newbook.Category.Name;
            bookResponse.TitleBookChapter = newbook.BookChapter.TitleChapter;

            response.IsSuccess = true;
            response.message = "Thêm thành công";
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
            List<BookResponse> bookResponses = new List<BookResponse>();
            List<Book> books = await _context.books.Include(x=>x.Category).Include(x=>x.BookChapter).Where(b => b.IsDeleted == false).ToListAsync();
            //BookCategory category = new BookCategory();
            //foreach (Book item1 in books)
            //{
            //    category = await _context.bookCategories.FindAsync(item1.CategoryId);
            //    break;
            //}
            //BookChapter chapter = new BookChapter();
            //foreach(Book item2 in books)
            //{
            //    chapter = await _context.bookChapters.FindAsync(item2.BookChapterId);
            //    break;
            //}

            
            //FirstOrDefaultAsync dùng để tìm và trả về một phần tử đầu tiên thôi
            //.Select(b => new BookResponse
            //{
            //    Id = b.Id.Value,
            //    Title = b.Title,
            //    Author = b.Author,
            //    CategoryName = b.Category.Name
            /*}).ToListAsync();*/
            foreach (var book in books)
            {
                if(book.Category.IsDeleted == true || book.BookChapter.IsDeleted == true)
                {
                    continue;
                }
                BookResponse bookResponse = new BookResponse();
                
                bookResponse = setBookResponse(book);
                bookResponse.CategoryName = book.Category.Name;
                bookResponse.TitleBookChapter = book.BookChapter.TitleChapter;
                //lấy 1 giá trị ra xong để tiếp tục lặp, add nó vào list
                bookResponses.Add(bookResponse);
            }
            if (bookResponses == null)
            {
                response.IsSuccess = false;
                response.message = "Lấy dữ liệu thất bại";
                return response;
            }
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = bookResponses;
            return response;
        }

        public async Task<BaseResponse<BookResponse>> GetBookById(Guid id)
        {
            BaseResponse<BookResponse> response = new BaseResponse<BookResponse>();
            //cách không cần dùng tới khóa ngoại
            Book book = await _context.books.FirstOrDefaultAsync(x=> x.Id == id && x.IsDeleted == false);//FirstOrDefaultAsync chỉ lấy một phần tử đầu tiên
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
            BookResponse bookResponse = new BookResponse();
            
            if(book == null)
            {
                response.IsSuccess = false;
                response.message = "Không tìm thấy dữ liệu";
                return response;
            }
           
            bookResponse = setBookResponse(book);
            bookResponse.CategoryName = bookCategory.Name;
            bookResponse.TitleBookChapter = bookChapter.TitleChapter;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = bookResponse;
            return response;
        }

        public async Task<BaseResponse<BookResponse>> UpdateBook(Guid id, BookRequest bookRequest)
        {
            BaseResponse<BookResponse> response = new BaseResponse<BookResponse>();
            Book book = await _context.books.FindAsync(id);
            if(book == null)
            {
                response.IsSuccess = false;
                response.message = "Không có dữ liệu hiển thị";
                return response;
            }

            book.Title = bookRequest.Title;
            book.Author = bookRequest.Author;
            book.Publisher = bookRequest.Publisher;
            book.YearPublished= bookRequest.YearPublished;
            book.Quantity = bookRequest.Quantity;
            

            _context.books.UpdateRange(book);
            await _context.SaveChangesAsync();

            BookResponse bookResponse = new BookResponse();
            BookCategory bookCategory = await _context.bookCategories.FirstOrDefaultAsync(c => c.Id == bookRequest.CategoryId);
            BookChapter bookChapter = await _context.bookChapters.FirstOrDefaultAsync(c => c.Id == bookRequest.BookChapterId);
            bookResponse.Id = id;
            bookResponse = setBookResponse(book);
            bookResponse.CategoryName = bookCategory.Name;
            bookResponse.TitleBookChapter = bookChapter.TitleChapter;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = bookResponse;
            return response;
        }
        private BookResponse setBookResponse(Book book)
        {
            BookResponse response = new BookResponse();
            response.Id = book.Id;
            response.Title = book.Title;
            response.Author = book.Author;
            response.Publisher = book.Publisher;
            response.YearPublished = book.YearPublished;
            response.Quantity = book.Quantity;
            return response;
        }
    }
}
