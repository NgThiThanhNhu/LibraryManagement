using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary.Kho
{
    public class FloorService : IFloorService
    {
        private readonly ApplicationDbContext _context;
        public FloorService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<FloorResponse>> AddFloor(FloorRequest floorRequest)
        {
            BaseResponse<FloorResponse> response = new BaseResponse<FloorResponse>();
            Floor floor = new Floor();
            floor.Id = Guid.NewGuid();
            floor.FloorName = floorRequest.FloorName;
            _context.floors.Add(floor);
            await _context.SaveChangesAsync();
            FloorResponse floorResponse = new FloorResponse();
            if(floor == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm thất bại";
                return response;
            }
            floorResponse.Id = floor.Id;
            floorResponse.FloorName = floor.FloorName;  
            response.IsSuccess = true;
            response.message = "Thêm dữ liệu thành công";
            response.data = floorResponse;
            return response;


        }

        public async Task<BaseResponse<FloorResponse>> DeleteFloor(Guid id)
        {
            BaseResponse<FloorResponse> response = new BaseResponse<FloorResponse>();
            Floor floor = await _context.floors.Where(f=>f.IsDeleted == false && f.Id == id).FirstOrDefaultAsync();
            if(floor == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            floor.IsDeleted = true;
            _context.floors.Update(floor);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa dữ liệu thành công";
            return response;
        }

        public async Task<BaseResponse<List<FloorResponse>>> GetAllFloor()
        {
            BaseResponse<List<FloorResponse>> response = new BaseResponse<List<FloorResponse>>();
            List<FloorResponse> floorResponses = await _context.floors.Where(f => f.IsDeleted == false).Select(f=> new FloorResponse
            {
                Id = f.Id,
                FloorName = f.FloorName
            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy danh sách thành công";
            response.data= floorResponses;
            return response;
        }

        public async Task<BaseResponse<FloorResponse>> GetFloorById(Guid id)
        {
            BaseResponse<FloorResponse> response = new BaseResponse<FloorResponse>();
            Floor floor = await _context.floors.Where(f => f.IsDeleted == false && f.Id == id).FirstOrDefaultAsync();
            if (floor == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            FloorResponse floorResponse = new FloorResponse();
            floorResponse.Id = floor.Id;
            floorResponse.FloorName = floor.FloorName;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = floorResponse;
            return response;
        }

        public async Task<BaseResponse<FloorResponse>> UpdateFloor(Guid id, FloorRequest floorRequest)
        {
            BaseResponse<FloorResponse> response = new BaseResponse<FloorResponse>();
            Floor floor = await _context.floors.Where(f => f.IsDeleted == false && f.Id == id).FirstOrDefaultAsync();
            if(floor == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            floor.FloorName = floorRequest.FloorName;
            _context.floors.Update(floor);
            await _context.SaveChangesAsync();
            FloorResponse floorResponse = new FloorResponse();
            floorResponse.Id = floor.Id;
            floorResponse.FloorName= floor.FloorName;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = floorResponse;
            return response;
        }
    }
}
