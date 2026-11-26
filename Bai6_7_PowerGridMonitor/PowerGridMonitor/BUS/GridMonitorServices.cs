using System;
using System.Collections.Generic;
using PowerGridMonitor.DAL;
using PowerGridMonitor.DTO;

namespace PowerGridMonitor.BUS
{
    public class SubstationBUS
    {
        private readonly SubstationDAL _dal = new SubstationDAL();

        public List<SubstationDTO> GetAll()
        {
            return _dal.GetAll();
        }
    }

    public class GridMonitorBUS
    {
        private readonly GridIncidentDAL _incidentDal = new GridIncidentDAL();

        public List<GridIncidentLogDTO> GetRecentIncidents(int top = 50)
        {
            return _incidentDal.GetRecentLogs(top);
        }

        /// <summary>
        /// Sinh dữ liệu đo lường SCADA ngẫu nhiên: Điện áp (108kV - 115kV), Mức tải (50% - 98%).
        /// </summary>
        public ScadaTelemetryDTO GenerateTelemetrySample(SubstationDTO substation, int cycleCount)
        {
            double voltage = Math.Round(108.0 + Random.Shared.NextDouble() * 7.0, 1);
            int loadPercent = Random.Shared.Next(50, 99); // 50% - 98%
            bool isOverload = loadPercent > substation.NguongQuaTai; // > 85%

            var sample = new ScadaTelemetryDTO
            {
                MaTram = substation.MaTram,
                TenTram = substation.TenTram,
                Timestamp = DateTime.Now,
                VoltageKv = voltage,
                LoadPercent = loadPercent,
                CycleCount = cycleCount,
                IsOverload = isOverload,
                WorkerThreadId = Environment.CurrentManagedThreadId
            };

            if (isOverload)
            {
                try
                {
                    _incidentDal.Insert(new GridIncidentLogDTO
                    {
                        MaTram = substation.MaTram,
                        TenTram = substation.TenTram,
                        ThoiGian = sample.Timestamp,
                        DienApKV = sample.VoltageKv,
                        MucTaiPhanTram = sample.LoadPercent,
                        TrangThai = "Quá tải",
                        MoTa = $"Vượt ngưỡng an toàn (> {substation.NguongQuaTai}%)"
                    });
                }
                catch
                {
                    // Tránh ngắt vòng lặp SCADA nếu DB tạm gián đoạn
                }
            }

            return sample;
        }
    }
}
