using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BankAtmSystem.DTO;

namespace BankAtmSystem.DAL
{
    public class ReconciliationDAL
    {
        public List<AtmTransactionDTO> GetTransactions()
        {
            var list = new List<AtmTransactionDTO>();
            string sql = @"SELECT MaGiaoDich, MaTram, SoTheMasked, LoaiGiaoDich, SoTien, ThoiGianGD, TrangThaiDoiSoat, GhiChu
                           FROM AtmTransactions
                           ORDER BY ThoiGianGD DESC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new AtmTransactionDTO
                {
                    MaGiaoDich = Convert.ToInt32(row["MaGiaoDich"]),
                    MaTram = row["MaTram"].ToString() ?? string.Empty,
                    SoTheMasked = row["SoTheMasked"].ToString() ?? string.Empty,
                    LoaiGiaoDich = row["LoaiGiaoDich"].ToString() ?? string.Empty,
                    SoTien = Convert.ToDecimal(row["SoTien"]),
                    ThoiGianGD = Convert.ToDateTime(row["ThoiGianGD"]),
                    TrangThaiDoiSoat = row["TrangThaiDoiSoat"].ToString() ?? "Chờ đối soát",
                    GhiChu = row["GhiChu"] == DBNull.Value ? string.Empty : row["GhiChu"].ToString() ?? string.Empty
                });
            }
            return list;
        }

        public void UpdateTransactionReconciliationStatuses()
        {
            string sql = @"
                UPDATE AtmTransactions
                SET TrangThaiDoiSoat = CASE
                    WHEN GhiChu IS NOT NULL AND LTRIM(RTRIM(GhiChu)) <> '' THEN N'Sai lệch cần kiểm tra'
                    ELSE N'Đã đối soát - Khớp'
                END;";
            DatabaseHelper.ExecuteNonQuery(sql);
        }

        public void ResetTransactionsToPending()
        {
            string sql = "UPDATE AtmTransactions SET TrangThaiDoiSoat = N'Chờ đối soát';";
            DatabaseHelper.ExecuteNonQuery(sql);
        }

        public void InsertReconciliationLog(ReconciliationLogDTO log)
        {
            string sql = @"INSERT INTO ReconciliationLogs (ThoiGianChay, GiaoDichVien, TongGiaoDich, SoHopLe, SoSaiLech, TrangThai, ThoiLuongMs)
                           VALUES (@ThoiGianChay, @GiaoDichVien, @TongGiaoDich, @SoHopLe, @SoSaiLech, @TrangThai, @ThoiLuongMs)";
            SqlParameter[] parameters =
            {
                new SqlParameter("@ThoiGianChay", SqlDbType.DateTime) { Value = log.ThoiGianChay },
                new SqlParameter("@GiaoDichVien", SqlDbType.NVarChar, 100) { Value = log.GiaoDichVien },
                new SqlParameter("@TongGiaoDich", SqlDbType.Int) { Value = log.TongGiaoDich },
                new SqlParameter("@SoHopLe", SqlDbType.Int) { Value = log.SoHopLe },
                new SqlParameter("@SoSaiLech", SqlDbType.Int) { Value = log.SoSaiLech },
                new SqlParameter("@TrangThai", SqlDbType.NVarChar, 50) { Value = log.TrangThai },
                new SqlParameter("@ThoiLuongMs", SqlDbType.Int) { Value = log.ThoiLuongMs }
            };
            DatabaseHelper.ExecuteNonQuery(sql, parameters);
        }

        public List<ReconciliationLogDTO> GetReconciliationHistory()
        {
            var list = new List<ReconciliationLogDTO>();
            string sql = @"SELECT TOP 20 MaDoiSoat, ThoiGianChay, GiaoDichVien, TongGiaoDich, SoHopLe, SoSaiLech, TrangThai, ThoiLuongMs
                           FROM ReconciliationLogs
                           ORDER BY MaDoiSoat DESC";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ReconciliationLogDTO
                {
                    MaDoiSoat = Convert.ToInt32(row["MaDoiSoat"]),
                    ThoiGianChay = Convert.ToDateTime(row["ThoiGianChay"]),
                    GiaoDichVien = row["GiaoDichVien"].ToString() ?? string.Empty,
                    TongGiaoDich = Convert.ToInt32(row["TongGiaoDich"]),
                    SoHopLe = Convert.ToInt32(row["SoHopLe"]),
                    SoSaiLech = Convert.ToInt32(row["SoSaiLech"]),
                    TrangThai = row["TrangThai"].ToString() ?? string.Empty,
                    ThoiLuongMs = Convert.ToInt32(row["ThoiLuongMs"])
                });
            }
            return list;
        }
    }
}
