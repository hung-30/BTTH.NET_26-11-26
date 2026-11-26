using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PowerGridMonitor.BUS;
using PowerGridMonitor.DTO;

namespace PowerGridMonitor.GUI
{
    public partial class frmPowerGridMonitor : Form
    {
        private readonly SubstationBUS _substationBus = new SubstationBUS();
        private readonly GridMonitorBUS _monitorBus = new GridMonitorBUS();
        private CancellationTokenSource? _cts;
        private int _totalCycles = 0;

        public bool IsMonitoring => _cts != null && !_cts.IsCancellationRequested;
        public int CurrentCycleCount => _totalCycles;
        public ScadaTelemetryDTO? LastTelemetry { get; private set; }

        public frmPowerGridMonitor()
        {
            InitializeComponent();
            ConfigureGridColumnsManually();
        }

        /// <summary>
        /// Cấu hình thủ công DataGridView (dgvGridLogs) bằng code theo yêu cầu đề bài.
        /// </summary>
        private void ConfigureGridColumnsManually()
        {
            dgvGridLogs.AutoGenerateColumns = false;
            dgvGridLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvGridLogs.Columns.Clear();

            dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colThoiGian",
                HeaderText = "Thời gian ghi nhận",
                FillWeight = 18
            });
            dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMaTram",
                HeaderText = "Mã trạm",
                FillWeight = 13
            });
            dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTenTram",
                HeaderText = "Tên trạm biến áp",
                FillWeight = 22
            });
            dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDienAp",
                HeaderText = "Điện áp (kV)",
                FillWeight = 12
            });
            dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMucTai",
                HeaderText = "Mức tải (%)",
                FillWeight = 12
            });
            dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTrangThai",
                HeaderText = "Trạng thái",
                FillWeight = 12
            });
            dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colMoTa",
                HeaderText = "Chi tiết cảnh báo",
                FillWeight = 25
            });
        }

        private void frmPowerGridMonitor_Load(object? sender, EventArgs e)
        {
            LoadSubstations();
            LoadHistoricalIncidents();
        }

        private void LoadSubstations()
        {
            try
            {
                var stations = _substationBus.GetAll();
                cboSubstation.DataSource = stations;
                cboSubstation.DisplayMember = nameof(SubstationDTO.TenTram);
                cboSubstation.ValueMember = nameof(SubstationDTO.MaTram);
                if (stations.Count > 0)
                    cboSubstation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách trạm biến áp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadHistoricalIncidents()
        {
            try
            {
                dgvGridLogs.Rows.Clear();
                var logs = _monitorBus.GetRecentIncidents(30);
                foreach (var log in logs)
                {
                    dgvGridLogs.Rows.Add(
                        log.ThoiGian.ToString("dd/MM/yyyy HH:mm:ss"),
                        log.MaTram,
                        log.TenTram,
                        $"{log.DienApKV:F1} kV",
                        $"{log.MucTaiPhanTram}%",
                        log.TrangThai,
                        log.MoTa
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải nhật ký sự cố từ CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboSubstation_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboSubstation.SelectedItem is SubstationDTO st)
            {
                lblStationDetail.Text = $"Trạm đang chọn: {st.MaTram} - {st.TenTram} ({st.KhuVuc})";
            }
        }

        private void btnStartScada_Click(object? sender, EventArgs e)
        {
            StartMonitoring();
        }

        public void StartMonitoring()
        {
            if (cboSubstation.SelectedItem is not SubstationDTO station)
            {
                MessageBox.Show("Vui lòng chọn Trạm biến áp trước khi giám sát!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_cts != null && !_cts.IsCancellationRequested)
                return;

            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;

            btnStartScada.Enabled = false;
            cboSubstation.Enabled = false;
            btnStopScada.Enabled = true;

            tslConnection.Text = $"SCADA: Đang giám sát {station.MaTram}";
            lblStationDetail.Text = $"Trạm đang giám sát: {station.MaTram} - {station.TenTram} ({station.KhuVuc}) | Chu kỳ: 2 giây/lần";

            // Khởi chạy Background Thread độc lập (Task.Run) lặp định kỳ 2 giây/lần
            Task.Run(() => ScadaBackgroundLoop(station, token), token);
        }

        private void ScadaBackgroundLoop(SubstationDTO station, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    int nextCycle = Interlocked.Increment(ref _totalCycles);
                    ScadaTelemetryDTO sample = _monitorBus.GenerateTelemetrySample(station, nextCycle);

                    // Cập nhật UI tuân thủ tuyệt đối Thread Safety qua InvokeRequired & Invoke
                    UpdateUiThreadSafe(sample);

                    // Định kỳ 2 giây / lần
                    try
                    {
                        if (token.WaitHandle.WaitOne(2000))
                        {
                            break;
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        /// <summary>
        /// Bắt buộc kiểm tra this.InvokeRequired và dùng this.Invoke(new Action(...))
        /// để chuyển ngữ cảnh từ Thread phụ về Main UI Thread.
        /// </summary>
        private void UpdateUiThreadSafe(ScadaTelemetryDTO sample)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            if (this.InvokeRequired)
            {
                try
                {
                    this.Invoke(new Action(() => UpdateUiThreadSafe(sample)));
                }
                catch (ObjectDisposedException)
                {
                }
                catch (InvalidOperationException)
                {
                }
                return;
            }

            // Đang chạy trên Main UI Thread: an toàn khi cập nhật mọi UI control
            LastTelemetry = sample;
            lblVoltage.Text = $"{sample.VoltageKv:F1} kV";

            if (sample.IsOverload)
            {
                lblLoadPercent.Text = $"{sample.LoadPercent}% - CẢNH BÁO QUÁ TẢI!";
                lblLoadPercent.ForeColor = Color.Red;

                // Tự động thêm dòng cảnh báo quá tải vào đầu DataGridView (dgvGridLogs)
                dgvGridLogs.Rows.Insert(
                    0,
                    sample.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"),
                    sample.MaTram,
                    sample.TenTram,
                    $"{sample.VoltageKv:F1} kV",
                    $"{sample.LoadPercent}%",
                    "Quá tải",
                    "Cảnh báo: Mức mang tải vượt ngưỡng an toàn (> 85%)"
                );
            }
            else
            {
                lblLoadPercent.Text = $"{sample.LoadPercent}% (Ổn định)";
                lblLoadPercent.ForeColor = Color.Green;
            }

            tslLastPacket.Text = $"Gói tin cuối: {sample.Timestamp:HH:mm:ss}";
            tslCycleCount.Text = $"Tổng chu kỳ đã quét: {sample.CycleCount} (Worker Thread #{sample.WorkerThreadId})";
        }

        private void btnStopScada_Click(object? sender, EventArgs e)
        {
            StopMonitoring();
        }

        public void StopMonitoring()
        {
            var oldCts = _cts;
            _cts = null;
            if (oldCts != null)
            {
                try
                {
                    if (!oldCts.IsCancellationRequested)
                        oldCts.Cancel();
                }
                catch (ObjectDisposedException)
                {
                }
            }

            btnStartScada.Enabled = true;
            cboSubstation.Enabled = true;
            btnStopScada.Enabled = false;
            tslConnection.Text = "SCADA: Đã ngắt kết nối";

            if (cboSubstation.SelectedItem is SubstationDTO st)
            {
                lblStationDetail.Text = $"Trạm đang chọn: {st.MaTram} - {st.TenTram} (Đã ngắt kết nối)";
            }
        }

        public void SelectSubstationByIndex(int index)
        {
            if (index >= 0 && index < cboSubstation.Items.Count)
            {
                cboSubstation.SelectedIndex = index;
            }
        }

        private void btnLoadHistory_Click(object? sender, EventArgs e)
        {
            LoadHistoricalIncidents();
        }

        private void frmPowerGridMonitor_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
        }
    }
}
