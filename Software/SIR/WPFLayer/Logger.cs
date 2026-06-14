using System;
using System.IO;
using System.Text;
using System.Threading;

namespace WPFLayer {
    internal static class Logger {
        private static readonly object _sync = new object();
        private static string _logPath;

        public static void Initialize() {
            try {
                var appDir = AppDomain.CurrentDomain.BaseDirectory;
                var logsDir = Path.Combine(appDir, "logs");
                if (!Directory.Exists(logsDir)) Directory.CreateDirectory(logsDir);
                _logPath = Path.Combine(logsDir, "app.log");
            } catch {
                _logPath = "app.log";
            }
        }

        public static void Log(string message) {
            try {
                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] INFO: {message}";
                WriteLine(line);
            } catch { }
        }

        public static void LogError(string context, Exception ex) {
            try {
                var sb = new StringBuilder();
                sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ERROR: {context}");
                sb.AppendLine(ex.ToString());
                WriteLine(sb.ToString());
            } catch { }
        }

        private static void WriteLine(string text) {
            lock (_sync) {
                try {
                    File.AppendAllText(_logPath, text + Environment.NewLine);
                } catch { }
            }
        }

        public static string ReadAll() {
            lock (_sync) {
                try {
                    if (!File.Exists(_logPath)) return string.Empty;
                    return File.ReadAllText(_logPath);
                } catch {
                    return string.Empty;
                }
            }
        }
    }
}
