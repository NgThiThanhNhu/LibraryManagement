using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.UserBehavior;

namespace DoAnCuoiKy.Model.Entities.UserBehavior
{
    public class BookRecommendation : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Librarian User { get; set; }  
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public decimal Score { get; set; }              
        public int Rank { get; set; }                  
        public string Reason { get; set; }              
        public RecommendationType RecommendationType { get; set; }
        public DateTime? ExpiresAt { get; set; }        
        public string ModelVersion { get; set; }
        public bool IsClicked { get; set; } = false;          
        public DateTime? ClickedAt { get; set; }
        public bool IsViewed { get; set; } = false;             
        public DateTime? ViewedAt { get; set; }
        public bool IsBorrowed { get; set; } = false;            
        public DateTime? BorrowedAt { get; set; }       
        public bool IsExpired() => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.Now;
    }
}
