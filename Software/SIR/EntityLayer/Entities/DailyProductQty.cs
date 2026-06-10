using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities {
    public class DailyProductQty {
        public System.DateTime Day { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }

        public DailyProductQty() {
            ProductName = "";
        }
    }
}

