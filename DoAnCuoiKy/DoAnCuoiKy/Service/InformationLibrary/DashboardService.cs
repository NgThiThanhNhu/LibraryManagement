using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<List<CategoryDistributionResponse>>> GetCategoryDistrubute()
        {
            BaseResponse<List<CategoryDistributionResponse>> response = new BaseResponse<List<CategoryDistributionResponse>>();
            List<CategoryDistributionResponse> categoryDistributionResponses = await _context.books.Include(x=>x.Category).GroupBy(b => b.Category.Name).Select(g => new CategoryDistributionResponse
            {
               CategoryName = g.Key,
               BookCount = g.Count()
            }).OrderByDescending(c => c.BookCount).ToListAsync();
            response.IsSuccess = true;
            response.message = "Thống kê theo thể loại thành công";
            response.data = categoryDistributionResponses;
            return response;
        }

        public async Task<BaseResponse<DashboardResponse>> GetCurrentInformations()
        {
            BaseResponse<DashboardResponse> response = new BaseResponse<DashboardResponse>();
            DashboardResponse dashboardResponse = new DashboardResponse();
            dashboardResponse.TotalBook = await _context.books.CountAsync();
            dashboardResponse.ToTalBookItem = await _context.bookItems.CountAsync();
            dashboardResponse.ToTalUser = await _context.librarians.Where(x=>x.RoleId.ToString() == "27A91ADE-F847-4D20-AE11-EDDCB0D96D27").CountAsync();
            dashboardResponse.TotalComment = await _context.userBookInteractions.Where(x=>x.InteractionType == Model.Enum.UserBehavior.InteractionType.RateAndReview).CountAsync();
            dashboardResponse.ToTalReadingSession = await _context.userReadingSessions.CountAsync();
            response.IsSuccess = true;
            response.message = "Lấy thông tin thống kê tổng quát thành công";
            response.data = dashboardResponse;
            return response;
        }

        public async Task<BaseResponse<List<MostCommentBookResponse>>> GetMostCommentedBook()
        {
            BaseResponse<List<MostCommentBookResponse>> response = new BaseResponse<List<MostCommentBookResponse>>();
            List<MostCommentBookResponse> mostCommentedBooks = new List<MostCommentBookResponse>();
            var topCommentedBook = await _context.userBookInteractions.Where(x => x.InteractionType == Model.Enum.UserBehavior.InteractionType.RateAndReview).GroupBy(x => x.BookId).Select(g => new
            {
                BookId = g.Key,
                CommentCount = g.Count()
            }).OrderByDescending(x=>x.CommentCount).Take(8).ToListAsync();
            foreach (var item in topCommentedBook)
            {
                Book bookInfomation = await _context.books.Include(x=>x.BookAuthor).Include(x=>x.bookFiles).FirstOrDefaultAsync(x => x.Id == item.BookId);
                MostCommentBookResponse mostCommentBookResponse = new MostCommentBookResponse();
                mostCommentBookResponse.BookId = item.BookId;
                mostCommentBookResponse.Title = bookInfomation.Title;
                mostCommentBookResponse.Author = bookInfomation.BookAuthor.Name;
                mostCommentBookResponse.CommentCount = item.CommentCount;
                mostCommentBookResponse.ImageUrl = bookInfomation.bookFiles.Select(x => x.ImageUrl).First();

                mostCommentedBooks.Add(mostCommentBookResponse);
            }
            response.IsSuccess = true;
            response.message = "Lấy sách sôi nổi thành công";
            response.data = mostCommentedBooks;
            return response;
        }
    }
}
