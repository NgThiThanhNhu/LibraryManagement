﻿using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary.Kho;
using DoAnCuoiKy.Model.Request.KhoRequest;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Model.Response.KhoResponse;
using DoAnCuoiKy.Service.IService.InformationLibrary.Kho;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary.Kho
{
    public class BookShelfService : IBookShelfService
    {
        private readonly ApplicationDbContext _context;
        public BookShelfService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<BookShelfResponse>> AddBookShelf(BookShelfRequest bookShelfRequest)
        {
            BaseResponse<BookShelfResponse> response = new BaseResponse<BookShelfResponse>();
            BookShelf bookShelf = new BookShelf();
            bookShelf.Id = Guid.NewGuid();
            bookShelf.BookShelfName = bookShelfRequest.BookShelfName;
            bookShelf.NumberOfShelves = bookShelfRequest.NumberOfShelves;
            _context.bookShelves.AddRange(bookShelf);
            await _context.SaveChangesAsync();
            if (bookShelf == null)
            {
                response.IsSuccess = false;
                response.message = "Thêm thất bại";
                return response;
            }
            BookShelfResponse bookShelfResponse = new BookShelfResponse();
            bookShelfResponse.Id = bookShelf.Id;
            bookShelfResponse.BookShelfName = bookShelf.BookShelfName;
            bookShelfResponse.NumberOfShelves = bookShelf.NumberOfShelves;
            response.IsSuccess = true;
            response.message = "Thêm thành công";
            response.data = bookShelfResponse;
            return response;

        }

        public async Task<BaseResponse<BookShelfResponse>> DeleteBookShelf(Guid id)
        {
            BaseResponse<BookShelfResponse> response = new BaseResponse<BookShelfResponse>();
            BookShelf bookShelf = await _context.bookShelves.Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (bookShelf == null)
            { 
                response.IsSuccess = false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            bookShelf.IsDeleted = true;
            _context.bookShelves.Update(bookShelf);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.message = "Xóa dữ liệu thành công";
            return response;
        }

        public async Task<BaseResponse<List<BookShelfResponse>>> GetAllBookShelf()
        {
            BaseResponse<List<BookShelfResponse>> response = new BaseResponse<List<BookShelfResponse>>();
            List<BookShelfResponse> bookShelves = await _context.bookShelves.Where(x=>x.IsDeleted == false).Select(x => new BookShelfResponse
            {
                Id = x.Id,
                BookShelfName = x.BookShelfName,
                NumberOfShelves = x.NumberOfShelves,
            }).ToListAsync();
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = bookShelves;
            return response;
        }

        public async Task<BaseResponse<BookShelfResponse>> GetBookShelfById(Guid id)
        {
            BaseResponse<BookShelfResponse> response = new BaseResponse<BookShelfResponse>();
            BookShelf bookShelf = await _context.bookShelves.Where(x=>x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if(bookShelf == null)
            {
                response.IsSuccess=false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            BookShelfResponse bookShelfResponse = new BookShelfResponse();
            bookShelfResponse.Id = bookShelf.Id;
            bookShelfResponse.BookShelfName = bookShelf.BookShelfName;
            bookShelfResponse.NumberOfShelves = bookShelf.NumberOfShelves;
            response.IsSuccess = true;
            response.message = "Lấy dữ liệu thành công";
            response.data = bookShelfResponse;
            return response;
        }

        public async Task<BaseResponse<BookShelfResponse>> UpdateBookShelf(Guid id, BookShelfRequest bookShelfRequest)
        {
            BaseResponse<BookShelfResponse> response =  new BaseResponse<BookShelfResponse>();
            BookShelf bookShelf = await _context.bookShelves.Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
            if (bookShelf == null)
            {
                response.IsSuccess=false;
                response.message = "Dữ liệu không tồn tại";
                return response;
            }
            bookShelf.BookShelfName = bookShelfRequest.BookShelfName;
            bookShelf.NumberOfShelves = bookShelfRequest.NumberOfShelves;
            _context.bookShelves.UpdateRange(bookShelf);
            await _context.SaveChangesAsync();
            if (bookShelf == null)
            {
                response.IsSuccess=false;
                response.message = "Cập nhật thất bại";
                return response;
            }
            BookShelfResponse bookShelfResponse= new BookShelfResponse();
            bookShelfResponse.Id = bookShelf.Id;
            bookShelfResponse.BookShelfName = bookShelf.BookShelfName;
            bookShelfResponse.NumberOfShelves = bookShelf.NumberOfShelves;
            response.IsSuccess = true;
            response.message = "Cập nhật thành công";
            response.data = bookShelfResponse;
            return response;

        }
    }
}
