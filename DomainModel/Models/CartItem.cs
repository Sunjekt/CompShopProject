namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CartItem")]
    public partial class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual Product Product { get; set; }

        [NotMapped]
        public int Subtotal { get; set; }
    }
}
