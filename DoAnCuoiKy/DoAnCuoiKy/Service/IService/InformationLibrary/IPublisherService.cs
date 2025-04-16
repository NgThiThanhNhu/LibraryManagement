using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;

namespace DoAnCuoiKy.Service.IService.InformationLibrary
{
    public interface IPublisherService
    {
        Task<BaseResponse<PublisherResponse>> AddPublisher(PublisherRequest publisherRequest);
        Task<BaseResponse<List<PublisherResponse>>> GetAllPublisher();
        Task<BaseResponse<PublisherResponse>> GetPublisherById(Guid id);
        Task<BaseResponse<PublisherResponse>> UpdatePublisher(Guid id, PublisherRequest publisherRequest);
        Task<BaseResponse<PublisherResponse>> DeletePublisher(Guid id);
    }
}
