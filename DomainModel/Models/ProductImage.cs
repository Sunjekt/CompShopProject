namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductImage")]
    public partial class ProductImage
    {
        public int Id { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        [StringLength(10)]
        public string FileExtension { get; set; }

        public decimal Size { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
