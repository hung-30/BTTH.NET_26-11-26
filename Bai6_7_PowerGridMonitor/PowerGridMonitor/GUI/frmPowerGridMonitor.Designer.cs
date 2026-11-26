#nullable enable
using System.Windows.Forms;

namespace PowerGridMonitor.GUI
{
    partial class frmPowerGridMonitor
    {
        private System.ComponentModel.IContainer? components = null;
        private GroupBox grpDieuKhien = null!;
        private Label lblTram = null!;
        private ComboBox cboSubstation = null!;
        private Button btnStartScada = null!;
        private Button btnStopScada = null!;
        private Button btnLoadHistory = null!;
        private GroupBox grpThongSo = null!;
        private Label lblVoltageTitle = null!;
        private Label lblVoltage = null!;
        private Label lblLoadTitle = null!;
        private Label lblLoadPercent = null!;
        private Label lblStationDetail = null!;
        private GroupBox grpNhatKySuCo = null!;
        private DataGridView dgvGridLogs = null!;
        private StatusStrip statusStrip1 = null!;
        private ToolStripStatusLabel tslConnection = null!;
        private ToolStripStatusLabel tslLastPacket = null!;
        private ToolStripStatusLabel tslCycleCount = null!;

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
            lblTram = new Label();
            cboSubstation = new ComboBox();
            btnStartScada = new Button();
            btnStopScada = new Button();
            btnLoadHistory = new Button();
            grpThongSo = new GroupBox();
            lblVoltageTitle = new Label();
            lblVoltage = new Label();
            lblLoadTitle = new Label();
            lblLoadPercent = new Label();
            lblStationDetail = new Label();
            grpNhatKySuCo = new GroupBox();
            dgvGridLogs = new DataGridView();
            statusStrip1 = new StatusStrip();
            tslConnection = new ToolStripStatusLabel();
            tslLastPacket = new ToolStripStatusLabel();
            tslCycleCount = new ToolStripStatusLabel();

            grpDieuKhien.SuspendLayout();
            grpThongSo.SuspendLayout();
            grpNhatKySuCo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGridLogs).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();

            // grpDieuKhien
            grpDieuKhien.Dock = DockStyle.Top;
            grpDieuKhien.Location = new System.Drawing.Point(10, 10);
            grpDieuKhien.Size = new System.Drawing.Size(960, 78);
            grpDieuKhien.TabIndex = 0;
            grpDieuKhien.TabStop = false;
            grpDieuKhien.Text = "Khu vực Điều khiển SCADA";

            lblTram.AutoSize = true;
            lblTram.Location = new System.Drawing.Point(16, 33);
            lblTram.Text = "Chọn Trạm biến áp:";

            cboSubstation.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSubstation.Location = new System.Drawing.Point(145, 29);
            cboSubstation.Size = new System.Drawing.Size(270, 23);
            cboSubstation.SelectedIndexChanged += cboSubstation_SelectedIndexChanged;

            btnStartScada.Location = new System.Drawing.Point(432, 27);
            btnStartScada.Size = new System.Drawing.Size(185, 30);
            btnStartScada.Text = "Bắt đầu Giám sát SCADA";
            btnStartScada.UseVisualStyleBackColor = true;
            btnStartScada.Click += btnStartScada_Click;

            btnStopScada.Enabled = false;
            btnStopScada.Location = new System.Drawing.Point(628, 27);
            btnStopScada.Size = new System.Drawing.Size(130, 30);
            btnStopScada.Text = "Ngắt Kết nối";
            btnStopScada.UseVisualStyleBackColor = true;
            btnStopScada.Click += btnStopScada_Click;

            btnLoadHistory.Location = new System.Drawing.Point(770, 27);
            btnLoadHistory.Size = new System.Drawing.Size(165, 30);
            btnLoadHistory.Text = "Tải lịch sử từ CSDL";
            btnLoadHistory.UseVisualStyleBackColor = true;
            btnLoadHistory.Click += btnLoadHistory_Click;

            grpDieuKhien.Controls.AddRange(new Control[]
            {
                lblTram, cboSubstation, btnStartScada, btnStopScada, btnLoadHistory
            });

