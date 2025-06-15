using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary.Kho
{
    public class ShelfSectionService : IShelfSectionService
    {
        private readonly ApplicationDbContext _context;
        public ShelfSectionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<ShelfSectionResponse>> AddShelfSection(ShelfSectionRequest shelfSectionRequest)
        {
            BaseResponse<ShelfSectionResponse> response = new BaseResponse<ShelfSectionResponse>();
            bool canAdd = await CanAddShelfSectionToShelf(shelfSectionRequest.ShelfId);
            if (!canAdd)
            {
                response.IsSuccess = false;
                response.message = "Kệ sách đã đạt giới hạn ô sách, không thể thêm nữa.";
                return response;
            }
            ShelfSection shelfSection = new ShelfSection();
            shelfSection.Id = Guid.NewGuid();
            shelfSection.SectionName = shelfSectionRequest.SectionName;
            shelfSection.Capacity = shelfSectionRequest.Capacity;
            Shelf shelf = await _context.shelves.Where(x => x.IsDeleted == false && x.Id == shelfSectionRequest.ShelfId).FirstOrDefaultAsync();
            if (shelf==null)
            {
                response.IsSuccess = false;
                response.message = "ShelfId không tồn tại";
                return response;
            }
            shelfSection.ShelfId = shelf.Id;
            _context.shelfSections.AddRange(shelfSection);
            await _context.SaveChangesAsync();

            ShelfSectionResponse shelfSectionResponse = new ShelfSectionResponse();
            shelfSectionResponse.Id = shelfSection.Id;
            shelfSectionResponse.SectionName = shelfSection.SectionName;
            shelfSectionResponse.Capacity = shelfSection.Capacity;
            shelfSectionResponse.ShelfId = shelfSection.ShelfId;
            shelfSectionResponse.ShelfName = shelfSection.Shelf.ShelfName;
            response.IsSuccess = true;
            response.message = "Thêm dữ liệu thành công";
            return response;
        }

        public async Task<bool> CanAddShelfSectionToShelf(Guid shelfId)
        {
            var shelf = await _context.shelves
                .Include(s => s.Sections)
                .FirstOrDefaultAsync(bs => bs.Id == shelfId);

            if (shelf == null)
                throw new Exception("Tủ sách không tồn tại.");

            return shelf.Sections.Count < shelf.NumberOfSections;
        }

        public async Task<BaseResponse<ShelfSectionResponse>> DeleteShelfSection(Guid id)
        {
            BaseResponse<ShelfSectionResponse> response = new BaseResponse<ShelfSectionResponse>();
            ShelfSection shelfSection = await _context.shelfSections.Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (shelfSection == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            shelfSection.IsDeleted = true;
            _context.shelfSections.Update(shelfSection);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa dữ liệu thành công";
            response.data = null;
            return response;

        }

        public async Task<BaseResponse<List<ShelfSectionResponse>>> GetAllShelfSection()
        {
            BaseResponse<List<ShelfSectionResponse>> response = new BaseResponse<List<ShelfSectionResponse>>();
            List<ShelfSectionResponse> shelfSectionResponses = await _context.shelfSections.Include(x=>x.Shelf).Where(x=>x.IsDeleted == false).Select(x=> new ShelfSectionResponse
            {
                Id = x.Id,
                SectionName = x.SectionName,
                Capacity = x.Capacity,
                ShelfId = x.ShelfId,
                ShelfName = x.Shelf.ShelfName,
                CurrentBookItem = x.BookItems.Count(bi=>bi.IsDeleted==false)

            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = shelfSectionResponses;
            return response;
        }

        public async Task<BaseResponse<ShelfSectionResponse>> GetShelfSectionById(Guid id)
        {
            BaseResponse<ShelfSectionResponse> response = new BaseResponse<ShelfSectionResponse>();
            ShelfSection shelfSection = await _context.shelfSections.Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if(shelfSection == null)
            {
                response.IsSuccess=false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            ShelfSectionResponse shelfSectionResponse = new ShelfSectionResponse();
            shelfSectionResponse.Id = shelfSection.Id;
            shelfSectionResponse.ShelfId = shelfSection.ShelfId;
            shelfSectionResponse.ShelfName = shelfSection.Shelf.ShelfName;
            shelfSection.Capacity = shelfSection.Capacity;
            shelfSection.SectionName = shelfSection.SectionName;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = shelfSectionResponse;
            return response;
        }

        public async Task<BaseResponse<ShelfSectionResponse>> UpdateShelfSection(Guid id, ShelfSectionRequest shelfSectionRequest)
        {
            BaseResponse<ShelfSectionResponse> response = new BaseResponse<ShelfSectionResponse>();
            ShelfSection shelfSection = await _context.shelfSections.Include(x=>x.Shelf).Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if(shelfSection == null)
            {
                response.IsSuccess=false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            shelfSection.SectionName = shelfSectionRequest.SectionName;
            shelfSection.ShelfId = shelfSectionRequest.ShelfId;
            shelfSection.Capacity= shelfSectionRequest.Capacity;
            shelfSection.UpdateDate = DateTime.Now;
            _context.shelfSections.Update(shelfSection);
            await _context.SaveChangesAsync();
            ShelfSectionResponse shelfSectionResponse = new ShelfSectionResponse();
            shelfSectionResponse.Id = shelfSection.Id;
            shelfSectionResponse.SectionName = shelfSection.SectionName;
            shelfSectionResponse.Capacity = shelfSection.Capacity;
            shelfSectionResponse.ShelfName = shelfSection.Shelf.ShelfName;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = shelfSectionResponse;
            return response;

        }
    }
}
