using System;
using System.Windows.Forms;
using BankAtmSystem.BUS;

namespace BankAtmSystem.GUI
{
    public partial class frmBankAtmMain : Form
    {
        private readonly CoreBankingBUS _coreBus = new CoreBankingBUS();
        public string CurrentOperator { get; private set; } = "Phan Công Thành Đạt";

        public frmBankAtmMain()
        {
            InitializeComponent();
        }

        private void frmBankAtmMain_Load(object? sender, EventArgs e)
        {
            RefreshConnectionStatus();
            UpdateOperatorDisplay();
        }

        public void RefreshConnectionStatus()
        {
            var (_, message) = _coreBus.CheckConnection();
            tslCoreBanking.Text = $"Core Banking: {message}";
        }

        private void UpdateOperatorDisplay()
        {
            tslOperator.Text = $"GDV trực: {CurrentOperator}";
        }

        /// <summary>
        /// Mở Form con MDI hoặc kích hoạt Form đang mở để tránh mở trùng lặp.
        /// </summary>
        public T OpenOrActivateChild<T>(Func<T> factory) where T : Form
        {
            foreach (Form child in this.MdiChildren)
            {
                if (child is T existing)
                {
                    if (existing.WindowState == FormWindowState.Minimized)
                        existing.WindowState = FormWindowState.Normal;
                    existing.Activate();
                    return existing;
                }
            }

            T newChild = factory();
            newChild.MdiParent = this;
            newChild.Show();
            return newChild;
        }

        public void UpdateReconciliationProgress(int percent, string statusText)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateReconciliationProgress(percent, statusText)));
                return;
            }

            tspbMain.Value = Math.Clamp(percent, 0, 100);
            tslProgressStatus.Text = statusText;
        }

        public void ResetProgress(string statusText = "Sẵn sàng")
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ResetProgress(statusText)));
                return;
            }

            tspbMain.Value = 0;
            tslProgressStatus.Text = statusText;
        }

        private void mniDanhSachAtm_Click(object? sender, EventArgs e)
        {
            OpenOrActivateChild(() => new frmAtmStations());
        }

        private void mniDoiSoatCuoiNgay_Click(object? sender, EventArgs e)
        {
            OpenOrActivateChild(() => new frmAtmReconciliation(this));
        }

        private void mniXepTang_Click(object? sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void mniXepNgang_Click(object? sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void mniXepDoc_Click(object? sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void mniDongTatCa_Click(object? sender, EventArgs e)
        {
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }
        }

        private void mniKhoaManHinh_Click(object? sender, EventArgs e)
        {
            tslProgressStatus.Text = "Màn hình đang bị khóa";
            using (var dlg = new frmLockScreen(CurrentOperator, isLogout: false))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    this.Close();
                    return;
                }
            }
            tslProgressStatus.Text = "Đã mở khóa màn hình";
        }

        private void mniDangXuat_Click(object? sender, EventArgs e)
        {
            foreach (Form child in this.MdiChildren)
            {
                child.Close();
            }

            tslOperator.Text = "GDV trực: (Chưa đăng nhập)";
            tslProgressStatus.Text = "Đã đăng xuất phiên làm việc";

            using (var dlg = new frmLockScreen(CurrentOperator, isLogout: true))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    CurrentOperator = dlg.OperatorName;
                    UpdateOperatorDisplay();
                    tslProgressStatus.Text = "Đăng nhập lại thành công";
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void mniThoat_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}