            // grpThongSo
            grpThongSo.Dock = DockStyle.Top;
            grpThongSo.Location = new System.Drawing.Point(10, 88);
            grpThongSo.Size = new System.Drawing.Size(960, 128);
            grpThongSo.TabIndex = 1;
            grpThongSo.TabStop = false;
            grpThongSo.Text = "Bảng thông số trực quan thời gian thực (Real-time Dashboard)";

            lblVoltageTitle.AutoSize = true;
            lblVoltageTitle.Location = new System.Drawing.Point(18, 28);
            lblVoltageTitle.Text = "Điện áp hiện tại (Ngưỡng 108 kV - 115 kV):";

            lblVoltage.AutoSize = true;
            lblVoltage.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblVoltage.Location = new System.Drawing.Point(18, 52);
            lblVoltage.Text = "--- kV";

            lblLoadTitle.AutoSize = true;
            lblLoadTitle.Location = new System.Drawing.Point(390, 28);
            lblLoadTitle.Text = "Mức mang tải (Ngưỡng an toàn <= 85%):";

            lblLoadPercent.AutoSize = true;
            lblLoadPercent.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblLoadPercent.Location = new System.Drawing.Point(390, 52);
            lblLoadPercent.Text = "--- %";

            lblStationDetail.AutoSize = true;
            lblStationDetail.Location = new System.Drawing.Point(18, 96);
            lblStationDetail.Text = "Trạm đang chọn: (Chưa kết nối)";

            grpThongSo.Controls.AddRange(new Control[]
            {
                lblVoltageTitle, lblVoltage, lblLoadTitle, lblLoadPercent, lblStationDetail
            });

            // grpNhatKySuCo
            grpNhatKySuCo.Dock = DockStyle.Fill;
            grpNhatKySuCo.Location = new System.Drawing.Point(10, 216);
            grpNhatKySuCo.Size = new System.Drawing.Size(960, 380);
            grpNhatKySuCo.TabIndex = 2;
            grpNhatKySuCo.TabStop = false;
            grpNhatKySuCo.Text = "Nhật ký sự cố Quá tải (> 85%) - Tự động ghi nhận qua Thread Safety";

            dgvGridLogs.AllowUserToAddRows = false;
            dgvGridLogs.AllowUserToDeleteRows = false;
            dgvGridLogs.Dock = DockStyle.Fill;
            dgvGridLogs.MultiSelect = false;
            dgvGridLogs.ReadOnly = true;
            dgvGridLogs.RowHeadersVisible = false;
            dgvGridLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grpNhatKySuCo.Controls.Add(dgvGridLogs);

            // statusStrip1
            statusStrip1.Items.AddRange(new ToolStripItem[]
            {
                tslConnection, tslLastPacket, tslCycleCount
            });
            statusStrip1.Location = new System.Drawing.Point(0, 598);
            statusStrip1.Size = new System.Drawing.Size(980, 22);

            tslConnection.BorderSides = ToolStripStatusLabelBorderSides.Right;
            tslConnection.Text = "SCADA: Chưa kết nối";

            tslLastPacket.BorderSides = ToolStripStatusLabelBorderSides.Right;
            tslLastPacket.Text = "Gói tin cuối: --:--:--";

            tslCycleCount.Spring = true;
            tslCycleCount.Text = "Tổng chu kỳ đã quét: 0";
            tslCycleCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // frmPowerGridMonitor
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(980, 620);
            Controls.Add(grpNhatKySuCo);
            Controls.Add(grpThongSo);
            Controls.Add(grpDieuKhien);
            Controls.Add(statusStrip1);
            Name = "frmPowerGridMonitor";
            Padding = new Padding(10, 10, 10, 0);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bài 6.7 - Trung tâm Điều độ & Giám sát Lưới điện Thời gian thực (SCADA)";
            Load += frmPowerGridMonitor_Load;
            FormClosing += frmPowerGridMonitor_FormClosing;

            grpDieuKhien.ResumeLayout(false);
            grpDieuKhien.PerformLayout();
            grpThongSo.ResumeLayout(false);
            grpThongSo.PerformLayout();
            grpNhatKySuCo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvGridLogs).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
