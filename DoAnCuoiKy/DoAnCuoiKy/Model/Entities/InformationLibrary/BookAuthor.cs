﻿namespace DoAnCuoiKy.Model.Entities.InformationLibrary
{
    public class BookAuthor : BaseEntity
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public List<Book>? books { get; set; }
    }
}
