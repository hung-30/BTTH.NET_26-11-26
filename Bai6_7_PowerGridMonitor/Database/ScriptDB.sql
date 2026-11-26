-- ========================================================
-- MÔN HỌC: LẬP TRÌNH .NET (C# WINFORMS 3-TIER)
-- BÀI TẬP 6.7: TRUNG TÂM ĐIỀU ĐỘ & GIÁM SÁT LƯỚI ĐIỆN THỜI GIAN THỰC (SCADA)
-- CSDL: PowerGridScadaDB
-- ========================================================

IF DB_ID('PowerGridScadaDB') IS NOT NULL
BEGIN
    ALTER DATABASE PowerGridScadaDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PowerGridScadaDB;
END
GO

CREATE DATABASE PowerGridScadaDB;
GO

USE PowerGridScadaDB;
GO

-- 1. Bảng Substations (Danh sách Trạm biến áp)
CREATE TABLE Substations (
    MaTram VARCHAR(20) PRIMARY KEY,
    TenTram NVARCHAR(150) NOT NULL,
    CapDienAp NVARCHAR(20) NOT NULL,
    KhuVuc NVARCHAR(100) NOT NULL,
    NguongDienApMin FLOAT NOT NULL DEFAULT 108.0,
    NguongDienApMax FLOAT NOT NULL DEFAULT 115.0,
    NguongQuaTai INT NOT NULL DEFAULT 85
);
GO

-- 2. Bảng GridIncidentLogs (Nhật ký sự cố quá tải SCADA)
CREATE TABLE GridIncidentLogs (
    MaSuCo INT IDENTITY(1,1) PRIMARY KEY,
    MaTram VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES Substations(MaTram) ON DELETE CASCADE,
    TenTram NVARCHAR(150) NOT NULL,
    ThoiGian DATETIME NOT NULL DEFAULT GETDATE(),
    DienApKV FLOAT NOT NULL,
    MucTaiPhanTram INT NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL,
    MoTa NVARCHAR(300) NOT NULL
);
GO

-- ========================================================
-- DỮ LIỆU MẪU (SAMPLE DATA)
-- ========================================================
INSERT INTO Substations (MaTram, TenTram, CapDienAp, KhuVuc, NguongDienApMin, NguongDienApMax, NguongQuaTai) VALUES
('TBA-HD-110', N'Trạm 110kV Hà Đông', N'110kV', N'Hà Nội - Khu vực Tây Nam', 108.0, 115.0, 85),
('TBA-CH-220', N'Trạm 220kV Chèm', N'220kV', N'Hà Nội - Khu vực Bắc', 108.0, 115.0, 85),
('TBA-MD-110', N'Trạm 110kV Mỹ Đình', N'110kV', N'Hà Nội - Khu vực Tây', 108.0, 115.0, 85),
('TBA-LB-220', N'Trạm 220kV Long Biên', N'220kV', N'Hà Nội - Khu vực Đông', 108.0, 115.0, 85),
('TBA-TT-500', N'Trạm 500kV Thường Tín', N'500kV', N'Hà Nội - Khu vực Nam', 108.0, 115.0, 85);
GO

INSERT INTO GridIncidentLogs (MaTram, TenTram, ThoiGian, DienApKV, MucTaiPhanTram, TrangThai, MoTa) VALUES
('TBA-HD-110', N'Trạm 110kV Hà Đông', '2026-11-26 08:10:05', 109.4, 89, N'Quá tải', N'Vượt ngưỡng an toàn (> 85%)'),
('TBA-CH-220', N'Trạm 220kV Chèm', '2026-11-26 08:25:42', 108.7, 94, N'Quá tải', N'Vượt ngưỡng an toàn (> 85%)');
GO

PRINT N'Khởi tạo CSDL PowerGridScadaDB thành công!';
GO
