using BusinessLogicLayer;
using EntityLayer.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFLayer {
    public partial class Statistics : UserControl {
        private readonly StatisticsService _service = new StatisticsService();
        private string _breakdownMode = "Top suppliers";
        private string _lastTipKey = null;
        private int _tipToken = 0;
        private StatisticsSummary _currentSummary;
        private DateTime _currentFrom;
        private DateTime _currentTo;


        public Statistics() {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e) {
            cmbPeriod.ItemsSource = new[] { "Last 7 days", "Last 30 days", "Last 90 days", "Custom" };
            cmbPeriod.SelectedIndex = 0;

            cmbBreakdownMode.ItemsSource = new[] { "Top suppliers", "Top products (export)" };
            cmbBreakdownMode.SelectedIndex = 0;
            cmbChartMode.ItemsSource = new[] { "Incoming", "Outgoing", "Both" };
            cmbChartMode.SelectedIndex = 2;

            SetPreset("Last 7 days");
            await ReloadAsync();
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e) {
            await ReloadAsync();
        }

        private async void BtnApply_Click(object sender, RoutedEventArgs e) {
            cmbPeriod.SelectedItem = "Custom";
            await ReloadAsync();
        }

        private async void BtnReset_Click(object sender, RoutedEventArgs e) {
            cmbPeriod.SelectedItem = "Last 7 days";
            SetPreset("Last 7 days");
            await ReloadAsync();
        }

        private void BtnExportReport_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Export Report not implemented yet.");
        }

        private async void CmbPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cmbPeriod.SelectedItem == null) return;

            string preset = cmbPeriod.SelectedItem.ToString();
            if (preset == "Custom") return;

            SetPreset(preset);
            await ReloadAsync();
        }

        private async void CmbBreakdownMode_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cmbBreakdownMode.SelectedItem == null) return;

            _breakdownMode = cmbBreakdownMode.SelectedItem.ToString();
            txtBreakdownTitle.Text =
                _breakdownMode == "Top products (export)"
                ? "Top Products (Export)"
                : "Top Suppliers";

            await ReloadAsync();
        }

        private void SetPreset(string preset) {
            DateTime today = DateTime.Today;

            if (preset == "Last 7 days") {
                dpFrom.SelectedDate = today.AddDays(-7);
                dpTo.SelectedDate = today;
            } else if (preset == "Last 30 days") {
                dpFrom.SelectedDate = today.AddDays(-30);
                dpTo.SelectedDate = today;
            } else if (preset == "Last 90 days") {
                dpFrom.SelectedDate = today.AddDays(-90);
                dpTo.SelectedDate = today;
            }
        }

        private async Task ReloadAsync() {
            DateTime from = (dpFrom.SelectedDate ?? DateTime.Today.AddDays(-7)).Date;
            DateTime to = (dpTo.SelectedDate ?? DateTime.Today).Date;

            btnRefresh.IsEnabled = false;
            btnApply.IsEnabled = false;
            btnReset.IsEnabled = false;
            btnReport.IsEnabled = false;


            try {
                StatisticsSummary s = await _service.GetSummaryAsync(from, to);
                _currentSummary = s;
                _currentFrom = from;
                _currentTo = to;
                btnReport.IsEnabled = true;

                txtLowStock.Text = s.LowStockCount.ToString();
                txtIncoming.Text = s.IncomingUnits.ToString();
                txtOrders.Text = s.TotalOrders.ToString();
                txtLead.Text = string.Format("{0:0.0} days", s.AvgLeadTimeDays);

                DrawStackedBars(cnvChart, s.IncomingByDayByProduct, s.OutgoingByDayByProduct);

                icBreakdown.ItemsSource =
                    _breakdownMode == "Top products (export)"
                    ? (s.TopProductsExport ?? new List<BreakdownRow>())
                    : (s.TopSuppliers ?? new List<BreakdownRow>());
            } finally {
                btnRefresh.IsEnabled = true;
                btnApply.IsEnabled = true;
                btnReset.IsEnabled = true;
            }
        }

        private string _chartMode = "Both";

        private async void CmbChartMode_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (cmbChartMode.SelectedItem == null) return;
            _chartMode = cmbChartMode.SelectedItem.ToString();
            await ReloadAsync();
        }

        private void DrawStackedBars(Canvas canvas, List<DailyProductQty> incoming, List<DailyProductQty> outgoing) {
            canvas.Children.Clear();

            double W = canvas.ActualWidth; if (W < 50) W = 700;
            double H = canvas.ActualHeight; if (H < 50) H = 220;

            double L = 40, R = 10, T = 10, B = 25;
            double plotW = W - L - R;
            double plotH = H - T - B;

            var days = new HashSet<DateTime>();
            if (incoming != null) foreach (var r in incoming) days.Add(r.Day.Date);
            if (outgoing != null) foreach (var r in outgoing) days.Add(r.Day.Date);

            if (days.Count == 0) return;

            var allDays = days.OrderBy(d => d).ToList();

            var incByDay = (incoming ?? new List<DailyProductQty>())
                .GroupBy(x => x.Day.Date)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(z => z.Qty).ToList());

            var outByDay = (outgoing ?? new List<DailyProductQty>())
                .GroupBy(x => x.Day.Date)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(z => z.Qty).ToList());

            int maxTotal = 1;
            foreach (var d in allDays) {
                int incTot = incByDay.ContainsKey(d) ? incByDay[d].Sum(x => x.Qty) : 0;
                int outTot = outByDay.ContainsKey(d) ? outByDay[d].Sum(x => x.Qty) : 0;

                int cand = 0;
                if (_chartMode == "Incoming") cand = incTot;
                else if (_chartMode == "Outgoing") cand = outTot;
                else cand = Math.Max(incTot, outTot);

                if (cand > maxTotal) maxTotal = cand;
            }

            canvas.Children.Add(new Line { X1 = L, Y1 = T, X2 = L, Y2 = T + plotH, Stroke = Brushes.LightGray, StrokeThickness = 1 });
            canvas.Children.Add(new Line { X1 = L, Y1 = T + plotH, X2 = L + plotW, Y2 = T + plotH, Stroke = Brushes.LightGray, StrokeThickness = 1 });

            int n = allDays.Count;
            if (n == 0) return;

            double daySlot = plotW / n;
            double gap = 10;

            double barW = (_chartMode == "Both")
                ? Math.Max(8, (daySlot - gap) / 2.0)
                : Math.Max(10, daySlot - gap);

            Brush[] palette = new Brush[]
            {
        Brushes.SteelBlue, Brushes.MediumSeaGreen, Brushes.Orange,
        Brushes.MediumPurple, Brushes.CadetBlue, Brushes.Coral,
        Brushes.Goldenrod, Brushes.SlateBlue
            };

            Func<string, Brush> colorFor = name => {
                if (string.IsNullOrEmpty(name)) return Brushes.Gray;
                int idx = Math.Abs(name.GetHashCode()) % palette.Length;
                return palette[idx];
            };

            Action<DateTime, List<DailyProductQty>, double, string> drawOneBar = (day, segments, xLeft, typeLabel) => {
                int total = segments.Sum(s => s.Qty);
                if (total <= 0) return;

                double yBottom = T + plotH;
                double currentY = yBottom;

                int take = 6;
                var top = segments.Take(take).ToList();
                int others = segments.Skip(take).Sum(x => x.Qty);
                if (others > 0) {
                    top.Add(new DailyProductQty { Day = day, ProductName = "Others", Qty = others });
                }

                foreach (var seg in top) {
                    double segH = (plotH * seg.Qty) / maxTotal;
                    if (segH < 1) segH = 1;

                    currentY -= segH;

                    var rect = new Rectangle {
                        Width = barW,
                        Height = segH,
                        Fill = colorFor(seg.ProductName),
                        RadiusX = 2,
                        RadiusY = 2
                    };

                    Canvas.SetLeft(rect, xLeft);
                    Canvas.SetTop(rect, currentY);

                    rect.Tag = $"{day:dd.MM.yyyy} | {typeLabel}\n{seg.ProductName}: {seg.Qty}";

                    rect.MouseEnter += Segment_MouseEnter;
                    rect.MouseLeave += Segment_MouseLeave;

                    canvas.Children.Add(rect);
                }
            };

            for (int i = 0; i < n; i++) {
                DateTime day = allDays[i];

                double slotX = L + i * daySlot;
                double baseX = slotX + (daySlot - (_chartMode == "Both" ? (barW * 2 + 6) : barW)) / 2.0;

                var incSeg = incByDay.ContainsKey(day) ? incByDay[day] : new List<DailyProductQty>();
                var outSeg = outByDay.ContainsKey(day) ? outByDay[day] : new List<DailyProductQty>();

                if (_chartMode == "Incoming") {
                    drawOneBar(day, incSeg, baseX, "Incoming");
                } else if (_chartMode == "Outgoing") {
                    drawOneBar(day, outSeg, baseX, "Outgoing");
                } else 
                  {
                    drawOneBar(day, incSeg, baseX, "Incoming");
                    drawOneBar(day, outSeg, baseX + barW + 6, "Outgoing");
                }

                if (n <= 14 || i == 0 || i == n - 1 || i == n / 2) {
                    var lbl = new TextBlock { Text = day.ToString("dd.MM"), FontSize = 10, Foreground = Brushes.Gray };
                    Canvas.SetLeft(lbl, slotX + 2);
                    Canvas.SetTop(lbl, T + plotH + 5);
                    canvas.Children.Add(lbl);
                }
            }
        }
        private void Segment_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            string key = fe.Tag as string ?? "";

            _tipToken++;
            if (_lastTipKey != key) {
                _lastTipKey = key;
                txtChartTip.Text = key;
            }

            pChartTip.IsOpen = true;
        }

        private async void Segment_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            int token = ++_tipToken;

            await Task.Delay(80);

            if (token != _tipToken) return;

            pChartTip.IsOpen = false;
            _lastTipKey = null;
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e) {
            if (_currentSummary == null) {
                MessageBox.Show("No data loaded yet.");
                return;
            }

            string generatedBy = Environment.UserName; // ili tvoj login user
            var win = new ReportWindow(_currentSummary, _currentFrom, _currentTo, generatedBy);
            win.Owner = Window.GetWindow(this);
            win.ShowDialog();
        }

    }
}
