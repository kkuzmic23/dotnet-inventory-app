using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities {
    public partial class Product {
        [NotMapped]
        public int CurrentQuantity => Stock != null ? Stock.Quantity : 0;

        [NotMapped]
        public bool IsLowStock => IsActive && CurrentQuantity < ReorderLevel;

        [NotMapped]
        public bool IsCriticalStock => IsActive && CurrentQuantity <= 0;

        [NotMapped]
        public int MissingToMinimum {
            get {
                if (!IsActive) return 0;

                if (CurrentQuantity >= ReorderLevel) return 0;

                return ReorderLevel - CurrentQuantity;
            }
        }

        [NotMapped]
        public string AlertMessage {
            get {
                return $"Below minimum by {MissingToMinimum}.";
            }
        }

        [NotMapped]
        public DateTime? LastRestockAt { get; set; }

        [NotMapped]
        public string LastRestockAgo {
            get {
                if (!LastRestockAt.HasValue)
                    return "No restock yet";

                var ts = DateTime.Now - LastRestockAt.Value;

                if (ts.TotalSeconds < 60) return "Restocked just now";
                if (ts.TotalMinutes < 60) return $"Restocked {Math.Floor(ts.TotalMinutes)} min ago";
                if (ts.TotalHours < 24) return $"Restocked {Math.Floor(ts.TotalHours)} h ago";
                if (ts.TotalDays < 7) return $"Restocked {Math.Floor(ts.TotalDays)} days ago";

                var weeks = Math.Floor(ts.TotalDays / 7);
                return $"Restocked {weeks} weeks ago";
            }
        }
    }
}
