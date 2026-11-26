using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BankAtmSystem.GUI;

namespace BankAtmSystem
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

            Application.Run(new frmBankAtmMain());
        }

        private static void RunCaptureAndVerify()
        {
            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));
            string imgDir = Path.Combine(projectRoot, "images");
            Directory.CreateDirectory(imgDir);

            // Tạo ảnh mẫu hiện trường ATM để kiểm thử đính kèm qua OpenFileDialog
            string samplePhotoPath = Path.Combine(projectRoot, "Database", "sample_atm_site.png");
            CreateSampleAtmSiteImage(samplePhotoPath);

            using var main = new frmBankAtmMain();
            main.StartPosition = FormStartPosition.Manual;
            main.Location = new Point(40, 40);
            main.Show();
            PumpEvents(300);

            // 1. Màn hình chính MDI parent
            SaveForm(main, Path.Combine(imgDir, "01_mdi_main.png"));

            // 2. Mở Form Con 1 (frmAtmStations) + đính kèm ảnh hiện trường + test chống mở trùng lặp
            var frmStations = main.OpenOrActivateChild(() => new frmAtmStations());
            frmStations.WindowState = FormWindowState.Maximized;
            PumpEvents(300);
            frmStations.SelectRow(0);
            frmStations.AttachPhotoPath(samplePhotoPath);
            PumpEvents(250);
            SaveForm(main, Path.Combine(imgDir, "02_atm_stations_crud_photo.png"));

            // Kiểm tra mở trùng lặp: gọi lại OpenOrActivateChild phải giữ đúng 1 MdiChild frmAtmStations
            var frmStationsAgain = main.OpenOrActivateChild(() => new frmAtmStations());
            if (!ReferenceEquals(frmStations, frmStationsAgain) || main.MdiChildren.Length != 1)
            {
                throw new InvalidOperationException("MDI duplicate check failed!");
            }

            // 3. Mở thêm Form Con 2 (frmAtmReconciliation) & sắp xếp TileHorizontal để hiển thị đầy đủ các cột
            frmStations.WindowState = FormWindowState.Normal;
            var frmRecon = main.OpenOrActivateChild(() => new frmAtmReconciliation(main));
            PumpEvents(250);
            main.LayoutMdi(MdiLayout.TileHorizontal);
            PumpEvents(300);
            SaveForm(main, Path.Combine(imgDir, "03_mdi_multiwindow_tile.png"));

            // 4. Test bất đồng bộ async/await + ProgressBar trên MDI parent trong lúc UI vẫn phản hồi
            frmStations.WindowState = FormWindowState.Minimized;
            frmRecon.WindowState = FormWindowState.Maximized;
            frmRecon.Activate();
            PumpEvents(200);

            Task reconTask = frmRecon.StartReconciliationAsync();
            // Đợi ~2.4 giây (khoảng 48% - 52% tiến độ) và kiểm tra UI Thread vẫn phản hồi
            PumpForMilliseconds(2400);
            // Gọi OpenOrActivateChild trong lúc đối soát đang chạy (chứng minh MDI menu không bị đơ), giữ frmRecon ở trên
            var checkStations = main.OpenOrActivateChild(() => new frmAtmStations());
            checkStations.WindowState = FormWindowState.Minimized;
            frmRecon.WindowState = FormWindowState.Maximized;
            frmRecon.Activate();
            PumpEvents(150);
            SaveForm(main, Path.Combine(imgDir, "04_async_reconciliation_running.png"));

            // Chờ đối soát hoàn tất (~5s tổng cộng)
            while (!reconTask.IsCompleted)
            {
                Application.DoEvents();
                Thread.Sleep(50);
            }
            reconTask.GetAwaiter().GetResult();
            PumpEvents(300);
            SaveForm(main, Path.Combine(imgDir, "05_reconciliation_completed.png"));

            // Test bổ sung CancellationTokenSource (bắt đầu rồi hủy sau 600ms)
            Task cancelTask = frmRecon.StartReconciliationAsync();
            PumpForMilliseconds(600);
            frmRecon.CancelReconciliation();
            while (!cancelTask.IsCompleted)
            {
                Application.DoEvents();
                Thread.Sleep(50);
            }
            cancelTask.GetAwaiter().GetResult();
            PumpEvents(200);

            main.Close();
            Console.WriteLine("SUCCESS: Captured and verified all 5 screenshots for Bai 6.6 at " + imgDir);
        }

        private static void PumpEvents(int ms)
        {
            Application.DoEvents();
            Thread.Sleep(ms);
            Application.DoEvents();
        }

        private static void PumpForMilliseconds(int totalMs)
        {
            var end = DateTime.UtcNow.AddMilliseconds(totalMs);
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

        private static void CreateSampleAtmSiteImage(string path)
        {
            using var bmp = new Bitmap(420, 240);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.WhiteSmoke);
            using var pen = new Pen(Color.DimGray, 2);
            using var fontTitle = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            using var fontSub = new Font("Segoe UI", 9F, FontStyle.Regular);

            g.DrawRectangle(pen, 20, 16, 380, 208);
            g.DrawRectangle(pen, 145, 52, 130, 138);
            g.DrawRectangle(pen, 165, 68, 90, 55);
            g.DrawRectangle(pen, 170, 138, 80, 20);
            g.DrawString("TRẠM ATM - ẢNH HIỆN TRƯỜNG", fontTitle, Brushes.Black, 92, 24);
            g.DrawString("Mã trạm: ATM-HN-001 (198 Trần Quang Khải)", fontSub, Brushes.Black, 76, 198);
            bmp.Save(path, ImageFormat.Png);
        }
    }
}
