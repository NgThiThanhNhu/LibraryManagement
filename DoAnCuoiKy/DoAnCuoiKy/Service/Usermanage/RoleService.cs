using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.Usermanage;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.Usermanage
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        public RoleService(ApplicationDbContext context) {
            _context = context; 
        }
        public async Task<BaseResponse<RoleResponse>> AddRole(RoleRequest roleRequest)
        {
            BaseResponse<RoleResponse> response = new BaseResponse<RoleResponse>();
            Role role = new Role();
            RoleResponse roleResponse = new RoleResponse();
            role.Name = roleRequest.Name;
            _context.AddAsync(role);
            await _context.SaveChangesAsync();
            if (role == null) {
            response.IsSuccess = false;
            response.message = "Thêm thất bại";
            return response;
            }

            roleResponse.Name = role.Name;
            response.IsSuccess = true;
            response.message = "Thêm thành công";
            response.data = roleResponse;
            return response;
        }


        public async Task<BaseResponse<List<RoleResponse>>> GetAllRole()
        {
            BaseResponse<List<RoleResponse>> response = new BaseResponse<List<RoleResponse>>();
            List<RoleResponse> roleResponses = new List<RoleResponse>();
            List<Role> roles = await _context.roles.Where(x=>x.IsDeleted == false).ToListAsync();
            foreach (var role in roles) { 
                RoleResponse roleResponse = new RoleResponse();
                roleResponse.Id = role.Id;
                roleResponse.Name = role.Name;
                roleResponses.Add(roleResponse);
            }
            if(roleResponses == null)
            {
                response.IsSuccess = false;
                response.message = "Không có dữ liệu hiển thị";
                return response;
            }
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = roleResponses;
            return response;

        }
    }
}
