using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary.Kho
{
    public class ShelfService : IShelfService
    {
        private readonly ApplicationDbContext _context;
        public ShelfService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<ShelfResponse>> AddShelf(ShelfRequest shelfRequest)
        {
            BaseResponse<ShelfResponse> response =  new BaseResponse<ShelfResponse>();
            Shelf shelf = new Shelf();
            shelf.Id = Guid.NewGuid();
            shelf.ShelfName = shelfRequest.ShelfName;
            shelf.NumberOfSections = shelfRequest.NumberOfSections;
            BookShelf bookShelf = await _context.bookShelves.Where(x=>x.IsDeleted == false && x.Id == shelfRequest.BookshelfId).FirstOrDefaultAsync();
            if (bookShelf == null) 
            { 
                response.IsSuccess = false;
                response.message = "Tủ sách không tồn tại";
                return response;
            }
            shelf.BookshelfId = shelfRequest.BookshelfId;
            _context.shelves.AddRange(shelf);
            await _context.SaveChangesAsync();
            ShelfResponse shelfResponse = new ShelfResponse();
            shelfResponse.Id = shelf.Id;
            shelfResponse.ShelfName = shelf.ShelfName;
            shelfResponse.NumberOfSections = shelf.NumberOfSections;
            shelfResponse.BookshelfName = bookShelf.BookShelfName;
            response.IsSuccess = true;
            response.message = "Thêm dữ liệu thành công";
            response.data = shelfResponse;
            return response;
        }

        public async Task<BaseResponse<ShelfResponse>> DeleteShelf(Guid id)
        {
            BaseResponse<ShelfResponse> response = new BaseResponse<ShelfResponse>();
            Shelf shelf = await _context.shelves.Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (shelf==null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                response.data = null;
                return response;
            }
            shelf.IsDeleted = true;
            _context.shelves.Update(shelf);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa dữ liệu thành công";
            response.data = null;
            return response;
        }

        public async Task<BaseResponse<List<ShelfResponse>>> GetAllShelf()
        {
            BaseResponse<List<ShelfResponse>> response = new BaseResponse<List<ShelfResponse>>();
            List<ShelfResponse> shelfResponses = await _context.shelves.Include(x=>x.Bookshelf).Where(x=>x.IsDeleted == false).Select(x=> new ShelfResponse
            {
                Id = x.Id,
                ShelfName = x.ShelfName,
                NumberOfSections = x.NumberOfSections,
                BookshelfName = x.Bookshelf.BookShelfName
            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = shelfResponses;
            return response;
        }

        public async Task<BaseResponse<ShelfResponse>> GetShelfById(Guid id)
        {
            BaseResponse<ShelfResponse> response = new BaseResponse<ShelfResponse>();
            Shelf shelf = await _context.shelves.Include(x=>x.Bookshelf).Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (shelf == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            ShelfResponse shelfResponse = new ShelfResponse();
            shelfResponse.Id = shelf.Id;
            shelfResponse.ShelfName = shelf.ShelfName;
            shelfResponse.NumberOfSections = shelf.NumberOfSections;
            shelfResponse.BookshelfName = shelf.Bookshelf.BookShelfName;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = shelfResponse;
            return response;
        }

        public async Task<BaseResponse<ShelfResponse>> UpdateShelf(Guid id, ShelfRequest shelfRequest)
        {
            BaseResponse<ShelfResponse> response = new BaseResponse<ShelfResponse>();
            Shelf shelf = await _context.shelves.Include(x => x.Bookshelf).Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (shelf==null)
            {
                response.IsSuccess=false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            shelf.UpdateDate = DateTime.Now;
            shelf.ShelfName= shelfRequest.ShelfName;
            shelf.NumberOfSections = shelfRequest.NumberOfSections;
            shelf.BookshelfId = shelfRequest.BookshelfId;
            _context.shelves.UpdateRange(shelf);
            await _context.SaveChangesAsync();
            ShelfResponse shelfResponse = new ShelfResponse();
            shelfResponse.Id = shelf.Id;
            shelfResponse.ShelfName = shelf.ShelfName;
            shelfResponse.NumberOfSections = shelf.NumberOfSections;
            shelfResponse.BookshelfName = shelf.Bookshelf.BookShelfName;
            response.IsSuccess = true;
            response.message = "Cập nhật dữ liệu thành công";
            response.data = shelfResponse;
            return response;
        }
    }
}
