namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class Publisher : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? PublisherName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<Book>? Books { get; set; }
    }
}
