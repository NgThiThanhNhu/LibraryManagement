using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class PublisherService : IPublisherService
    {
        private readonly ApplicationDbContext _context;
        public PublisherService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<PublisherResponse>> AddPublisher(PublisherRequest publisherRequest)
        {
            BaseResponse<PublisherResponse> response = new BaseResponse<PublisherResponse>();
            Publisher publisher = new Publisher();
            publisher.Id = Guid.NewGuid();
            publisher.PublisherName = publisherRequest.PublisherName;
            publisher.Phone = publisherRequest.Phone;
            publisher.Email = publisherRequest.Email;
            publisher.Address = publisherRequest.Address;
            await _context.publishers.AddAsync(publisher);
            await _context.SaveChangesAsync();
            if (publisher == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm thất bại";
                return response;
            }
            PublisherResponse publisherResponse = new PublisherResponse();
            publisherResponse.Id = publisher.Id;
            publisherResponse.PublisherName = publisher.PublisherName;
            publisherResponse.Phone = publisher.Phone;
            publisherResponse.Email = publisher.Email;
            publisherResponse.Address = publisher.Address;
            response.IsSuccess = true;
            response.message = "Thêm thành công";
            response.data = publisherResponse;
            return response;
        }

       

        public async Task<BaseResponse<PublisherResponse>> DeletePublisher(Guid id)
        {
            BaseResponse<PublisherResponse> response = new BaseResponse<PublisherResponse>();
            Publisher publisher = await _context.publishers.Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (publisher == null) 
            { 
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            publisher.IsDeleted = true;
            _context.publishers.Update(publisher);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa dữ liệu thành công";
            return response;
        }

        public async Task<BaseResponse<List<PublisherResponse>>> GetAllPublisher()
        {
            BaseResponse<List<PublisherResponse>> response = new BaseResponse<List<PublisherResponse>>();
            List<PublisherResponse> publisherResponses = await _context.publishers.Where(x=>x.IsDeleted == false).Select(x=> new PublisherResponse
            {
                Id = x.Id,
                PublisherName = x.PublisherName,
                Phone = x.Phone,
                Email = x.Email,
                Address = x.Address,
            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy list thành công";
            response.data = publisherResponses;
            return response;
        }

        public async Task<BaseResponse<PublisherResponse>> GetPublisherById(Guid id)
        {
            BaseResponse<PublisherResponse> response = new BaseResponse<PublisherResponse>();
            Publisher publisher = await _context.publishers.Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if(publisher == null)
            {
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            PublisherResponse publisherResponse = new PublisherResponse();
            publisherResponse.Id = id;
            publisherResponse.PublisherName = publisher.PublisherName;
            publisherResponse.Phone = publisher.Phone;
            publisherResponse.Email = publisher.Email;
            publisherResponse.Address = publisher.Address;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = publisherResponse;
            return response;
        }

        public async Task<BaseResponse<PublisherResponse>> UpdatePublisher(Guid id, PublisherRequest publisherRequest)
        {
            BaseResponse<PublisherResponse> response = new BaseResponse<PublisherResponse>();
            Publisher publisher = await _context.publishers.Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (publisher == null)
            { 
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            publisher.PublisherName = publisherRequest.PublisherName;
            publisher.Phone = publisherRequest.Phone;
            publisher.Email = publisherRequest.Email;
            publisher.Address = publisherRequest.Address;
            _context.publishers.UpdateRange(publisher);
            await _context.SaveChangesAsync();
            if(publisher == null)
            {
                response.IsSuccess = false;
                response.message = "Cập nhật thất bại";
                return response;
            }
            PublisherResponse publisherResponse = new PublisherResponse();
            publisherResponse.Id = publisher.Id;
            publisherResponse.PublisherName = publisher.PublisherName;
            publisherResponse.Phone = publisher.Phone;
            publisherResponse.Email = publisher.Email;
            publisherResponse.Address = publisher.Address;
            response.IsSuccess = true;
            response.message = "Cập nhật dữ liệu thành công";
            response.data = publisherResponse;
            return response;
        }

        
    }
}
