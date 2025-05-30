using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnCuoiKy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }
        [HttpPost("AddPublisher")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponse<PublisherResponse>> addPublisher(PublisherRequest publisherRequest)
        {
            BaseResponse<PublisherResponse> response = await _publisherService.AddPublisher(publisherRequest);
            return response;
        }
        [HttpGet("GetAllPublisher")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponse<List<PublisherResponse>>> getAllPublisher()
        {
            BaseResponse<List<PublisherResponse>> response = await _publisherService.GetAllPublisher();
            return response;
        }
        [HttpGet("GetPublisherById/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponse<PublisherResponse>> getPublisherById(Guid id)
        {
            BaseResponse<PublisherResponse> response = await _publisherService.GetPublisherById(id);
            return response;
        }
        [HttpPost("UpdatePublisher/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<BaseResponse<PublisherResponse>> updatePublisher(Guid id, PublisherRequest publisherRequest)
        {
            BaseResponse<PublisherResponse> response = await _publisherService.UpdatePublisher(id, publisherRequest);
            return response;
        }
        [HttpPost("DeletePublisher/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BaseResponse<PublisherResponse>> deletePublisher(Guid id)
        {
            BaseResponse<PublisherResponse> response = await _publisherService.DeletePublisher(id);
            return response;
        }
    }
}
