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
            locationResponse.Room = section.Shelf.Bookshelf.Room.RoomName;
            locationResponse.Floor = section.Shelf.Bookshelf.Room.Floor.FloorName;
            locationResponse.LocationStatus = location.LocationStatus;
            response.IsSuccess = true;
            response.message = "Thêm dữ liệu thành công";
            response.data = locationResponse;
            return response;
        }

        public Task<BaseResponse<LocationResponse>> DeleteLocation(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<List<LocationResponse>>> GetAllLocation()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<LocationResponse>> GetLocationById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<LocationResponse>> UpdateLocation(Guid id, LocationRequest locationRequest)
        {
            throw new NotImplementedException();
        }
    }
}
