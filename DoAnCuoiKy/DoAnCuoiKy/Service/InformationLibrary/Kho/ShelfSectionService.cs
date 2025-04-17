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
            shelfSectionResponse.ShelfName = shelfSection.Shelf.ShelfName;
            response.IsSuccess = true;
            response.message = "Thêm dữ liệu thành công";
            return response;
        }

        public Task<BaseResponse<ShelfSectionResponse>> DeleteShelfSection(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<List<ShelfSectionResponse>>> GetAllShelfSection()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ShelfSectionResponse>> GetShelfSectionById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ShelfSectionResponse>> UpdateShelfSection(Guid id, ShelfSectionRequest shelfSectionRequest)
        {
            throw new NotImplementedException();
        }
    }
}
