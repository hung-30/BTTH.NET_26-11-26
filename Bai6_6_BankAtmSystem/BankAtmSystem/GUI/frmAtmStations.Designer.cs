#nullable enable
using System.Windows.Forms;

namespace BankAtmSystem.GUI
{
    partial class frmAtmStations
    {
        private System.ComponentModel.IContainer? components = null;
        private GroupBox grpThongTin = null!;
        private Label lblMaTram = null!;
        private TextBox txtMaTram = null!;
        private Label lblDiaChi = null!;
        private TextBox txtDiaChi = null!;
        private Label lblTonQuy = null!;
        private NumericUpDown nudTonQuy = null!;
        private Label lblTrangThai = null!;
        private ComboBox cboTrangThai = null!;
        private Label lblAnh = null!;
        private TextBox txtAnhHienTruong = null!;
        private Button btnChonAnh = null!;
        private PictureBox picHienTruong = null!;
        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnNhapMoi = null!;
        private GroupBox grpDanhSach = null!;
        private Label lblTimKiem = null!;
        private TextBox txtTimKiem = null!;
        private Button btnTimKiem = null!;
        private Button btnTaiLai = null!;
        private DataGridView dgvAtmStations = null!;
        private OpenFileDialog openFileDialogAnh = null!;

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
            grpThongTin = new GroupBox();
            lblMaTram = new Label();
            txtMaTram = new TextBox();
            lblDiaChi = new Label();
            txtDiaChi = new TextBox();
            lblTonQuy = new Label();
            nudTonQuy = new NumericUpDown();
            lblTrangThai = new Label();
            cboTrangThai = new ComboBox();
            lblAnh = new Label();
            txtAnhHienTruong = new TextBox();
            btnChonAnh = new Button();
            picHienTruong = new PictureBox();
            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnNhapMoi = new Button();
            grpDanhSach = new GroupBox();
            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            btnTimKiem = new Button();
            btnTaiLai = new Button();
            dgvAtmStations = new DataGridView();
            openFileDialogAnh = new OpenFileDialog();

            grpThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudTonQuy).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHienTruong).BeginInit();
            grpDanhSach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAtmStations).BeginInit();
            SuspendLayout();

            // grpThongTin (trái)
            grpThongTin.Dock = DockStyle.Left;
            grpThongTin.Location = new System.Drawing.Point(8, 8);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new System.Drawing.Size(340, 534);
            grpThongTin.TabIndex = 0;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin Trạm ATM && Ảnh hiện trường";

            lblMaTram.AutoSize = true;
            lblMaTram.Location = new System.Drawing.Point(14, 28);
            lblMaTram.Text = "Mã trạm ATM:";

            txtMaTram.Location = new System.Drawing.Point(14, 48);
            txtMaTram.Size = new System.Drawing.Size(310, 23);

            lblDiaChi.AutoSize = true;
            lblDiaChi.Location = new System.Drawing.Point(14, 80);
            lblDiaChi.Text = "Địa chỉ đặt máy:";

            txtDiaChi.Location = new System.Drawing.Point(14, 100);
            txtDiaChi.Size = new System.Drawing.Size(310, 23);

            lblTonQuy.AutoSize = true;
            lblTonQuy.Location = new System.Drawing.Point(14, 132);
            lblTonQuy.Text = "Số tiền tồn quỹ hiện tại (VNĐ):";

            nudTonQuy.Increment = 1000000m;
            nudTonQuy.Location = new System.Drawing.Point(14, 152);
            nudTonQuy.Maximum = 100000000000m;
            nudTonQuy.Minimum = 0m;
            nudTonQuy.Size = new System.Drawing.Size(310, 23);
            nudTonQuy.ThousandsSeparator = true;

            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new System.Drawing.Point(14, 184);
            lblTrangThai.Text = "Trạng thái hoạt động:";

            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangThai.Items.AddRange(new object[]
            {
                "Hoạt động",
                "Tồn quỹ thấp",
                "Bảo trì",
                "Ngưng hoạt động"
            });
            cboTrangThai.Location = new System.Drawing.Point(14, 204);
            cboTrangThai.Size = new System.Drawing.Size(310, 23);

            lblAnh.AutoSize = true;
            lblAnh.Location = new System.Drawing.Point(14, 238);
            lblAnh.Text = "Ảnh chụp hiện trường trạm ATM:";

            txtAnhHienTruong.Location = new System.Drawing.Point(14, 258);
            txtAnhHienTruong.ReadOnly = true;
            txtAnhHienTruong.Size = new System.Drawing.Size(218, 23);

            btnChonAnh.Location = new System.Drawing.Point(238, 257);
            btnChonAnh.Size = new System.Drawing.Size(86, 25);
            btnChonAnh.Text = "Chọn ảnh...";
            btnChonAnh.UseVisualStyleBackColor = true;
            btnChonAnh.Click += btnChonAnh_Click;

            picHienTruong.BorderStyle = BorderStyle.FixedSingle;
            picHienTruong.Location = new System.Drawing.Point(14, 290);
            picHienTruong.Size = new System.Drawing.Size(310, 165);
            picHienTruong.SizeMode = PictureBoxSizeMode.Zoom;
            picHienTruong.TabStop = false;

            btnThem.Location = new System.Drawing.Point(14, 472);
            btnThem.Size = new System.Drawing.Size(72, 30);
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;

            btnSua.Location = new System.Drawing.Point(92, 472);
            btnSua.Size = new System.Drawing.Size(72, 30);
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;

            btnXoa.Location = new System.Drawing.Point(170, 472);
            btnXoa.Size = new System.Drawing.Size(72, 30);
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;

            btnNhapMoi.Location = new System.Drawing.Point(248, 472);
            btnNhapMoi.Size = new System.Drawing.Size(76, 30);
            btnNhapMoi.Text = "Nhập mới";
            btnNhapMoi.UseVisualStyleBackColor = true;
            btnNhapMoi.Click += btnNhapMoi_Click;

            grpThongTin.Controls.AddRange(new Control[]
            {
                lblMaTram, txtMaTram,
                lblDiaChi, txtDiaChi,
                lblTonQuy, nudTonQuy,
                lblTrangThai, cboTrangThai,
                lblAnh, txtAnhHienTruong, btnChonAnh,
                picHienTruong,
                btnThem, btnSua, btnXoa, btnNhapMoi
            });

            // grpDanhSach (phải)
            grpDanhSach.Dock = DockStyle.Fill;
            grpDanhSach.Location = new System.Drawing.Point(348, 8);
            grpDanhSach.Name = "grpDanhSach";
            grpDanhSach.Size = new System.Drawing.Size(604, 534);
            grpDanhSach.TabIndex = 1;
            grpDanhSach.TabStop = false;
            grpDanhSach.Text = "Danh sách Trạm ATM && Tồn quỹ";

            lblTimKiem.AutoSize = true;
            lblTimKiem.Location = new System.Drawing.Point(14, 28);
            lblTimKiem.Text = "Tìm kiếm:";

            txtTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTimKiem.Location = new System.Drawing.Point(80, 25);
            txtTimKiem.Size = new System.Drawing.Size(330, 23);

            btnTimKiem.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTimKiem.Location = new System.Drawing.Point(417, 24);
            btnTimKiem.Size = new System.Drawing.Size(85, 25);
            btnTimKiem.Text = "Tìm";
            btnTimKiem.UseVisualStyleBackColor = true;
            btnTimKiem.Click += btnTimKiem_Click;

            btnTaiLai.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTaiLai.Location = new System.Drawing.Point(508, 24);
            btnTaiLai.Size = new System.Drawing.Size(82, 25);
            btnTaiLai.Text = "Làm mới";
            btnTaiLai.UseVisualStyleBackColor = true;
            btnTaiLai.Click += btnTaiLai_Click;

            dgvAtmStations.AllowUserToAddRows = false;
            dgvAtmStations.AllowUserToDeleteRows = false;
            dgvAtmStations.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAtmStations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAtmStations.Location = new System.Drawing.Point(14, 60);
            dgvAtmStations.MultiSelect = false;
            dgvAtmStations.ReadOnly = true;
            dgvAtmStations.RowHeadersVisible = false;
            dgvAtmStations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAtmStations.Size = new System.Drawing.Size(576, 458);
            dgvAtmStations.SelectionChanged += dgvAtmStations_SelectionChanged;

            grpDanhSach.Controls.AddRange(new Control[]
            {
                lblTimKiem, txtTimKiem, btnTimKiem, btnTaiLai, dgvAtmStations
            });

            // openFileDialogAnh
            openFileDialogAnh.Filter = "Hình ảnh (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|Tất cả tệp (*.*)|*.*";
            openFileDialogAnh.Title = "Chọn ảnh chụp hiện trường trạm ATM";

            // frmAtmStations
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(960, 550);
            Controls.Add(grpDanhSach);
            Controls.Add(grpThongTin);
            Name = "frmAtmStations";
            Padding = new Padding(8);
            Text = "Quản lý Danh sách Trạm ATM & Tồn quỹ";
            Load += frmAtmStations_Load;

            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudTonQuy).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHienTruong).EndInit();
            grpDanhSach.ResumeLayout(false);
            grpDanhSach.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAtmStations).EndInit();
            ResumeLayout(false);
        }
    }
}
