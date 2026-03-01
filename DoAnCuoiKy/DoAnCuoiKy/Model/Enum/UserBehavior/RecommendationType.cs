namespace DoAnCuoiKy.Model.Enum.UserBehavior
{
    public enum RecommendationType : byte
    {
        MatrixFactorization = 1,    // ML-based (main)
        ContentBased = 2,           // Dựa trên content
        Collaborative = 3,          // Collaborative filtering
        Hybrid = 4,                 // Kết hợp nhiều phương pháp
        Popular = 5,                // Sách phổ biến
        Trending = 6,               // Trending
        NewArrival = 7,             // Sách mới
        SameAuthor = 8,             // Cùng tác giả
        SameCategory = 9            // Cùng thể loại
    }
}
