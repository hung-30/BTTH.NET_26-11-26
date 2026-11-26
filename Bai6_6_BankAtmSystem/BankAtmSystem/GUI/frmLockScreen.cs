using System;
using System.Windows.Forms;

namespace BankAtmSystem.GUI
{
    public class frmLockScreen : Form
    {
        private readonly bool _isLogout;
        private TextBox txtOperator = null!;
        private TextBox txtPin = null!;
        private Button btnConfirm = null!;
        private Button btnExit = null!;

        public string OperatorName { get; private set; }

        public frmLockScreen(string currentOperator, bool isLogout)
        {
            _isLogout = isLogout;
            OperatorName = currentOperator;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = _isLogout ? "Đăng nhập Giao dịch viên" : "Khóa màn hình hệ thống";
            this.Size = new System.Drawing.Size(380, 210);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblInfo = new Label
            {
                Text = _isLogout
                    ? "Phiên làm việc đã đăng xuất. Vui lòng đăng nhập lại:"
                    : "Hệ thống đang tạm khóa. Nhập mã PIN (1234) để mở khóa:",
                Location = new System.Drawing.Point(18, 16),
                Size = new System.Drawing.Size(330, 32)
            };

            var lblOperator = new Label
            {
                Text = "Tên GDV:",
                Location = new System.Drawing.Point(18, 56),
                AutoSize = true
            };
            txtOperator = new TextBox
            {
                Location = new System.Drawing.Point(110, 53),
                Width = 230,
                Text = OperatorName,
                ReadOnly = !_isLogout
            };

            var lblPin = new Label
            {
                Text = "Mã PIN:",
                Location = new System.Drawing.Point(18, 92),
                AutoSize = true
            };
            txtPin = new TextBox
            {
                Location = new System.Drawing.Point(110, 89),
                Width = 230,
                UseSystemPasswordChar = true,
                Text = "1234"
            };

            btnConfirm = new Button
            {
                Text = _isLogout ? "Đăng nhập" : "Mở khóa",
                Location = new System.Drawing.Point(168, 128),
                Size = new System.Drawing.Size(85, 28),
                UseVisualStyleBackColor = true
            };
            btnConfirm.Click += BtnConfirm_Click;

            btnExit = new Button
            {
                Text = "Thoát CT",
                Location = new System.Drawing.Point(258, 128),
                Size = new System.Drawing.Size(82, 28),
                UseVisualStyleBackColor = true
            };
            btnExit.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.AddRange(new Control[] { lblInfo, lblOperator, txtOperator, lblPin, txtPin, btnConfirm, btnExit });
            this.AcceptButton = btnConfirm;
        }

        private void BtnConfirm_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOperator.Text))
            {
                MessageBox.Show("Vui lòng nhập tên Giao dịch viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPin.Text.Trim() != "1234")
            {
                MessageBox.Show("Mã PIN không chính xác! (PIN mặc định: 1234)", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OperatorName = txtOperator.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
