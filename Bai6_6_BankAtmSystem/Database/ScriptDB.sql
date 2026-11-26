-- ========================================================
-- MÔN HỌC: LẬP TRÌNH .NET (C# WINFORMS 3-TIER)
-- BÀI TẬP 6.6: HỆ THỐNG MDI ĐIỀU HÀNH MẠNG LƯỚI CÂY ATM NGÂN HÀNG
-- CSDL: BankAtmDB
-- ========================================================

IF DB_ID('BankAtmDB') IS NOT NULL
BEGIN
    ALTER DATABASE BankAtmDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BankAtmDB;
END
GO

CREATE DATABASE BankAtmDB;
GO

USE BankAtmDB;
GO

-- 1. Bảng AtmStations (Danh sách Trạm ATM & Tồn quỹ)
CREATE TABLE AtmStations (
    MaTram VARCHAR(20) PRIMARY KEY,
    DiaChi NVARCHAR(250) NOT NULL,
    TonQuy DECIMAL(18, 0) NOT NULL DEFAULT 0,
    TrangThai NVARCHAR(50) NOT NULL DEFAULT N'Hoạt động',
    AnhHienTruong NVARCHAR(500) NULL,
    NgayCapNhat DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- 2. Bảng AtmTransactions (Giao dịch ATM chờ đối soát cuối ngày)
CREATE TABLE AtmTransactions (
    MaGiaoDich INT IDENTITY(1,1) PRIMARY KEY,
    MaTram VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES AtmStations(MaTram) ON DELETE CASCADE,
    SoTheMasked VARCHAR(25) NOT NULL,
    LoaiGiaoDich NVARCHAR(50) NOT NULL,
    SoTien DECIMAL(18, 0) NOT NULL,
    ThoiGianGD DATETIME NOT NULL,
    TrangThaiDoiSoat NVARCHAR(50) NOT NULL DEFAULT N'Chờ đối soát',
    GhiChu NVARCHAR(250) NULL
);
GO

-- 3. Bảng ReconciliationLogs (Nhật ký các đợt đối soát cuối ngày)
CREATE TABLE ReconciliationLogs (
    MaDoiSoat INT IDENTITY(1,1) PRIMARY KEY,
    ThoiGianChay DATETIME NOT NULL DEFAULT GETDATE(),
    GiaoDichVien NVARCHAR(100) NOT NULL,
    TongGiaoDich INT NOT NULL,
    SoHopLe INT NOT NULL,
    SoSaiLech INT NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL,
    ThoiLuongMs INT NOT NULL
);
GO

-- ========================================================
-- DỮ LIỆU MẪU (SAMPLE DATA)
-- ========================================================
INSERT INTO AtmStations (MaTram, DiaChi, TonQuy, TrangThai, AnhHienTruong) VALUES
('ATM-HN-001', N'Số 198 Trần Quang Khải, Hoàn Kiếm, Hà Nội', 850000000, N'Hoạt động', NULL),
('ATM-HN-002', N'Số 18 Hoàng Quốc Việt, Cầu Giấy, Hà Nội', 420000000, N'Hoạt động', NULL),
('ATM-HN-003', N'Số 54 Nguyễn Chí Thanh, Đống Đa, Hà Nội', 95000000, N'Tồn quỹ thấp', NULL),
('ATM-HN-004', N'Khu đô thị Times City, Hai Bà Trưng, Hà Nội', 0, N'Bảo trì', NULL),
('ATM-HCM-001', N'Số 72 Lê Thánh Tôn, Quận 1, TP. Hồ Chí Minh', 1200000000, N'Hoạt động', NULL),
('ATM-DN-001', N'Số 120 Nguyễn Văn Linh, Hải Châu, Đà Nẵng', 310000000, N'Hoạt động', NULL);
GO

INSERT INTO AtmTransactions (MaTram, SoTheMasked, LoaiGiaoDich, SoTien, ThoiGianGD, TrangThaiDoiSoat, GhiChu) VALUES
('ATM-HN-001', '9704-xxxx-xxxx-1024', N'Rút tiền', 2000000, '2026-11-26 08:15:10', N'Chờ đối soát', NULL),
('ATM-HN-001', '9704-xxxx-xxxx-3318', N'Chuyển khoản', 15000000, '2026-11-26 08:22:45', N'Chờ đối soát', NULL),
('ATM-HN-002', '9704-xxxx-xxxx-5592', N'Rút tiền', 5000000, '2026-11-26 08:30:12', N'Chờ đối soát', N'Nghi vấn nhả thiếu tiền'),
('ATM-HN-003', '9704-xxxx-xxxx-7741', N'Rút tiền', 3000000, '2026-11-26 08:40:03', N'Chờ đối soát', NULL),
('ATM-HCM-001', '9704-xxxx-xxxx-8890', N'Chuyển khoản', 25000000, '2026-11-26 08:42:19', N'Chờ đối soát', NULL),
('ATM-DN-001', '9704-xxxx-xxxx-4420', N'Rút tiền', 1000000, '2026-11-26 08:44:50', N'Chờ đối soát', N'Timeout Core Banking');
GO

INSERT INTO ReconciliationLogs (ThoiGianChay, GiaoDichVien, TongGiaoDich, SoHopLe, SoSaiLech, TrangThai, ThoiLuongMs) VALUES
('2026-11-25 17:30:00', N'Phan Công Thành Đạt', 185000, 184996, 4, N'Hoàn tất', 4850);
GO

PRINT N'Khởi tạo CSDL BankAtmDB thành công!';
GO

