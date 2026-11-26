#nullable enable
using System.Windows.Forms;

namespace BankAtmSystem.GUI
{
    partial class frmAtmReconciliation
    {
        private System.ComponentModel.IContainer? components = null;
        private GroupBox grpDieuKhien = null!;
        private Button btnRunReconciliation = null!;
        private Button btnStopReconciliation = null!;
        private Button btnResetPending = null!;
        private Label lblProgressInfo = null!;
        private Label lblSummary = null!;
        private GroupBox grpGiaoDich = null!;
        private DataGridView dgvTransactions = null!;
        private GroupBox grpLichSu = null!;
        private DataGridView dgvHistory = null!;

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
            grpDieuKhien = new GroupBox();
            btnRunReconciliation = new Button();
            btnStopReconciliation = new Button();
            btnResetPending = new Button();
            lblProgressInfo = new Label();
            lblSummary = new Label();
            grpGiaoDich = new GroupBox();
            dgvTransactions = new DataGridView();
            grpLichSu = new GroupBox();
            dgvHistory = new DataGridView();

            grpDieuKhien.SuspendLayout();
            grpGiaoDich.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).BeginInit();
            grpLichSu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHistory).BeginInit();
            SuspendLayout();

            // grpDieuKhien
            grpDieuKhien.Dock = DockStyle.Top;
            grpDieuKhien.Location = new System.Drawing.Point(8, 8);
            grpDieuKhien.Size = new System.Drawing.Size(904, 108);
            grpDieuKhien.TabIndex = 0;
            grpDieuKhien.TabStop = false;
            grpDieuKhien.Text = "Điều khiển Đối soát Giao dịch Bất đồng bộ (Async/Await && CancellationTokenSource)";

            btnRunReconciliation.Location = new System.Drawing.Point(16, 22);
            btnRunReconciliation.Size = new System.Drawing.Size(185, 30);
            btnRunReconciliation.Text = "Chạy Đối Soát Tự Động";
            btnRunReconciliation.UseVisualStyleBackColor = true;
            btnRunReconciliation.Click += btnRunReconciliation_Click;

            btnStopReconciliation.Enabled = false;
            btnStopReconciliation.Location = new System.Drawing.Point(212, 22);
            btnStopReconciliation.Size = new System.Drawing.Size(150, 30);
            btnStopReconciliation.Text = "Dừng Đối Soát";
            btnStopReconciliation.UseVisualStyleBackColor = true;
            btnStopReconciliation.Click += btnStopReconciliation_Click;

            btnResetPending.Location = new System.Drawing.Point(374, 22);
            btnResetPending.Size = new System.Drawing.Size(165, 30);
            btnResetPending.Text = "Đặt lại trạng thái chờ";
            btnResetPending.UseVisualStyleBackColor = true;
            btnResetPending.Click += btnResetPending_Click;

            lblProgressInfo.AutoSize = true;
            lblProgressInfo.Location = new System.Drawing.Point(16, 60);
            lblProgressInfo.Text = "Trạng thái: Sẵn sàng thực hiện đối soát (200.000 giao dịch từ Core Banking trong 4 - 6 giây).";

            lblSummary.AutoSize = true;
            lblSummary.Location = new System.Drawing.Point(16, 82);
            lblSummary.Text = "Kết quả: Chưa chạy đối soát trong phiên hiện tại.";

            grpDieuKhien.Controls.AddRange(new Control[]
            {
                btnRunReconciliation, btnStopReconciliation, btnResetPending, lblProgressInfo, lblSummary
            });

            // grpLichSu (bottom)
            grpLichSu.Dock = DockStyle.Bottom;
            grpLichSu.Location = new System.Drawing.Point(8, 427);
            grpLichSu.Size = new System.Drawing.Size(904, 115);
            grpLichSu.TabIndex = 2;
            grpLichSu.TabStop = false;
            grpLichSu.Text = "Lịch sử các đợt Đối soát Cuối ngày";

            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistory.Dock = DockStyle.Fill;
            dgvHistory.MultiSelect = false;
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grpLichSu.Controls.Add(dgvHistory);

            // grpGiaoDich (fill)
            grpGiaoDich.Dock = DockStyle.Fill;
            grpGiaoDich.Location = new System.Drawing.Point(8, 126);
            grpGiaoDich.Size = new System.Drawing.Size(904, 240);
            grpGiaoDich.TabIndex = 1;
            grpGiaoDich.TabStop = false;
            grpGiaoDich.Text = "Danh sách Giao dịch ATM cần đối chiếu trong ngày";

            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.Dock = DockStyle.Fill;
            dgvTransactions.MultiSelect = false;
            dgvTransactions.ReadOnly = true;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grpGiaoDich.Controls.Add(dgvTransactions);

            // frmAtmReconciliation
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(920, 550);
            Controls.Add(grpGiaoDich);
            Controls.Add(grpLichSu);
            Controls.Add(grpDieuKhien);
            Name = "frmAtmReconciliation";
            Padding = new Padding(8);
            Text = "Đối soát Giao dịch ATM Cuối ngày";
            Load += frmAtmReconciliation_Load;
            FormClosing += frmAtmReconciliation_FormClosing;

            grpDieuKhien.ResumeLayout(false);
            grpDieuKhien.PerformLayout();
            grpGiaoDich.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).EndInit();
            grpLichSu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHistory).EndInit();
            ResumeLayout(false);
        }
    }
}
