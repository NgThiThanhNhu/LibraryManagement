namespace DoAnCuoiKy.Model.Entities.InformationLibrary.Kho
{
    public class BookShelf
    {
        public Guid Id { get; set; }
        public string BookShelfName { get; set; }
        public List<Shelf> Shelves { get; set; }
    }
}
