using EntityLayer.Entities;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BusinessLogicLayer {
    public class EmailNotificationService {
        public async Task<bool> SendLowStockEmailAsync(Product p, bool isCritical) {
            System.Diagnostics.Debug.WriteLine("AlertEmailEnabled raw = " + ConfigurationManager.AppSettings["AlertEmailEnabled"]);

            if (p == null) return false;
            if (!ReadBool("AlertEmailEnabled", false)) return false;

            var to = ConfigurationManager.AppSettings["AlertEmailTo"];
            var fromEmail = ConfigurationManager.AppSettings["AlertEmailFrom"];

            if (string.IsNullOrWhiteSpace(to) || string.IsNullOrWhiteSpace(fromEmail))
                return false;

            var host = ConfigurationManager.AppSettings["SmtpHost"];
            var port = ReadInt("SmtpPort", 587);
            var user = ConfigurationManager.AppSettings["SmtpUser"];
            var pass = ConfigurationManager.AppSettings["SmtpPass"];
            var ssl = ReadBool("SmtpEnableSsl", true);

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(pass))
                return false;

            var subject = isCritical
                ? $"[CRITICAL] Stock is EMPTY: {p.Name}"
                : $"[WARNING] Low stock: {p.Name}";

            var body =
                $@"Product: {p.Name}
                Code: {p.ProductCode}

                Current qty: {p.CurrentQuantity}
                Minimum: {p.ReorderLevel}
                Missing to min: {p.MissingToMinimum}

                Last restock: {p.LastRestockAgo}
                Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            System.Diagnostics.Debug.WriteLine(
                "AlertEmailEnabled = " + ConfigurationManager.AppSettings["AlertEmailEnabled"]
            );


            try {
                var html = BuildLowStockHtml(p, isCritical);
                using (var msg = new MailMessage()) {
                    msg.From = new MailAddress(fromEmail, "Warehouse Alerts");
                    msg.To.Add(to);
                    msg.Subject = subject;
                    msg.IsBodyHtml = true;
                    msg.Body = html;

                    using (var client = new SmtpClient(host, port)) {
                        client.EnableSsl = ssl;
                        client.Credentials = new NetworkCredential(user, pass);
                        await client.SendMailAsync(msg);
                        return true;
                    }
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        private static int ReadInt(string key, int fallback)
            => int.TryParse(ConfigurationManager.AppSettings[key], out var v) ? v : fallback;

        private static bool ReadBool(string key, bool fallback)
            => bool.TryParse(ConfigurationManager.AppSettings[key], out var v) ? v : fallback;
        private static string BuildLowStockHtml(Product p, bool isCritical) {
            string severity = isCritical ? "CRITICAL" : "WARNING";
            string badgeBg = isCritical ? "#FEE2E2" : "#FEF3C7";
            string badgeFg = isCritical ? "#B91C1C" : "#92400E";
            string title = isCritical ? "Stock is empty" : "Low stock detected";

            string productName = WebUtility.HtmlEncode(p.Name ?? "");
            string productCode = WebUtility.HtmlEncode(p.ProductCode ?? "");
            string supplier = "—";
            string lastRestock = WebUtility.HtmlEncode(p.LastRestockAgo ?? "—");

            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int qty = p.CurrentQuantity;
            int min = p.ReorderLevel;
            int missing = p.MissingToMinimum;

            return $@"
            <!doctype html>
            <html>
            <head>
              <meta charset='utf-8' />
              <meta name='viewport' content='width=device-width, initial-scale=1.0' />
              <title>{severity} - {productName}</title>
            </head>
            <body style='margin:0;padding:0;background:#f6f7fb;font-family:Segoe UI, Arial, sans-serif;color:#111827;'>
              <div style='max-width:640px;margin:0 auto;padding:24px;'>
    
                <div style='padding:18px 20px;border-radius:12px;background:#ffffff;border:1px solid #e5e7eb;'>
                  <div style='display:flex;align-items:center;justify-content:space-between;gap:12px;'>
                    <div style='font-size:18px;font-weight:700;line-height:1.2;'>{title}</div>
                    <div style='font-size:12px;font-weight:700;padding:6px 10px;border-radius:999px;background:{badgeBg};color:{badgeFg};border:1px solid #e5e7eb;'>
                      {severity}
                    </div>
                  </div>

                  <div style='margin-top:10px;color:#6b7280;font-size:13px;line-height:1.5;'>
                    This is an automated notification from <b>Warehouse Alerts</b>.
                  </div>

                  <div style='margin-top:18px;border-top:1px solid #e5e7eb;padding-top:16px;'>
                    <div style='font-size:16px;font-weight:700;'>{productName}</div>
                    <div style='color:#6b7280;font-size:13px;margin-top:4px;'>
                      Code: <b>{productCode}</b> &nbsp;•&nbsp; Supplier: <b>{supplier}</b>
                    </div>

                    <div style='margin-top:14px;display:flex;gap:10px;flex-wrap:wrap;'>
                      <div style='background:#f9fafb;border:1px solid #e5e7eb;border-radius:10px;padding:10px 12px;min-width:160px;'>
                        <div style='color:#6b7280;font-size:12px;'>Current quantity</div>
                        <div style='font-size:20px;font-weight:800;margin-top:2px;'>{qty}</div>
                      </div>

                      <div style='background:#f9fafb;border:1px solid #e5e7eb;border-radius:10px;padding:10px 12px;min-width:160px;'>
                        <div style='color:#6b7280;font-size:12px;'>Minimum level</div>
                        <div style='font-size:20px;font-weight:800;margin-top:2px;'>{min}</div>
                      </div>

                      <div style='background:#f9fafb;border:1px solid #e5e7eb;border-radius:10px;padding:10px 12px;min-width:160px;'>
                        <div style='color:#6b7280;font-size:12px;'>Missing to minimum</div>
                        <div style='font-size:20px;font-weight:800;margin-top:2px;'>{missing}</div>
                      </div>
                    </div>

                    <div style='margin-top:14px;color:#6b7280;font-size:13px;'>
                      Last restock: <b style='color:#111827;'>{lastRestock}</b><br/>
                      Time: <b style='color:#111827;'>{time}</b>
                    </div>
                  </div>

                  <div style='margin-top:18px;border-top:1px solid #e5e7eb;padding-top:14px;color:#9ca3af;font-size:12px;line-height:1.4;'>
                    If you believe you received this message in error, you can ignore it.
                  </div>
                </div>

                <div style='text-align:center;margin-top:14px;color:#9ca3af;font-size:12px;'>
                  © {DateTime.Now:yyyy} Warehouse Alerts
                </div>

              </div>
            </body>
            </html>";
        }

    }

}
