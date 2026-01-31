namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            InventoryTransactions = new HashSet<InventoryTransaction>();
            OrderHasProducts = new HashSet<OrderHasProduct>();
            StockExportHasProducts = new HashSet<StockExportHasProduct>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public int? SupplierId { get; set; }

        public int ReorderLevel { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderHasProduct> OrderHasProducts { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockExportHasProduct> StockExportHasProducts { get; set; }

        public virtual Stock Stock { get; set; }
    }
}
