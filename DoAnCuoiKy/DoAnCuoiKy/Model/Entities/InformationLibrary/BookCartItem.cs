namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookCartItem : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public BookCart BookCart { get; set; }
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
