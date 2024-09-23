using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoilerplateProject.Entities.Products
{
    [Table("ProductPictures")]
    public class ProductPicture : Entity, ISoftDelete
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public bool IsDeleted { get; set; }
    }
}