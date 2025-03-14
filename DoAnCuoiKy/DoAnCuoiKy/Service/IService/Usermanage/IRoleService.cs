using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.Usermanage
{
    public interface IRoleService
    {
        Task<BaseResponse<RoleResponse>> AddRole(RoleRequest roleRequest);
        Task<BaseResponse<List<RoleResponse>>> GetAllRole();

    }
}
