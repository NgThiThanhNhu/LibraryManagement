namespace DoAnCuoiKy.Model.Response
{
    public class MostCommentBookResponse
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int CommentCount { get; set; }
        public string ImageUrl { get; set; }
    }
}
