namespace DomainModel.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            CartItem = new HashSet<CartItem>();
            OrderItem = new HashSet<OrderItem>();
            ProductImage = new HashSet<ProductImage>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int Price { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public double Rate { get; set; }

        public DateTime CreationDate { get; set; }

        public int Quantity { get; set; }

        public int ProducerId { get; set; }

        public int CategoryId { get; set; }

        public DateTime? Deleted_at { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartItem> CartItem { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItem { get; set; }

        public virtual Producer Producer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductImage> ProductImage { get; set; }

        [NotMapped]
        public IEnumerable<string> Pathes { get; set; }
        [NotMapped]
        public byte[] ImageBytes { get; set; }
        [NotMapped]
        public string CurrentRateSource { get; set; }
        [NotMapped]
        public string Color { get; set; }
    }
}
