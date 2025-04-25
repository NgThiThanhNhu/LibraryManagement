using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary.Kho
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;
        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<LocationResponse>> AddLocation(LocationRequest locationRequest)
        {
            BaseResponse<LocationResponse> response = new BaseResponse<LocationResponse>();
            Location location = new Location();
            ShelfSection section = await _context.shelfSections.Include(x=>x.Shelf).ThenInclude(x=>x.Bookshelf).ThenInclude(r=>r.Room).ThenInclude(f=>f.Floor).Where(x => x.IsDeleted == false && x.Id == locationRequest.ShelfSectionId).FirstOrDefaultAsync();
            
            if (section == null)
            {
                response.IsSuccess = false;
                response.message = "section không tồn tại";
                return response;
            }
            location.Id = Guid.NewGuid();
            location.ShelfSectionId = section.Id;
            location.Description = locationRequest.Description;
            _context.locations.Add(location);
            await _context.SaveChangesAsync();
            if (location == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            LocationResponse locationResponse = new LocationResponse();
            locationResponse.Id = location.Id;
            locationResponse.Description = location.Description;
            locationResponse.SectionName = section.SectionName;
            locationResponse.ShelfName = section.Shelf.ShelfName;
            locationResponse.BookShelfName = section.Shelf.Bookshelf.BookShelfName;
            locationResponse.RoomName = section.Shelf.Bookshelf.Room.RoomName;
            locationResponse.FloorName = section.Shelf.Bookshelf.Room.Floor.FloorName;
            locationResponse.LocationStatus = location.LocationStatus;
            response.IsSuccess = true;
            response.message = "Thêm dữ liệu thành công";
            response.data = locationResponse;
            return response;
        }

        public async Task<BaseResponse<LocationResponse>> DeleteLocation(Guid id)
        {
            BaseResponse<LocationResponse> response = new BaseResponse<LocationResponse>();
            Location location = await _context.locations.Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (location == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                response.data = null;
                return response;
            }
            location.IsDeleted = true;
            _context.locations.Update(location);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa thành công";
            response.data = null;
            return response;
        }

        public async Task<BaseResponse<List<LocationResponse>>> GetAllLocation()
        {
            BaseResponse<List<LocationResponse>> response = new BaseResponse<List<LocationResponse>>();
            List<LocationResponse> locationResponses = await _context.locations.Include(x => x.ShelfSection).ThenInclude(x => x.Shelf).ThenInclude(x => x.Bookshelf).ThenInclude(x => x.Room).ThenInclude(x => x.Floor).Where(x => x.IsDeleted == false).Select(x => new LocationResponse
            {
                Id = x.Id,
                SectionName = x.ShelfSection.SectionName,
                ShelfName = x.ShelfSection.Shelf.ShelfName,
                BookShelfName = x.ShelfSection.Shelf.Bookshelf.BookShelfName,
                RoomName = x.ShelfSection.Shelf.Bookshelf.Room.RoomName,
                FloorName = x.ShelfSection.Shelf.Bookshelf.Room.Floor.FloorName,
                Description = x.Description,
                LocationStatus = x.LocationStatus

            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy danh sách thành công";
            response.data = locationResponses;
            return response;
        }

        public async Task<BaseResponse<LocationResponse>> GetLocationById(Guid id)
        {
            BaseResponse<LocationResponse> response = new BaseResponse<LocationResponse>();
            Location location = await _context.locations.Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if(location == null)
            {
                response.IsSuccess= false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            LocationResponse locationResponse = new LocationResponse();
            locationResponse.Id = location.Id;
            locationResponse.SectionName = location.ShelfSection.SectionName;
            locationResponse.ShelfName = location.ShelfSection.Shelf.ShelfName;
            locationResponse.BookShelfName = location.ShelfSection.Shelf.Bookshelf.BookShelfName;
            locationResponse.RoomName = location.ShelfSection.Shelf.Bookshelf.Room.RoomName;
            locationResponse.FloorName = location.ShelfSection.Shelf.Bookshelf.Room.Floor.FloorName;
            locationResponse.Description = location.Description;
            locationResponse.LocationStatus = location.LocationStatus;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = locationResponse;
            return response;
        }

        public async Task<BaseResponse<LocationResponse>> UpdateLocation(Guid id, LocationRequest locationRequest)
        {
            BaseResponse<LocationResponse> response = new BaseResponse<LocationResponse>();
            Location location = await _context.locations.Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if(location == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            location.UpdateDate = DateTime.Now;
            ShelfSection shelfSection = await _context.shelfSections.Where(x=>x.IsDeleted == false && x.Id == locationRequest.ShelfSectionId).FirstOrDefaultAsync();
            if(shelfSection == null)
            {
                response.IsSuccess = false;
                response.message = "shelfSectionId không tồn tại";
                return response;
            }
            location.ShelfSectionId = locationRequest.ShelfSectionId;
            location.Description = locationRequest.Description;
            location.LocationStatus = locationRequest.LocationStatus;
            _context.locations.Update(location);
            await _context.SaveChangesAsync();
            LocationResponse locationResponse = new LocationResponse();
            locationResponse.SectionName = location.ShelfSection.SectionName;
            locationResponse.Description = location.Description;
            locationResponse.LocationStatus = location.LocationStatus;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = locationResponse;
            return response;
        }
    }
}
