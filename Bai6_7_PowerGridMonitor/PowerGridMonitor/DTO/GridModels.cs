using System;

namespace PowerGridMonitor.DTO
{
    public class SubstationDTO
    {
        public string MaTram { get; set; } = string.Empty;
        public string TenTram { get; set; } = string.Empty;
        public string CapDienAp { get; set; } = string.Empty;
        public string KhuVuc { get; set; } = string.Empty;
        public double NguongDienApMin { get; set; } = 108.0;
        public double NguongDienApMax { get; set; } = 115.0;
        public int NguongQuaTai { get; set; } = 85;

        public override string ToString() => TenTram;
    }

    public class GridIncidentLogDTO
    {
        public int MaSuCo { get; set; }
        public string MaTram { get; set; } = string.Empty;
        public string TenTram { get; set; } = string.Empty;
        public DateTime ThoiGian { get; set; }
        public double DienApKV { get; set; }
        public int MucTaiPhanTram { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
    }

    public class ScadaTelemetryDTO
    {
        public string MaTram { get; set; } = string.Empty;
        public string TenTram { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public double VoltageKv { get; set; }
        public int LoadPercent { get; set; }
        public int CycleCount { get; set; }
        public bool IsOverload { get; set; }
        public int WorkerThreadId { get; set; }
    }
}
