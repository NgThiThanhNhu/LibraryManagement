using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service
{
    public class BookFileService : IBookFileService
    {
        private readonly ApplicationDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public BookFileService(ApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<BaseResponse<BookFileResponse>> UploadFileAndImage(BookFileRequest bookFileRequest)
        {
           BaseResponse<BookFileResponse> response = new BaseResponse<BookFileResponse>();

            Book book = await _context.books.FirstOrDefaultAsync(b => b.Id == bookFileRequest.BookId);
            if (book == null)
            {
                response.IsSuccess = false;
                response.message = "Book không tồn tại";
                return response;
            }

            BookFile bookFile = new BookFile();
            bookFile.Id = Guid.NewGuid();
            bookFile.BookId = bookFileRequest.BookId;
            var imageUploadResult = await _cloudinaryService.UploadFileAsync(bookFileRequest.Image, "bookfile/image");
            var fileUploadResult = await _cloudinaryService.UploadFileAsync(bookFileRequest.UploadFile, "bookfile/docs");

            bookFile.ImageUrl = imageUploadResult.Url;
            bookFile.PublicIdImage = imageUploadResult.PublicId;
            bookFile.FileUrl = fileUploadResult.Url;
            bookFile.PublicIdFile = fileUploadResult.PublicId;
            BookFileType type = BookFileType.None;

            if (!string.IsNullOrEmpty(bookFile.ImageUrl) && !string.IsNullOrEmpty(bookFile.FileUrl))
                type = BookFileType.All;
            else if (!string.IsNullOrEmpty(bookFile.FileUrl))
                type = BookFileType.PDF;
            else if (!string.IsNullOrEmpty(bookFile.ImageUrl))
                type = BookFileType.image;

            // Gán type vào nếu BookFile có trường lưu loại
            bookFile.bookFileType = type;
            await _context.bookFiles.AddAsync(bookFile);
            await _context.SaveChangesAsync();
            if(bookFile == null)
            {
                response.IsSuccess = false;
                response.message = "Lưu địa chỉ ảnh và file thất bại";
                return response;
            }
            BookFileResponse bookFileResponse = new BookFileResponse();
            bookFileResponse.Id = bookFile.Id;
            bookFileResponse.BookId= bookFile.BookId;
            Book book1 = await _context.books.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == bookFile.BookId);
            bookFileResponse.BookName = book1.Title;
            bookFileResponse.ImageUrl = bookFile.ImageUrl;
            bookFileResponse.PublicIdImage = bookFile.PublicIdImage;
            bookFileResponse.FileUrl = bookFile.FileUrl;
            bookFileResponse.PublicIdFile = bookFile.PublicIdFile;
            bookFileResponse.Type = bookFile.bookFileType;
            response.IsSuccess = true;
            response.message = "Upload ảnh và image thành công";
            response.data = bookFileResponse;
            return response;
        }
    }
}
