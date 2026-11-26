#nullable enable
using System;
using System.Windows.Forms;

namespace BankAtmSystem.GUI
{
    partial class frmBankAtmMain
    {
        private System.ComponentModel.IContainer? components = null;
        private MenuStrip menuStrip1 = null!;
        private ToolStripMenuItem mnuHeThong = null!;
        private ToolStripMenuItem mniDangXuat = null!;
        private ToolStripMenuItem mniKhoaManHinh = null!;
        private ToolStripSeparator mniSepHeThong = null!;
        private ToolStripMenuItem mniThoat = null!;
        private ToolStripMenuItem mnuQuanLyAtm = null!;
        private ToolStripMenuItem mniDanhSachAtm = null!;
        private ToolStripMenuItem mniDoiSoatCuoiNgay = null!;
        private ToolStripMenuItem mnuCuaSo = null!;
        private ToolStripMenuItem mniXepTang = null!;
        private ToolStripMenuItem mniXepNgang = null!;
        private ToolStripMenuItem mniXepDoc = null!;
        private ToolStripSeparator mniSepCuaSo = null!;
        private ToolStripMenuItem mniDongTatCa = null!;
        private StatusStrip statusStrip1 = null!;
        private ToolStripStatusLabel tslOperator = null!;
        private ToolStripStatusLabel tslCoreBanking = null!;
        private ToolStripStatusLabel tslProgressStatus = null!;
        private ToolStripProgressBar tspbMain = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            mnuHeThong = new ToolStripMenuItem();
            mniDangXuat = new ToolStripMenuItem();
            mniKhoaManHinh = new ToolStripMenuItem();
            mniSepHeThong = new ToolStripSeparator();
            mniThoat = new ToolStripMenuItem();
            mnuQuanLyAtm = new ToolStripMenuItem();
            mniDanhSachAtm = new ToolStripMenuItem();
            mniDoiSoatCuoiNgay = new ToolStripMenuItem();
            mnuCuaSo = new ToolStripMenuItem();
            mniXepTang = new ToolStripMenuItem();
            mniXepNgang = new ToolStripMenuItem();
            mniXepDoc = new ToolStripMenuItem();
            mniSepCuaSo = new ToolStripSeparator();
            mniDongTatCa = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            tslOperator = new ToolStripStatusLabel();
            tslCoreBanking = new ToolStripStatusLabel();
            tslProgressStatus = new ToolStripStatusLabel();
            tspbMain = new ToolStripProgressBar();

            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();

            // menuStrip1
            menuStrip1.Items.AddRange(new ToolStripItem[] { mnuHeThong, mnuQuanLyAtm, mnuCuaSo });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(1180, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "Menu chính";
            menuStrip1.MdiWindowListItem = mnuCuaSo;

            // mnuHeThong
            mnuHeThong.DropDownItems.AddRange(new ToolStripItem[]
            {
                mniDangXuat,
                mniKhoaManHinh,
                mniSepHeThong,
                mniThoat
            });
            mnuHeThong.Name = "mnuHeThong";
            mnuHeThong.Text = "Hệ thống";

            mniDangXuat.Name = "mniDangXuat";
            mniDangXuat.Text = "Đăng xuất";
            mniDangXuat.Click += mniDangXuat_Click;

            mniKhoaManHinh.Name = "mniKhoaManHinh";
            mniKhoaManHinh.Text = "Khóa màn hình";
            mniKhoaManHinh.Click += mniKhoaManHinh_Click;

            mniSepHeThong.Name = "mniSepHeThong";

            mniThoat.Name = "mniThoat";
            mniThoat.Text = "Thoát";
            mniThoat.Click += mniThoat_Click;

            // mnuQuanLyAtm
            mnuQuanLyAtm.DropDownItems.AddRange(new ToolStripItem[]
            {
                mniDanhSachAtm,
                mniDoiSoatCuoiNgay
            });
            mnuQuanLyAtm.Name = "mnuQuanLyAtm";
            mnuQuanLyAtm.Text = "Quản lý ATM";

            mniDanhSachAtm.Name = "mniDanhSachAtm";
            mniDanhSachAtm.Text = "Danh sách Trạm ATM & Tồn quỹ";
            mniDanhSachAtm.Click += mniDanhSachAtm_Click;

            mniDoiSoatCuoiNgay.Name = "mniDoiSoatCuoiNgay";
            mniDoiSoatCuoiNgay.Text = "Đối soát Giao dịch Cuối ngày";
            mniDoiSoatCuoiNgay.Click += mniDoiSoatCuoiNgay_Click;

            // mnuCuaSo
            mnuCuaSo.DropDownItems.AddRange(new ToolStripItem[]
            {
                mniXepTang,
                mniXepNgang,
                mniXepDoc,
                mniSepCuaSo,
                mniDongTatCa
            });
            mnuCuaSo.Name = "mnuCuaSo";
            mnuCuaSo.Text = "Cửa sổ";

            mniXepTang.Name = "mniXepTang";
            mniXepTang.Text = "Xếp tầng (Cascade)";
            mniXepTang.Click += mniXepTang_Click;

            mniXepNgang.Name = "mniXepNgang";
            mniXepNgang.Text = "Xếp ngang (Tile Horizontal)";
            mniXepNgang.Click += mniXepNgang_Click;

            mniXepDoc.Name = "mniXepDoc";
            mniXepDoc.Text = "Xếp dọc (Tile Vertical)";
            mniXepDoc.Click += mniXepDoc_Click;

            mniSepCuaSo.Name = "mniSepCuaSo";

            mniDongTatCa.Name = "mniDongTatCa";
            mniDongTatCa.Text = "Đóng tất cả";
            mniDongTatCa.Click += mniDongTatCa_Click;

            // statusStrip1
            statusStrip1.Items.AddRange(new ToolStripItem[]
            {
                tslOperator,
                tslCoreBanking,
                tslProgressStatus,
                tspbMain
            });
            statusStrip1.Location = new System.Drawing.Point(0, 718);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(1180, 22);
            statusStrip1.TabIndex = 1;

            tslOperator.BorderSides = ToolStripStatusLabelBorderSides.Right;
            tslOperator.Name = "tslOperator";
            tslOperator.Text = "GDV trực: Phan Công Thành Đạt";

            tslCoreBanking.BorderSides = ToolStripStatusLabelBorderSides.Right;
            tslCoreBanking.Name = "tslCoreBanking";
            tslCoreBanking.Text = "Core Banking: Đang kiểm tra...";

            tslProgressStatus.Name = "tslProgressStatus";
            tslProgressStatus.Spring = true;
            tslProgressStatus.Text = "Sẵn sàng";
            tslProgressStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            tspbMain.Maximum = 100;
            tspbMain.Minimum = 0;
            tspbMain.Name = "tspbMain";
            tspbMain.Size = new System.Drawing.Size(200, 16);
            tspbMain.Style = ProgressBarStyle.Continuous;
            tspbMain.Value = 0;

            // frmBankAtmMain
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1280, 780);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Name = "frmBankAtmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 6.6 - Hệ thống MDI Điều hành Mạng lưới Cây ATM Ngân hàng";
            Load += frmBankAtmMain_Load;

            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
