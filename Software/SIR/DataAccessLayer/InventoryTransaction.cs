namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class InventoryTransaction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int QuantityDelta { get; set; }

        [Required]
        [StringLength(30)]
        public string Type { get; set; }

        public int? ReferenceId { get; set; }

        [StringLength(250)]
        public string Notes { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        public virtual Product Product { get; set; }
    }
}
