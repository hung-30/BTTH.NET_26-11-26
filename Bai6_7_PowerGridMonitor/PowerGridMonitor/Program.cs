using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using PowerGridMonitor.GUI;

namespace PowerGridMonitor
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            if (args.Length > 0 && args[0] == "--capture")
            {
                try
                {
                    RunCaptureAndVerify();
                }
                catch (Exception ex)
                {
                    string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "capture_error.txt");
                    File.WriteAllText(logPath, ex.ToString());
                    Console.Error.WriteLine(ex.ToString());
                    Environment.Exit(1);
                }
                return;
            }

            Application.Run(new frmPowerGridMonitor());
        }

        private static void RunCaptureAndVerify()
        {
            // Bật kiểm tra nghiêm ngặt Cross-thread giống chế độ Debug (F5) trên Visual Studio
            Control.CheckForIllegalCrossThreadCalls = true;

            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));
            string imgDir = Path.Combine(projectRoot, "images");
            Directory.CreateDirectory(imgDir);

            using var form = new frmPowerGridMonitor();
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(60, 60);
            form.Show();
            PumpEvents(400);

            // 1. Giao diện ban đầu trước khi kết nối SCADA
            SaveForm(form, Path.Combine(imgDir, "01_scada_initial.png"));

            // 2. Bắt đầu giám sát SCADA trên Trạm 110kV Hà Đông (lặp thật 2 giây/lần)
            form.StartMonitoring();

            bool capturedGreen = false;
            bool capturedRed = false;
            DateTime deadline = DateTime.UtcNow.AddSeconds(40);

            while ((!capturedGreen || !capturedRed) && DateTime.UtcNow < deadline)
            {
                PumpEvents(250);
                var sample = form.LastTelemetry;
                if (sample == null) continue;

                if (!sample.IsOverload && !capturedGreen)
                {
                    capturedGreen = true;
                    SaveForm(form, Path.Combine(imgDir, "02_scada_normal_green.png"));
                }
                else if (sample.IsOverload && !capturedRed)
                {
                    capturedRed = true;
                    SaveForm(form, Path.Combine(imgDir, "03_scada_overload_red_log.png"));
                }
            }

            // 4. Chuyển sang Trạm 220kV Chèm (ngắt kết nối -> chọn trạm khác -> giám sát tiếp)
            form.StopMonitoring();
            PumpEvents(250);
            form.SelectSubstationByIndex(1); // Trạm 220kV Chèm
            form.StartMonitoring();
            PumpEvents(2400);
            SaveForm(form, Path.Combine(imgDir, "04_scada_station_switch.png"));

            // 5. Ngắt kết nối SCADA an toàn & giữ lịch sử sự cố
            form.StopMonitoring();
            PumpEvents(300);
            SaveForm(form, Path.Combine(imgDir, "05_scada_disconnected_history.png"));

            form.Close();
            Console.WriteLine($"SUCCESS: Captured Bai 6.7 screenshots (green={capturedGreen}, red={capturedRed}, cycles={form.CurrentCycleCount}) at {imgDir}");
        }

        private static void PumpEvents(int ms)
        {
            var end = DateTime.UtcNow.AddMilliseconds(ms);
            while (DateTime.UtcNow < end)
            {
                Application.DoEvents();
                Thread.Sleep(30);
            }
        }

        private static void SaveForm(Form f, string path)
        {
            using var bmp = new Bitmap(f.Width, f.Height);
            f.DrawToBitmap(bmp, new Rectangle(0, 0, f.Width, f.Height));
            bmp.Save(path, ImageFormat.Png);
            Console.WriteLine("Saved: " + path);
        }
    }
}
