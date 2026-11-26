using System;
using System.Collections.Generic;

namespace BankAtmSystem.DTO
{
    public class AtmStationDTO
    {
        public string MaTram { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public decimal TonQuy { get; set; }
        public string TrangThai { get; set; } = "Hoạt động";
        public string? AnhHienTruong { get; set; }
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;
    }

    public class AtmTransactionDTO
    {
        public int MaGiaoDich { get; set; }
        public string MaTram { get; set; } = string.Empty;
        public string SoTheMasked { get; set; } = string.Empty;
        public string LoaiGiaoDich { get; set; } = string.Empty;
        public decimal SoTien { get; set; }
        public DateTime ThoiGianGD { get; set; }
        public string TrangThaiDoiSoat { get; set; } = "Chờ đối soát";
        public string GhiChu { get; set; } = string.Empty;
    }

    public class ReconciliationLogDTO
    {
        public int MaDoiSoat { get; set; }
        public DateTime ThoiGianChay { get; set; }
        public string GiaoDichVien { get; set; } = string.Empty;
        public int TongGiaoDich { get; set; }
        public int SoHopLe { get; set; }
        public int SoSaiLech { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public int ThoiLuongMs { get; set; }
    }

    public class ReconciliationProgressDTO
    {
        public int Percent { get; set; }
        public int ProcessedCount { get; set; }
        public int TotalCount { get; set; }
        public string StatusText { get; set; } = string.Empty;
    }

    public class ReconciliationResultDTO
    {
        public int TotalProcessed { get; set; }
        public int MatchedCount { get; set; }
        public int MismatchCount { get; set; }
        public long DurationMs { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<AtmTransactionDTO> Transactions { get; set; } = new List<AtmTransactionDTO>();
    }
}
