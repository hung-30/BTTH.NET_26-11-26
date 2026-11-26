using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BankAtmSystem.BUS;
using BankAtmSystem.DTO;

namespace BankAtmSystem.GUI
{
    public partial class frmAtmReconciliation : Form
    {
        private readonly frmBankAtmMain? _mainForm;
        private readonly ReconciliationBUS _bus = new ReconciliationBUS();
        private CancellationTokenSource? _cts;
        private bool _isRunning = false;

        public frmAtmReconciliation() : this(null)
        {
        }

        public frmAtmReconciliation(frmBankAtmMain? mainForm)
        {
            _mainForm = mainForm;
            InitializeComponent();
            ConfigureGrids();
        }

        private void ConfigureGrids()
        {
            dgvTransactions.AutoGenerateColumns = false;
            dgvTransactions.Columns.Clear();
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã GD", DataPropertyName = nameof(AtmTransactionDTO.MaGiaoDich), FillWeight = 10 });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã trạm", DataPropertyName = nameof(AtmTransactionDTO.MaTram), FillWeight = 15 });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Số thẻ", DataPropertyName = nameof(AtmTransactionDTO.SoTheMasked), FillWeight = 20 });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Loại GD", DataPropertyName = nameof(AtmTransactionDTO.LoaiGiaoDich), FillWeight = 15 });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Số tiền (VNĐ)",
                DataPropertyName = nameof(AtmTransactionDTO.SoTien),
                FillWeight = 15,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Thời gian GD",
                DataPropertyName = nameof(AtmTransactionDTO.ThoiGianGD),
                FillWeight = 18,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm:ss" }
            });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái đối soát", DataPropertyName = nameof(AtmTransactionDTO.TrangThaiDoiSoat), FillWeight = 22 });
            dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ghi chú", DataPropertyName = nameof(AtmTransactionDTO.GhiChu), FillWeight = 25 });

            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.Columns.Clear();
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = nameof(ReconciliationLogDTO.MaDoiSoat), FillWeight = 8 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Thời gian chạy",
                DataPropertyName = nameof(ReconciliationLogDTO.ThoiGianChay),
                FillWeight = 20,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm:ss" }
            });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "GDV thực hiện", DataPropertyName = nameof(ReconciliationLogDTO.GiaoDichVien), FillWeight = 20 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tổng GD",
                DataPropertyName = nameof(ReconciliationLogDTO.TongGiaoDich),
                FillWeight = 14,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Hợp lệ",
                DataPropertyName = nameof(ReconciliationLogDTO.SoHopLe),
                FillWeight = 14,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Sai lệch",
                DataPropertyName = nameof(ReconciliationLogDTO.SoSaiLech),
                FillWeight = 10,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Trạng thái", DataPropertyName = nameof(ReconciliationLogDTO.TrangThai), FillWeight = 14 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Thời lượng (ms)",
                DataPropertyName = nameof(ReconciliationLogDTO.ThoiLuongMs),
                FillWeight = 14,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private void frmAtmReconciliation_Load(object? sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                dgvTransactions.DataSource = _bus.GetTransactions();
                dgvHistory.DataSource = _bus.GetHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu đối soát: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRunReconciliation_Click(object? sender, EventArgs e)
        {
            await StartReconciliationAsync();
        }

        public async Task StartReconciliationAsync()
        {
            if (_isRunning) return;

            _isRunning = true;
            _cts = new CancellationTokenSource();
            btnRunReconciliation.Enabled = false;
            btnResetPending.Enabled = false;
            btnStopReconciliation.Enabled = true;

            string operatorName = _mainForm?.CurrentOperator ?? "Phan Công Thành Đạt";
            lblProgressInfo.Text = "Trạng thái: Đang khởi tạo kết nối tải sao kê từ Core Banking...";
            lblSummary.Text = "Kết quả: Đang xử lý bất đồng bộ (giao diện vẫn thao tác bình thường)...";
            _mainForm?.UpdateReconciliationProgress(0, "Đang bắt đầu đối soát giao dịch ATM...");

            var progress = new Progress<ReconciliationProgressDTO>(p =>
            {
                if (this.IsDisposed) return;
                lblProgressInfo.Text = $"Trạng thái: {p.StatusText}";
                _mainForm?.UpdateReconciliationProgress(p.Percent, p.StatusText);
            });

            try
            {
                var result = await _bus.RunReconciliationAsync(operatorName, progress, _cts.Token);

                lblProgressInfo.Text = $"Trạng thái: Đối soát hoàn tất 100% ({result.TotalProcessed:N0} giao dịch).";
                lblSummary.Text = $"Kết quả: Tổng {result.TotalProcessed:N0} GD | Hợp lệ: {result.MatchedCount:N0} | Sai lệch: {result.MismatchCount:N0} | Thời gian: {result.DurationMs / 1000.0:F2} giây";
                _mainForm?.UpdateReconciliationProgress(100, $"Đối soát hoàn tất ({result.TotalProcessed:N0} GD - {result.DurationMs / 1000.0:F2}s)");
                LoadData();
            }
            catch (OperationCanceledException)
            {
                lblProgressInfo.Text = "Trạng thái: Đã dừng đối soát theo yêu cầu (CancellationToken).";
                lblSummary.Text = "Kết quả: Phiên đối soát đã được hủy an toàn bởi người dùng.";
                _mainForm?.ResetProgress("Đã dừng đối soát");
                LoadData();
            }
            catch (Exception ex)
            {
                lblProgressInfo.Text = "Trạng thái: Lỗi trong quá trình đối soát.";
                MessageBox.Show("Lỗi đối soát: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _mainForm?.ResetProgress("Lỗi đối soát");
            }
            finally
            {
                _isRunning = false;
                _cts?.Dispose();
                _cts = null;
                if (!this.IsDisposed)
                {
                    btnRunReconciliation.Enabled = true;
                    btnResetPending.Enabled = true;
                    btnStopReconciliation.Enabled = false;
                }
            }
        }

        private void btnStopReconciliation_Click(object? sender, EventArgs e)
        {
            CancelReconciliation();
        }

        public void CancelReconciliation()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                btnStopReconciliation.Enabled = false;
                lblProgressInfo.Text = "Trạng thái: Đang gửi tín hiệu hủy đối soát...";
                _cts.Cancel();
            }
        }

        private void btnResetPending_Click(object? sender, EventArgs e)
        {
            _bus.ResetTransactionsToPending();
            lblProgressInfo.Text = "Trạng thái: Đã đặt lại trạng thái giao dịch về 'Chờ đối soát'.";
            lblSummary.Text = "Kết quả: Sẵn sàng chạy đối soát mới.";
            _mainForm?.ResetProgress("Sẵn sàng");
            LoadData();
        }

        private void frmAtmReconciliation_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_isRunning && _cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
        }
    }
}
