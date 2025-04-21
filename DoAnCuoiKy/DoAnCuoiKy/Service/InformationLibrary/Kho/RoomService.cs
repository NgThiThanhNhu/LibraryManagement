using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary.Kho
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;
        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<RoomResponse>> AddRoom(RoomRequest roomRequest)
        {
            BaseResponse<RoomResponse> response = new BaseResponse<RoomResponse>();
            Room room = new Room();
            room.Id = Guid.NewGuid();
            room.RoomName = roomRequest.RoomName;
            room.FloorId = roomRequest.FloorId;
            _context.rooms.Add(room);
            await _context.SaveChangesAsync();
            if(room == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm thất bại";
                return response;
            }
            RoomResponse roomResponse = new RoomResponse();
            roomResponse.Id = room.Id;
            roomResponse.RoomName = room.RoomName;
            roomResponse.FloorName = room.Floor.FloorName;
            response.IsSuccess = true;
            response.message = "Thêm thành công";
            response.data = roomResponse;
            return response;
        }

        public async Task<BaseResponse<RoomResponse>> DeleteRoom(Guid id)
        {
            BaseResponse<RoomResponse> response = new BaseResponse<RoomResponse>();
            Room room = await _context.rooms.Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if(room == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            room.IsDeleted = true;
            _context.rooms.Update(room);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa dữ liệu thành công";
            response.data = null;
            return response;
        }

        public async Task<BaseResponse<List<RoomResponse>>> GetAllRoom()
        {
            BaseResponse<List<RoomResponse>> response =  new BaseResponse<List<RoomResponse>>();
            List<RoomResponse> roomResponses = await _context.rooms.Include(x => x.Floor).Where(x => x.IsDeleted == false).Select(x => new RoomResponse
            {
                Id = x.Id,
                RoomName = x.RoomName,
                FloorName = x.Floor.FloorName
            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = roomResponses;
            return response;
        }

        public async Task<BaseResponse<RoomResponse>> GetRoomById(Guid id)
        {
            BaseResponse<RoomResponse> response = new BaseResponse<RoomResponse>();
            Room room = await _context.rooms.Include(x=>x.Floor).Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if(room == null)
            {
                response.IsSuccess=false;
                response.message = "Dữ liệu không tồn tại";
                response.data = null;
                return response;
            }
            RoomResponse roomResponse = new RoomResponse();
            roomResponse.Id = room.Id;
            roomResponse.RoomName = room.RoomName;
            roomResponse.FloorName = room.Floor.FloorName;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = roomResponse;
            return response;

        }

        public async Task<BaseResponse<RoomResponse>> UpdateRoom(Guid id, RoomRequest roomRequest)
        {
            BaseResponse<RoomResponse> response = new BaseResponse<RoomResponse>();
            Room room = await _context.rooms.Include(x => x.Floor).Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (room == null)
            {
                response.IsSuccess=false;
                response.message = "Dữ liệu không tồn tại";
                response.data = null;
                return response;
            }
            room.UpdateDate = DateTime.Now;
            room.RoomName = roomRequest.RoomName;
            room.FloorId = roomRequest.FloorId;
            _context.rooms.Update(room);
            await _context.SaveChangesAsync();
            RoomResponse roomResponse = new RoomResponse();
            roomResponse.Id = room.Id;
            roomResponse.RoomName = room.RoomName;
            roomResponse.FloorName = room.Floor.FloorName;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = roomResponse;
            return response;
        }
    }
}
