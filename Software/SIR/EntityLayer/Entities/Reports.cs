using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities {
    public class DailyPoint {
        public System.DateTime Day { get; set; }
        public int Value { get; set; }
    }

    public class BreakdownRow {
        public string Name { get; set; }
        public int Value { get; set; }
        public double Percent { get; set; }
        public string ValueText { get { return Value.ToString(); } }

        public BreakdownRow() {
            Name = "";
        }
    }

    public class ActivityRow {
        public System.DateTime CreatedAt { get; set; }
        public string Type { get; set; }
        public string ProductName { get; set; }
        public int QuantityDelta { get; set; }
        public string ReferenceId { get; set; }
        public string Notes { get; set; }

        public ActivityRow() {
            Type = "";
            ProductName = "";
            ReferenceId = "";
            Notes = "";
        }
    }

    public class StatisticsSummary {
        public int TotalOrders { get; set; }
        public int IncomingUnits { get; set; }
        public int OutgoingUnits { get; set; }
        public double AvgLeadTimeDays { get; set; }
        public int LowStockCount { get; set; }

        public double TotalOrdersDeltaPct { get; set; }
        public double IncomingUnitsDeltaPct { get; set; }
        public double OutgoingUnitsDeltaPct { get; set; }
        public double AvgLeadTimeDeltaPct { get; set; }
        public double LowStockDeltaPct { get; set; }

        public List<DailyProductQty> IncomingByDayByProduct { get; set; }
        public List<DailyProductQty> OutgoingByDayByProduct { get; set; }

        public List<DailyPoint> IncomingByDay { get; set; }
        public List<DailyPoint> OutgoingByDay { get; set; }
        public List<BreakdownRow> TopSuppliers { get; set; }
        public List<BreakdownRow> TopProductsExport { get; set; }
        public List<ActivityRow> Activity { get; set; }

        public StatisticsSummary() {
            IncomingByDay = new List<DailyPoint>();
            OutgoingByDay = new List<DailyPoint>();
            TopSuppliers = new List<BreakdownRow>();
            TopProductsExport = new List<BreakdownRow>();
            Activity = new List<ActivityRow>();

            IncomingByDayByProduct = new List<DailyProductQty>();
            OutgoingByDayByProduct = new List<DailyProductQty>();
        }

    }
}
