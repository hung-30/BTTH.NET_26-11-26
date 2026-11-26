using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BankAtmSystem.BUS;
using BankAtmSystem.DTO;

namespace BankAtmSystem.GUI
{
    public partial class frmAtmStations : Form
    {
        private readonly AtmStationBUS _bus = new AtmStationBUS();
        private bool _isLoadingGrid = false;

        public frmAtmStations()
        {
            InitializeComponent();
            ConfigureGrid();
        }

        private void ConfigureGrid()
        {
            dgvAtmStations.AutoGenerateColumns = false;
            dgvAtmStations.Columns.Clear();

            dgvAtmStations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMaTram",
                HeaderText = "Mã trạm",
                DataPropertyName = nameof(AtmStationDTO.MaTram),
                FillWeight = 18
            });
            dgvAtmStations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDiaChi",
                HeaderText = "Địa chỉ đặt máy",
                DataPropertyName = nameof(AtmStationDTO.DiaChi),
                FillWeight = 38
            });
            dgvAtmStations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTonQuy",
                HeaderText = "Tồn quỹ (VNĐ)",
                DataPropertyName = nameof(AtmStationDTO.TonQuy),
                FillWeight = 20,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvAtmStations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTrangThai",
                HeaderText = "Trạng thái",
                DataPropertyName = nameof(AtmStationDTO.TrangThai),
                FillWeight = 16
            });
            dgvAtmStations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colAnh",
                HeaderText = "Ảnh hiện trường",
                DataPropertyName = nameof(AtmStationDTO.AnhHienTruong),
                Visible = false
            });
            dgvAtmStations.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNgayCapNhat",
                HeaderText = "Cập nhật",
                DataPropertyName = nameof(AtmStationDTO.NgayCapNhat),
                FillWeight = 18,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });
        }

        private void frmAtmStations_Load(object? sender, EventArgs e)
        {
            if (cboTrangThai.Items.Count > 0)
                cboTrangThai.SelectedIndex = 0;
            LoadStations();
        }

        public void LoadStations(string? keyword = null)
        {
            try
            {
                _isLoadingGrid = true;
                var list = _bus.GetAll(keyword);
                dgvAtmStations.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách trạm ATM: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoadingGrid = false;
            }

            if (dgvAtmStations.Rows.Count > 0)
            {
                SelectRow(0);
            }
            else
            {
                ClearInputFields();
            }
        }

        public void SelectRow(int index)
        {
            if (index >= 0 && index < dgvAtmStations.Rows.Count)
            {
                dgvAtmStations.ClearSelection();
                dgvAtmStations.Rows[index].Selected = true;
                PopulateFromSelectedRow();
            }
        }

        public void AttachPhotoPath(string filePath)
        {
            txtAnhHienTruong.Text = filePath;
            DisplayImageSafe(filePath);
        }

        private void DisplayImageSafe(string? path)
        {
            if (picHienTruong.Image != null)
            {
                picHienTruong.Image.Dispose();
                picHienTruong.Image = null;
            }

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return;

            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                using var ms = new MemoryStream(bytes);
                using var img = Image.FromStream(ms);
                picHienTruong.Image = new Bitmap(img);
            }
            catch
            {
                picHienTruong.Image = null;
            }
        }

        private void PopulateFromSelectedRow()
        {
            if (dgvAtmStations.SelectedRows.Count == 0) return;
            if (dgvAtmStations.SelectedRows[0].DataBoundItem is not AtmStationDTO item) return;

            txtMaTram.Text = item.MaTram;
            txtMaTram.ReadOnly = true;
            txtDiaChi.Text = item.DiaChi;
            nudTonQuy.Value = Math.Clamp(item.TonQuy, nudTonQuy.Minimum, nudTonQuy.Maximum);

            int idx = cboTrangThai.Items.IndexOf(item.TrangThai);
            cboTrangThai.SelectedIndex = idx >= 0 ? idx : 0;

            txtAnhHienTruong.Text = item.AnhHienTruong ?? string.Empty;
            DisplayImageSafe(item.AnhHienTruong);
        }

        private void ClearInputFields()
        {
            txtMaTram.ReadOnly = false;
            txtMaTram.Clear();
            txtDiaChi.Clear();
            nudTonQuy.Value = 0;
            if (cboTrangThai.Items.Count > 0)
                cboTrangThai.SelectedIndex = 0;
            txtAnhHienTruong.Clear();
            DisplayImageSafe(null);
            txtMaTram.Focus();
        }

        private AtmStationDTO BuildDtoFromInputs()
        {
            return new AtmStationDTO
            {
                MaTram = txtMaTram.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim(),
                TonQuy = nudTonQuy.Value,
                TrangThai = cboTrangThai.SelectedItem?.ToString() ?? "Hoạt động",
                AnhHienTruong = string.IsNullOrWhiteSpace(txtAnhHienTruong.Text) ? null : txtAnhHienTruong.Text.Trim()
            };
        }

        private void dgvAtmStations_SelectionChanged(object? sender, EventArgs e)
        {
            if (_isLoadingGrid) return;
            PopulateFromSelectedRow();
        }

        private void btnChonAnh_Click(object? sender, EventArgs e)
        {
            if (openFileDialogAnh.ShowDialog(this) == DialogResult.OK)
            {
                AttachPhotoPath(openFileDialogAnh.FileName);
            }
        }

        private void btnThem_Click(object? sender, EventArgs e)
        {
            string error = _bus.Add(BuildDtoFromInputs());
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Thêm trạm ATM thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadStations(txtTimKiem.Text);
        }

        private void btnSua_Click(object? sender, EventArgs e)
        {
            string error = _bus.Update(BuildDtoFromInputs());
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Cập nhật thông tin trạm ATM thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadStations(txtTimKiem.Text);
        }

        private void btnXoa_Click(object? sender, EventArgs e)
        {
            string maTram = txtMaTram.Text.Trim();
            if (string.IsNullOrEmpty(maTram))
            {
                MessageBox.Show("Vui lòng chọn trạm ATM cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn xóa trạm '{maTram}'?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string error = _bus.Delete(maTram);
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Xóa trạm ATM thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadStations(txtTimKiem.Text);
        }

        private void btnNhapMoi_Click(object? sender, EventArgs e)
        {
            dgvAtmStations.ClearSelection();
            ClearInputFields();
        }

        private void btnTimKiem_Click(object? sender, EventArgs e)
        {
            LoadStations(txtTimKiem.Text);
        }

        private void btnTaiLai_Click(object? sender, EventArgs e)
        {
            txtTimKiem.Clear();
            LoadStations();
        }
    }
}
