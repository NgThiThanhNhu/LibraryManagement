namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class Location : LocationBase
    {
        public Guid Id { get; set; }
        public Guid ShelfSection {  get; set; }
        public ShelfSection Shelf { get; set; }
        public Guid BookItemId { get; set; }
        public BookItem BookItem { get; set; }
        public string Description { get; set; }

    }
}
