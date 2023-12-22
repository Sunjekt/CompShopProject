namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserImage")]
    public partial class UserImage
    {
        public int Id { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        [StringLength(10)]
        public string FileExtension { get; set; }

        public decimal Size { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
