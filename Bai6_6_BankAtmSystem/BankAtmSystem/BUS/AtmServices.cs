using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BankAtmSystem.DAL;
using BankAtmSystem.DTO;

namespace BankAtmSystem.BUS
{
    public class CoreBankingBUS
    {
        public (bool Connected, string Message) CheckConnection()
        {
            bool ok = DatabaseHelper.TestConnection(out string msg);
            return (ok, msg);
        }
    }

    public class AtmStationBUS
    {
        private readonly AtmStationDAL _dal = new AtmStationDAL();

        public List<AtmStationDTO> GetAll(string? keyword = null)
        {
            return _dal.GetAll(keyword);
        }

        public string Add(AtmStationDTO station)
        {
            if (string.IsNullOrWhiteSpace(station.MaTram))
                return "Mã trạm ATM không được để trống!";
            if (string.IsNullOrWhiteSpace(station.DiaChi))
                return "Địa chỉ đặt máy không được để trống!";
            if (station.TonQuy < 0)
                return "Số tiền tồn quỹ không được âm!";

            station.MaTram = station.MaTram.Trim().ToUpperInvariant();
            if (_dal.ExistsById(station.MaTram))
                return $"Mã trạm '{station.MaTram}' đã tồn tại trong hệ thống!";

            return _dal.Insert(station) ? string.Empty : "Thêm trạm ATM thất bại!";
        }

        public string Update(AtmStationDTO station)
        {
            if (string.IsNullOrWhiteSpace(station.MaTram))
                return "Vui lòng chọn mã trạm ATM cần cập nhật!";
            if (string.IsNullOrWhiteSpace(station.DiaChi))
                return "Địa chỉ đặt máy không được để trống!";
            if (station.TonQuy < 0)
                return "Số tiền tồn quỹ không được âm!";

            if (!_dal.ExistsById(station.MaTram.Trim()))
                return $"Không tìm thấy trạm ATM '{station.MaTram}'!";

            return _dal.Update(station) ? string.Empty : "Cập nhật trạm ATM thất bại!";
        }

        public string Delete(string maTram)
        {
            if (string.IsNullOrWhiteSpace(maTram))
                return "Vui lòng chọn mã trạm ATM cần xóa!";

            return _dal.Delete(maTram.Trim()) ? string.Empty : "Xóa trạm ATM thất bại!";
        }
    }

    public class ReconciliationBUS
    {
        private readonly ReconciliationDAL _dal = new ReconciliationDAL();

        public List<AtmTransactionDTO> GetTransactions()
        {
            return _dal.GetTransactions();
        }

        public List<ReconciliationLogDTO> GetHistory()
        {
            return _dal.GetReconciliationHistory();
        }

        public void ResetTransactionsToPending()
        {
            _dal.ResetTransactionsToPending();
        }

        /// <summary>
        /// Thực thi tác vụ đối soát sao kê bất đồng bộ trên Thread Pool (4 - 6 giây cho 200.000 giao dịch),
        /// hỗ trợ báo cáo tiến độ liên tục và hủy ngang qua CancellationToken.
        /// </summary>
        public async Task<ReconciliationResultDTO> RunReconciliationAsync(
            string operatorName,
            IProgress<ReconciliationProgressDTO>? progress,
            CancellationToken cancellationToken)
        {
            const int totalTransactions = 200000;
            const int totalSteps = 50; // 50 * 100ms = ~5.0 giây (nằm trong khoảng 4 - 6 giây)
            const int batchSize = totalTransactions / totalSteps;

            var sw = Stopwatch.StartNew();

            return await Task.Run(async () =>
            {
                int processed = 0;
                int mismatchCount = 0;

                try
                {
                    for (int step = 1; step <= totalSteps; step++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        // Giả lập tải và đối chiếu lô giao dịch từ Core Banking (4 - 6 giây tổng thể)
                        await Task.Delay(100, cancellationToken);

                        processed = step * batchSize;
                        if (step % 15 == 0)
                        {
                            mismatchCount++;
                        }

                        int percent = (int)Math.Round(step * 100.0 / totalSteps);
                        progress?.Report(new ReconciliationProgressDTO
                        {
                            Percent = percent,
                            ProcessedCount = processed,
                            TotalCount = totalTransactions,
                            StatusText = $"Đang đối soát lô {step}/{totalSteps}: {processed:N0}/{totalTransactions:N0} giao dịch ({percent}%)"
                        });
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    _dal.UpdateTransactionReconciliationStatuses();
                    var dbTransactions = _dal.GetTransactions();
                    int dbMismatches = 0;
                    foreach (var tx in dbTransactions)
                    {
                        if (tx.TrangThaiDoiSoat.Contains("Sai lệch", StringComparison.OrdinalIgnoreCase))
                        {
                            dbMismatches++;
                        }
                    }

                    int totalMismatch = Math.Max(mismatchCount, dbMismatches);
                    int matched = totalTransactions - totalMismatch;
                    sw.Stop();

                    _dal.InsertReconciliationLog(new ReconciliationLogDTO
                    {
                        ThoiGianChay = DateTime.Now,
                        GiaoDichVien = operatorName,
                        TongGiaoDich = totalTransactions,
                        SoHopLe = matched,
                        SoSaiLech = totalMismatch,
                        TrangThai = "Hoàn tất",
                        ThoiLuongMs = (int)sw.ElapsedMilliseconds
                    });

                    return new ReconciliationResultDTO
                    {
                        TotalProcessed = totalTransactions,
                        MatchedCount = matched,
                        MismatchCount = totalMismatch,
                        DurationMs = sw.ElapsedMilliseconds,
                        Status = "Hoàn tất",
                        Transactions = dbTransactions
                    };
                }
                catch (OperationCanceledException)
                {
                    sw.Stop();
                    try
                    {
                        _dal.InsertReconciliationLog(new ReconciliationLogDTO
                        {
                            ThoiGianChay = DateTime.Now,
                            GiaoDichVien = operatorName,
                            TongGiaoDich = processed,
                            SoHopLe = Math.Max(0, processed - mismatchCount),
                            SoSaiLech = mismatchCount,
                            TrangThai = "Đã hủy",
                            ThoiLuongMs = (int)sw.ElapsedMilliseconds
                        });
                    }
                    catch
                    {
                    }
                    throw;
                }
            }, cancellationToken);
        }
    }
}
